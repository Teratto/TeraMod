import csv

import cobaltcsv


fields = {
    'type': 'string',
    'bg': 'string',
    'bgSetup': 'string[]',
    'once': 'bool',
    'oncePerRun': 'bool',
    'oncePerCombat': 'bool',
    'oncePerRunTags': 'string[]',
    'oncePerCombatTags': 'string[]',
    'priority': 'bool',
    'never': 'bool?',
    'canSpawnOnMap': 'bool?',
    'choiceFunc': 'string?',
    'choiceText': 'string?',
    'dontCountForProgression': 'bool',
    'lookup': 'HashSet<string>?',
    'zones': 'HashSet<string>?',
    'anyDrones': 'HashSet<string>?',
    'anyDronesHostile': 'HashSet<string>?',
    'anyDronesFriendly': 'HashSet<string>?',
    'lastNamedDroneSpawned': 'string?',
    'lastNamedDroneDestroyed': 'string?',
    'minRuns': 'int?',
    'minHullPercent': 'double?',
    'maxHullPercent': 'double?',
    'minHull': 'int?',
    'maxHull': 'int?',
    'requiredScenes': 'HashSet<string>',
    'excludedScenes': 'HashSet<string>',
    'handEmpty': 'bool?',
    'handFullOfTrash': 'bool?',
    'handFullOfUnplayableCards': 'bool?',
    'minEnergy': 'int?',
    'playerShotJustHit': 'bool?',
    'playerShotJustMissed': 'bool?',
    'playerShotWasFromStrafe': 'bool?',
    'playerShotWasFromPayback': 'bool?',
    'playerJustShuffledDiscardIntoDrawPile': 'bool?',
    'shipsDontOverlapAtAll': 'bool?',
    'whoDidThat': 'Deck?',
    'minCostOfCardJustPlayed': 'int?',
    'maxCostOfCardJustPlayed': 'int?',
    'enemyShotJustHit': 'bool?',
    'enemyShotJustMissed': 'bool?',
    'allPresent': 'HashSet<string>?',
    'nonePresent': 'HashSet<string>?',
    'requireCharsLocked': 'HashSet<string>?',
    'requireCharsUnlocked': 'HashSet<string>?',
    'hasArtifacts': 'HashSet<string>?',
    'doesNotHaveArtifacts': 'HashSet<string>?',
    'minCombatsThisRun': 'int?',
    'lastDeathZone': 'string?',
    'lastTurnPlayerStatuses': 'HashSet<Status>?',
    'lastTurnEnemyStatuses': 'HashSet<Status>?',
    'enemyHasPart': 'string?',
    'enemyDoesNotHavePart': 'string?',
    'enemyHasWeakPart': 'bool?',
    'enemyHasBrittlePart': 'bool?',
    'enemyHasArmoredPart': 'bool?',
    'minCardsPlayedThisTurn': 'int?',
    'minMovesThisTurn': 'int?',
    'minTurnsThisCombat': 'int?',
    'maxTurnsThisCombat': 'int?',
    'turnStart': 'bool?',
    'enemyIntent': 'string?',
    'minDamageDealtToEnemyThisTurn': 'int?',
    'minDamageDealtToPlayerThisTurn': 'int?',
    'maxDamageDealtToPlayerThisTurn': 'int?',
    'minDamageDealtToEnemyThisAction': 'int?',
    'maxDamageDealtToEnemyThisAction': 'int?',
    'minDamageBlockedByPlayerArmorThisTurn': 'int?',
    'minDamageBlockedByEnemyArmorThisTurn': 'int?',
    'maxDamageBlockedByEnemyArmorThisTurn': 'int?',
    'playerJustPiercedEnemyArmor': 'bool?',
    'goingToOverheat': 'bool?',
    'wasGoingToOverheatButStopped': 'bool?',
    'justOverheated': 'bool?',
    'playerJustShotAMidrowObject': 'bool?',
    'playerJustShotASoccerBall': 'bool?',
    'spikeName': 'string?',
    'minWinCount': 'int?',
    'minTimesYouFlippedACardThisTurn': 'int?',
    'specialFight': 'string?',
    'demo': 'bool?',
    'pax': 'bool?',
    'introDelay': 'bool?',
}


def main():
    with open('nodeprops.csv', 'w', newline='') as f:
        csvwriter = csv.writer(f)

        csvwriter.writerow(['name'] + list(fields.keys()))

        story_json = cobaltcsv.get_stock_story()
        all = story_json['all']
        for event_name, node in all.items():
            row = [event_name]
            for field, field_type in fields.items():
                field_type = field_type.replace('?', '')
                val = node.get(field, None)
                if field_type.startswith('HashSet<') or field_type.startswith('List<') or field_type == 'string[]':
                    if val:
                        val = ','.join(val)

                row.append(val)

            csvwriter.writerow(row)


if __name__ == '__main__':
    main()
