using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPStats : MonoBehaviour {

    public int CurrentHealth => _currentHealth;
    
    [SerializeField] private Text hpText;
    [SerializeField] private GameObject currentHealthBar;

    private int _maxHealth;
    private int _currentHealth;
    
    public void Setup(int maxHealth) {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        SetHealth(maxHealth);
    }
    
    public void SetHealth(int newHealth) {
        _currentHealth = newHealth;
        hpText.text = $"{_currentHealth}/{_maxHealth}";

        var normalizedHealth = (float) _currentHealth / _maxHealth;
        currentHealthBar.transform.localScale = new Vector3(normalizedHealth, 1f);
    }

    public IEnumerator SetHealthSmooth(int newHealth) {
        _currentHealth = newHealth;
        
        var normalizedHealth = (float) _currentHealth / _maxHealth;
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