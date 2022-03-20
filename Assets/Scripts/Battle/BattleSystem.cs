using UnityEngine;

enum BattleStage {
    PreBattle, InBattle
}

public class BattleSystem : MonoBehaviour {
    [SerializeField] private EnemyUnit enemyUnitBackLeft;
    [SerializeField] private EnemyUnit enemyUnitBackRight;
    [SerializeField] private EnemyUnit enemyUnitFront;

    [SerializeField] private BattleOptions preBattleOptions;
    [SerializeField] private BattleOptions inBattleOptions;

    private BattleStage battleStage;
    private int selectedOption;

    private void Start() {
        battleStage = BattleStage.PreBattle;
        selectedOption = 0;

        SetupBattle();
    }

    private void Update() {
        if (battleStage == BattleStage.PreBattle) {
            HandleOptionSelection(preBattleOptions);
            HandlePreBattleAdvance();
            preBattleOptions.gameObject.SetActive(true);
            inBattleOptions.gameObject.SetActive(false);
            preBattleOptions.SetSelectedOption(selectedOption);
        } else if (battleStage == BattleStage.InBattle) {
            HandleOptionSelection(inBattleOptions);
            inBattleOptions.gameObject.SetActive(true);
            preBattleOptions.gameObject.SetActive(false);
            inBattleOptions.SetSelectedOption(selectedOption);
        }
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
        //todo make sure correct option is selected?
        if (Input.GetKeyDown(KeyCode.Return)) {
            battleStage = BattleStage.InBattle;
        }
    }
}