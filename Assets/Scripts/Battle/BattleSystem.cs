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
    [SerializeField] private MenuOptions preBattleOptions;
    [SerializeField] private MenuOptions attackSelectionOptions;
    [SerializeField] private MenuOptions enemySelectionOptions;

    private BattleStage battleStage;

    public void StartBattle(List<Enemy> newEnemies) {
        battleStage = BattleStage.PreBattle;
        preBattleOptions.SetSelectedOption(0);
        attackSelectionOptions.SetSelectedOption(0);
        enemySelectionOptions.SetSelectedOption(0);

        SetupBattle(newEnemies);
    }

    public void HandleUpdate() {
        switch (battleStage) {
            case BattleStage.PreBattle:
                HandlePreBattleAdvance();
                break;
            case BattleStage.SelectingAttack:
                HandleAttackSelection();
                break;
            case BattleStage.SelectingEnemy:
                HandleEnemySelection();
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
            enemies[i].SetArrowVisible(battleStage == BattleStage.SelectingEnemy && i == enemySelectionOptions.SelectedOption);
        }
    }

    private void SetupBattle(List<Enemy> newEnemies) {
        for (var i = 0; i < enemies.Count; i++) {
            if (i < newEnemies.Count) {
                var newEnemy = newEnemies[i];
                newEnemy.Init();
                enemies[i].Setup(newEnemy);
                enemies[i].gameObject.SetActive(true);
            } else {
                //todo this adds them to list of defeated enemies as well, when it probably shouldn't:
                RemoveAttackableEnemy(enemies[i]);
            }
        }
    }
    
    private void HandlePreBattleAdvance() {
        if (!Input.GetKeyDown(KeyCode.Return)) return;

        switch (preBattleOptions.SelectedOption) {
            case 0: //begin battle
                battleStage = BattleStage.SelectingAttack;
                break;
            case 1: //run away
                OnBattleEnded(true);
                break;
        }
    }

    private void HandleAttackSelection() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            battleStage = BattleStage.SelectingEnemy;
        } else if (Input.GetKeyDown(KeyCode.Backspace)) {
            battleStage = BattleStage.PreBattle;
        }
    }

    private void HandleEnemySelection() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            battleStage = BattleStage.SelectingAttack;
            return;
        }
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        
        var enemy = enemies[enemySelectionOptions.SelectedOption];
        var type = AttackType.Magic;

        var selectedAttackOption = attackSelectionOptions.SelectedOption;
        if (selectedAttackOption == 0) {
            type = AttackType.Magic;
        } else if (selectedAttackOption == 1) {
            type = AttackType.Throw;
        } else if (selectedAttackOption == 2) {
            type = AttackType.Smack;
        }

        StartCoroutine(AttackEnemy(enemy, type));
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
            enemySelectionOptions.SetSelectedOption(0);
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