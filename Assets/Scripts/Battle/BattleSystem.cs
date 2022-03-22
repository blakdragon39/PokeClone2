using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BattleStage {
    Busy, PreBattle, SelectingAttack, SelectingEnemy
}

public class BattleSystem : MonoBehaviour {
    
    [SerializeField] private List<EnemyUnit> enemies;

    [SerializeField] private BattleOptions preBattleOptions;
    [SerializeField] private BattleOptions attackSelectionOptions;
    [SerializeField] private BattleOptions enemySelectionOptions;

    private BattleStage battleStage;
    private int selectedOption;
    private int selectedAttackOption;

    private void Start() {
        battleStage = BattleStage.PreBattle;
        selectedOption = 0;

        SetupBattle();
    }

    private void Update() {
        switch (battleStage) {
            case BattleStage.PreBattle:
                HandleOptionSelection(preBattleOptions);
                HandlePreBattleAdvance();
                preBattleOptions.SetSelectedOption(selectedOption);
                break;
            case BattleStage.SelectingAttack:
                HandleOptionSelection(attackSelectionOptions);
                HandleAttackSelection();
                attackSelectionOptions.SetSelectedOption(selectedOption);
                break;
            case BattleStage.SelectingEnemy:
                HandleOptionSelection(enemySelectionOptions);
                HandleEnemySelection();
                enemySelectionOptions.SetSelectedOption(selectedOption);
                break;
            case BattleStage.Busy:
            default:
                break;
        }
        
        preBattleOptions.gameObject.SetActive(battleStage == BattleStage.PreBattle);
        attackSelectionOptions.gameObject.SetActive(battleStage == BattleStage.SelectingAttack);
        enemySelectionOptions.gameObject.SetActive(battleStage == BattleStage.SelectingEnemy);

        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].SetArrowVisible(battleStage == BattleStage.SelectingEnemy && i == selectedOption);
        }
    }

    private void SetupBattle() {
        foreach (var enemy in enemies) {
            enemy.Setup();
        }
    }

    private void HandleOptionSelection(BattleOptions options) {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            selectedOption += 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            selectedOption -= 1;
        }

        if (selectedOption >= options.options.Count) {
            selectedOption = options.options.Count - 1;
        } else if (selectedOption < 0) {
            selectedOption = 0;
        }
    }

    private void HandlePreBattleAdvance() {
        if (!Input.GetKeyDown(KeyCode.Return)) return;

        switch (selectedOption) {
            case 0:
                battleStage = BattleStage.SelectingAttack;
                break;
            case 1:
                //todo flee
                break;
        }

        selectedOption = 0; //todo remember option selection?
    }

    private void HandleAttackSelection() {
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        
        selectedAttackOption = selectedOption;
        battleStage = BattleStage.SelectingEnemy;
        selectedOption = 0; //todo remember option selection?
    }

    private void HandleEnemySelection() {
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        
        var enemy = enemies[selectedOption];
        var type = AttackType.Magic;
        
        if (selectedAttackOption == 0) {
            type = AttackType.Magic;
        } else if (selectedAttackOption == 1) {
            type = AttackType.Throw;
        } else if (selectedAttackOption == 2) {
            type = AttackType.Smack;
        }

        StartCoroutine(StartAttack(enemy, type));
        selectedOption = 0; //todo remember option selection?
    }

    private IEnumerator StartAttack(EnemyUnit unit, AttackType type) {
        battleStage = BattleStage.Busy;
        yield return unit.Attack(type);
        battleStage = BattleStage.SelectingAttack; //todo move to enemy attack
    } 
}