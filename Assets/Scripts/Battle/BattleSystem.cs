using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

enum BattleStage {
    Busy, PreBattle, SelectingAttack, SelectingEnemy, EnemyAttack
}

public class BattleSystem : MonoBehaviour {

    public event Action<bool> OnBattleEnded;

    [SerializeField] private PlayerUnit playerUnit;
    [SerializeField] private List<EnemyUnit> enemies;
    private List<EnemyUnit> defeatedEnemies = new List<EnemyUnit>();

    [SerializeField] private DialogText dialog;
    [SerializeField] private BattleOptions preBattleOptions;
    [SerializeField] private BattleOptions attackSelectionOptions;
    [SerializeField] private BattleOptions enemySelectionOptions;

    private BattleStage battleStage;
    private int selectedOption;
    private int selectedAttackOption;

    public void StartBattle() {
        battleStage = BattleStage.PreBattle;
        selectedOption = 0;

        SetupBattle();
    }

    public void HandleUpdate() {
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
            case BattleStage.EnemyAttack:
                StartCoroutine(AttackEnemy());
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
        if (
            Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow) && battleStage == BattleStage.SelectingEnemy
        ) {
            selectedOption += 1;
        } else if (
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) && battleStage == BattleStage.SelectingEnemy
        ) {
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
                OnBattleEnded(true);
                break;
        }

        selectedOption = 0;
    }

    private void HandleAttackSelection() {
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        //todo need a way to move backwards!
        
        selectedAttackOption = selectedOption;
        battleStage = BattleStage.SelectingEnemy;
        selectedOption = 0;
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

        StartCoroutine(AttackEnemy(enemy, type));
        selectedOption = 0;
    }

    private IEnumerator AttackEnemy(EnemyUnit unit, AttackType type) {
        battleStage = BattleStage.Busy;
        var results = unit.Attack(type);
        
        yield return dialog.TypeText($"Attacked the {unit.EnemyBase.DisplayName}");
        yield return results.Enemy.Blink();
        yield return results.Enemy.EnemyStats.HPStats.SetHealthSmooth((int) results.NewHealth); // todo casting might be bad here
        yield return dialog.TypeText($"{results.Enemy.EnemyBase.name} took {results.Damage} damage"); //todo show effectiveness
        // todo wait?

        if (unit.EnemyStats.HPStats.CurrentHealth == 0) {
            yield return dialog.TypeText($"{unit.EnemyBase.DisplayName} was defeated!");
            RemoveAttackableEnemy(unit);
        }

        if (enemies.Count == 0) {
            OnBattleEnded(true);
        }

        battleStage = BattleStage.EnemyAttack;
    }

    private void RemoveAttackableEnemy(EnemyUnit unit) {
        var index = enemies.IndexOf(unit);
        
        enemies.Remove(unit);
        defeatedEnemies.Add(unit);
        unit.gameObject.SetActive(false);

        var selectionOption = enemySelectionOptions.options[index];
        enemySelectionOptions.options.RemoveAt(index);
        selectionOption.gameObject.SetActive(false);
    }

    private IEnumerator AttackEnemy() {
        battleStage = BattleStage.Busy;
        
        var enemy = enemies[Random.Range(0, enemies.Count)];
        var results = playerUnit.Attack(enemy);
        
        yield return dialog.TypeText($"The {enemy.EnemyBase.DisplayName} attacks!");
        yield return playerUnit.Blink();
        yield return playerUnit.HpStats.SetHealthSmooth((int) results.NewHealth); // todo casting?
        yield return dialog.TypeText($"You took {results.Damage} damage"); //todo show effectiveness

        if (results.NewHealth == 0) {
            OnBattleEnded(false);
        }

        battleStage = BattleStage.SelectingAttack;
    }
}