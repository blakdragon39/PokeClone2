using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPStats : MonoBehaviour {

    public int CurrentHealth => currentHealth;
    
    [SerializeField] private Text hpText;
    [SerializeField] private GameObject currentHealthBar;

    private int maxHealth;
    private int currentHealth;
    
    public void Setup(int maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        SetHealth(maxHealth);
    }
    
    public void SetHealth(int newHealth) {
        currentHealth = newHealth;
        hpText.text = $"{currentHealth}/{maxHealth}";

        var normalizedHealth = (float) currentHealth / maxHealth;
        currentHealthBar.transform.localScale = new Vector3(normalizedHealth, 1f);
    }

    public IEnumerator SetHealthSmooth(int newHealth) {
        currentHealth = newHealth;
        
        var normalizedHealth = (float) currentHealth / maxHealth;
        var currentScale = currentHealthBar.transform.localScale.x;
        var change = currentScale - normalizedHealth;

        while (currentScale - normalizedHealth > Mathf.Epsilon) {
            currentScale -= change * Time.deltaTime;
            currentHealthBar.transform.localScale = new Vector3(currentScale, 1f);
            yield return null;
        }
        
        SetHealth(newHealth);
    }
}