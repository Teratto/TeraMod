import argparse
import json
import os
import urllib.request

import cobaltcsv
import csv


argparser = argparse.ArgumentParser()
argparser.add_argument('--pull', action='store_true')
argparser.add_argument('--pull-url', default='https://docs.google.com/spreadsheets/d/e/2PACX-1vQPfRL9O3V5c59bw0SEqgE_6zy5TrLMStezB89IN1YEo1C72xEFVxivUN8lEKoEzA/pub?gid=1016104500&single=true&output=csv')


def main():
    args = argparser.parse_args()

    if args.pull:
        with urllib.request.urlopen(args.pull_url) as response:
            with open('story_input.csv', 'wb') as f:
                f.write(response.read())

    with open('story_input.csv') as f:
        csv_reader = csv.reader(f, lineterminator='\n')

        story_json = cobaltcsv.get_stock_story()
        all = story_json['all']

        items = []

        seen_header = False

        for line in csv_reader:
            node_name, line_index, who, what, loop_tag = line[:5]

            if not seen_header:
                seen_header = True
                continue

            if not node_name or node_name.lower().startswith('note') or node_name.startswith('//'):
                continue

            indices = line_index.split(',')
            parsed_indices = [int(index.strip()) for index in indices]

            # Is this a new item?
            is_new = False

            stock_event = all.get(node_name)
            parent_is_say_switch = False
            if stock_event:
                lines = stock_event['lines']
                for index in parsed_indices:
                    if not lines:
                        is_new = True
                        break

                    if index >= len(lines):
                        is_new = True
                        break

                    entry = lines[index]
                    parent_is_say_switch = entry['$type'] == 'SaySwitch, CobaltCore'
                    lines = entry.get('lines')

            else:
                is_new = True

            if not is_new:
                continue

            # This is new!
            print('Found new line: ' + ', '.join(line))

            # Add a prefix if this corresponds to a StoryNode not in the stock game.
            prefix = ''
            if node_name not in all:
                prefix = cobaltcsv.NODE_PREFIX

            payload = {
                'event_name': prefix + node_name,
                'indices': parsed_indices,
                'who': who,
                'what': what,
                'loop_tag': loop_tag,
                'is_inserted': parent_is_say_switch
            }
            items.append(payload)

    with open(os.path.join(cobaltcsv.output_dir, 'story.json'), 'w') as f:
        json.dump({'items': items}, f, indent=4)


if __name__ == '__main__':
    main()
