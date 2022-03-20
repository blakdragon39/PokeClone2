using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour {
    [SerializeField] private Text nameText;
    [SerializeField] private HPStats hpStats;

    public void Setup(Enemy enemy) {
        nameText.text = enemy.Base.name;
        hpStats.SetHealth(enemy.HP, enemy.Base.MaxHealth);
    }
}