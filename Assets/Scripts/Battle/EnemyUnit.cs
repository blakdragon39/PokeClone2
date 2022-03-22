using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType { Magic, Smack, Throw }

public class EnemyUnit : MonoBehaviour {

    [SerializeField] private EnemyBase _base;
    [SerializeField] private Image image;
    [SerializeField] private GameObject arrow;
    [SerializeField] private EnemyStats enemyStats;

    public void Setup() {
        var enemy = new Enemy(_base);

        image.sprite = _base.Sprite;
        enemyStats.Setup(enemy);
    }

    public void SetArrowVisible(bool visible) {
        arrow.SetActive(visible);
    }

    public IEnumerator Attack(AttackType type) {
        //todo calculate damage
        var newHealth = enemyStats.HPStats.CurrentHealth - 2;
        return enemyStats.HPStats.SetHealthSmooth(newHealth);
    }
}