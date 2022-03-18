using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private EnemyBase _base;
    [SerializeField] private Image image;
    [SerializeField] private EnemyStats enemyStats;

    public void Setup()
    {
        Debug.Log("Setting up EnemyUnit");
        var enemy = new Enemy(_base);
        
        image.sprite = _base.Sprite;
        enemyStats.Setup(enemy);
    }
}
