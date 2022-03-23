using System.Collections;
using UnityEngine;

public class PlayerUnit : MonoBehaviour {

    [SerializeField] private int maxHealth;
    [SerializeField] private HPStats hpStats;
    [SerializeField] private GameObject image;
    
    private void Start()
    {
        hpStats.Setup(maxHealth);
    }

    public IEnumerator Attack(EnemyUnit attacker) {
        //todo calculate damage
        var newHealth = hpStats.CurrentHealth - 2;
        if (newHealth < 0) newHealth = 0;

        yield return Blink();
        yield return hpStats.SetHealthSmooth(newHealth);
    }
    
    private IEnumerator Blink() {
        image.SetActive(false);
        yield return new WaitForSeconds(.1f);
        image.SetActive(true);
        yield return new WaitForSeconds(.1f);
        image.SetActive(false);
        yield return new WaitForSeconds(.1f);
        image.SetActive(true);
    }
}
