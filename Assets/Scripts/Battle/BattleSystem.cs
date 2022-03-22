using System.Collections;
using UnityEngine;

enum BattleStage {
    Busy, PreBattle, SelectingAttack, SelectingEnemy
}

public class BattleSystem : MonoBehaviour {
    [SerializeField] private EnemyUnit enemyUnitBackLeft;
    [SerializeField] private EnemyUnit enemyUnitBackRight;
    [SerializeField] private EnemyUnit enemyUnitFront;

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
        if (battleStage == BattleStage.PreBattle) {
            HandleOptionSelection(preBattleOptions);
            HandlePreBattleAdvance();
            preBattleOptions.SetSelectedOption(selectedOption);
        } else if (battleStage == BattleStage.SelectingAttack) {
            HandleOptionSelection(attackSelectionOptions);
            HandleAttackSelection();
            attackSelectionOptions.SetSelectedOption(selectedOption);
        } else if (battleStage == BattleStage.SelectingEnemy) {
            HandleOptionSelection(enemySelectionOptions);
            HandleEnemySelection();
            enemySelectionOptions.SetSelectedOption(selectedOption);
        }
        
        preBattleOptions.gameObject.SetActive(battleStage == BattleStage.PreBattle);
        attackSelectionOptions.gameObject.SetActive(battleStage == BattleStage.SelectingAttack);
        enemySelectionOptions.gameObject.SetActive(battleStage == BattleStage.SelectingEnemy);
        
        enemyUnitBackLeft.SetArrowVisible(battleStage == BattleStage.SelectingEnemy && selectedOption == 0);
        enemyUnitFront.SetArrowVisible(battleStage == BattleStage.SelectingEnemy && selectedOption == 1);
        enemyUnitBackRight.SetArrowVisible(battleStage == BattleStage.SelectingEnemy && selectedOption == 2);
    }

    private void SetupBattle() {
        enemyUnitBackLeft.Setup();
        enemyUnitBackRight.Setup();
        enemyUnitFront.Setup();
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
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (selectedOption == 0) {
                battleStage = BattleStage.SelectingAttack;    
            } else if (selectedOption == 1) {
                //todo flee
            }

            selectedOption = 0; //todo remember option selection?
        }
    }

    private void HandleAttackSelection() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            selectedAttackOption = selectedOption;
            battleStage = BattleStage.SelectingEnemy;
            selectedOption = 0; //todo remember option selection?
        }
    }

    private void HandleEnemySelection() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            var enemy = enemyUnitBackLeft;
            var type = AttackType.Magic;

            if (selectedOption == 0) {
                enemy = enemyUnitBackLeft;
            }
            else if (selectedOption == 1) {
                enemy = enemyUnitFront;
            }
            else if (selectedOption == 2) {
                enemy = enemyUnitBackRight;
            }

            if (selectedAttackOption == 0) {
                type = AttackType.Magic;
            }
            else if (selectedAttackOption == 1) {
                type = AttackType.Throw;
            }
            else if (selectedAttackOption == 2) {
                type = AttackType.Smack;
            }

            StartCoroutine(StartAttack(enemy, type));
            selectedOption = 0; //todo remember option selection?
        }
    }

    private IEnumerator StartAttack(EnemyUnit unit, AttackType type) {
        battleStage = BattleStage.Busy;
        yield return unit.Attack(type);
        battleStage = BattleStage.SelectingAttack; //todo move to enemy attack
    } 
}