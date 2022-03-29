using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour {
    
    public HPStats HPStats => hpStats;
    
    [SerializeField] private Text nameText;
    [SerializeField] private Text typeText;
    [SerializeField] private HPStats hpStats;

    public void Setup(Enemy enemy) {
        nameText.text = enemy.Base.name;
        typeText.text = enemy.Base.Type.ToString();
        hpStats.Setup(enemy.HP);
    }
}