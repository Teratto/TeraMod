import argparse
import csv
import json
import os
import sys
import urllib.request

import cobaltcsv
import dump_nodes


argparser = argparse.ArgumentParser()
argparser.add_argument('--pull', action='store_true')
argparser.add_argument('--pull-url', default='https://docs.google.com/spreadsheets/d/e/2PACX-1vRBgeaQaPwCPWXNX0ZM_WmPOT82MhSwNAOcGUf5YKGhbLn6IsrmcnGvk-ucwW_IOg/pub?gid=795863446&single=true&output=csv')


had_error = False


def err(msg: str):
    global had_error
    sys.stderr.write(msg)
    sys.stderr.write('\n')
    sys.stderr.flush()
    had_error = True


def main():
    args = argparser.parse_args()

    if args.pull:
        with urllib.request.urlopen(args.pull_url) as response:
            with open('nodes_input.csv', 'wb') as f:
                f.write(response.read())

    nodes = {}

    with open('nodes_input.csv') as f:
        csv_reader = csv.reader(f, lineterminator='\n')

        story_json = cobaltcsv.get_stock_story()
        all = story_json['all']

        seen_header = False
        fields = []

        for line in csv_reader:
            if not seen_header:
                # Google sheets csv files have a weird line at the very front...
                if len(line) > 1:
                    seen_header = True
                    fields = line

                continue

            if len(line) == 0:
                continue

            values = {fields[i]: val for i, val in enumerate(line)}
            node = {}

            event_name = values['name'].strip()

            if not event_name or event_name.startswith('//'):
                continue

            if event_name in all:
                # Exists in stock game. Ignore.
                continue

            print(f'Found new node {event_name}')

            for field_name, val in values.items():
                if field_name == 'name':
                    continue
                val_type = dump_nodes.fields[field_name]
                val_type = val_type.replace('?', '')

                if not val.strip():
                    # Ignore 'null' values.
                    continue

                if val_type == 'string' or val_type == 'Deck':
                    pass
                elif val_type == 'int':
                    try:
                        val = int(val)
                    except ValueError:
                        err(f'Failed to convert value {val} to int for field {field_name}')
                        val = 0
                elif val_type == 'bool':
                    val = bool(val)
                elif val_type == 'string[]' or val_type.startswith('HashSet<') or val_type.startswith('List<'):
                    val = [x.strip() for x in val.split(',')]
                    val = ['Teratto.TeraMod.Tera' if x.lower() == 'tera' else x for x in val]
                elif val_type == 'double':
                    try:
                        val = float(val)
                    except ValueError:
                        err(f'Failed to convert value {val} to int for field {field_name}')
                        val = 0.0
                else:
                    err(f'!! DONT KNOW HOW TO PROCESS {val_type}!!')

                node[field_name] = val

            nodes[event_name] = node

    with open(os.path.join(cobaltcsv.output_dir, 'story_nodes.json'), 'w') as f:
        json.dump(nodes, f, indent=4)

    if had_error:
        err('THERE WERE ERRORS!! SCROLL UP AND FIX THEM!!!')


if __name__ == '__main__':
    main()
