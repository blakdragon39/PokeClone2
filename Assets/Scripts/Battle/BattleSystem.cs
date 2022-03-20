using System;
using UnityEngine;

enum BattleStage {
    PreBattle
}

public class BattleSystem : MonoBehaviour {
    [SerializeField] private EnemyUnit enemyUnitBackLeft;
    [SerializeField] private EnemyUnit enemyUnitBackRight;
    [SerializeField] private EnemyUnit enemyUnitFront;

    [SerializeField] private BattleOptions preBattleOptions;

    private BattleStage battleStage;
    private int selectedOption;

    private void Start() {
        battleStage = BattleStage.PreBattle;
        selectedOption = 0;

        SetupBattle();
    }

    private void Update() {
        if (battleStage == BattleStage.PreBattle) {
            HandlePreBattleSelection();
        }

        preBattleOptions.SetSelectedOption(selectedOption);
    }

    private void SetupBattle() {
        enemyUnitBackLeft.Setup();
        enemyUnitBackRight.Setup();
        enemyUnitFront.Setup();
    }

    private void HandlePreBattleSelection() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            selectedOption += 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            selectedOption -= 1;
        }

        if (selectedOption >= preBattleOptions.options.Count) {
            selectedOption = preBattleOptions.options.Count - 1;
        } else if (selectedOption < 0) {
            selectedOption = 0;
        }
    }
}