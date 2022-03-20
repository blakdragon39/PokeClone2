using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour {
    [SerializeField] private EnemyBase _base;
    [SerializeField] private Image image;
    [SerializeField] private EnemyStats enemyStats;

    public void Setup() {
        var enemy = new Enemy(_base);

        image.sprite = _base.Sprite;
        enemyStats.Setup(enemy);
    }
}