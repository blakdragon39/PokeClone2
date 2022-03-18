using UnityEngine;
using UnityEngine.UI;

public class HPStats : MonoBehaviour
{
    [SerializeField] private Text hpText;
    [SerializeField] private GameObject currentHealthBar;

    public void SetHealth(int currentHealth, int maxHealth)
    {
        hpText.text = $"{currentHealth}/{maxHealth}";

        var normalizedHealth = (float) maxHealth / currentHealth;
        currentHealthBar.transform.localScale = new Vector3(normalizedHealth, 1f);
    }
}
