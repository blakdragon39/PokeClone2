using System.Collections;
using UnityEngine;

public class PlayerUnit : MonoBehaviour {

    public HPStats HpStats => hpStats;
    
    [SerializeField] private int maxHealth;
    [SerializeField] private HPStats hpStats;
    [SerializeField] private GameObject image;

    private void Start() {
        hpStats.Setup(maxHealth);
    }

    public PlayerAttackResults Attack(EnemyUnit attacker) {
        //todo calculate damage
        var damage = 2;
        var newHealth = hpStats.CurrentHealth - damage;
        if (newHealth < 0) newHealth = 0;

        
        return new PlayerAttackResults(damage, newHealth, attacker);
    }

    public IEnumerator Blink() {
        image.SetActive(false);
        yield return new WaitForSeconds(.1f);
        image.SetActive(true);
        yield return new WaitForSeconds(.1f);
        image.SetActive(false);
        yield return new WaitForSeconds(.1f);
        image.SetActive(true);
    }
}

public class PlayerAttackResults {

    public float Damage { get; }
    public float NewHealth { get; }
    public EnemyUnit Attacker { get; }
    
    public PlayerAttackResults(float damage, float newHealth, EnemyUnit attacker) {
        Damage = damage;
        NewHealth = newHealth;
        Attacker = attacker;
    }
}