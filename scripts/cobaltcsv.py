import json
import csv
import os
from typing import List

story_json_path = r'C:\Program Files (x86)\Steam\steamapps\common\Cobalt Core\Data\story.json'

who_allow_list = {'hacker', 'eunice', 'dizzy', 'goat', 'riggs', 'shard', 'peri', 'comp'}

output_dir = os.path.normpath(os.path.join(__file__, '..', '..', 'Tera'))


def append_line_rows(indices: List[str], rows, event_name: str, line_data) -> bool:
    did_append = False

    line_type = line_data['$type']
    if line_type == 'Say, CobaltCore':
        who = line_data['who']
        if who in who_allow_list:
            what = line_data['what']
            loop_tag = line_data.get('loopTag', '')
            rows.append((event_name, ','.join(indices), who, what, loop_tag))
            did_append = True

    elif line_type == 'SaySwitch, CobaltCore':
        for i, line in enumerate(line_data['lines']):
            did_append |= append_line_rows(indices + [str(i)], rows, event_name, line)

    return did_append


def get_stock_story():
    with open(story_json_path) as f:
        story_json = json.load(f)
    return story_json


def main():
    story_json = get_stock_story()
    all = story_json['all']

    rows = [('EventName', 'LineIndex', 'Who', 'What', 'LoopTag')]

    for event_name, data in all.items():
        had_event = False

        lines = data.get('lines')
        if not lines:
            continue

        for i, line in enumerate(data['lines']):
            line_type = line['$type']

            # if line_type != 'SaySwitch, CobaltCore':
            #     continue

            had_event |= append_line_rows([str(i)], rows, event_name, line)

        if had_event:
            rows.append(('', '', '', '', ''))

    with open('story.csv', 'w') as f:
        writer = csv.writer(f, lineterminator='\n')
        writer.writerows(rows)


if __name__ == '__main__':
    main()
