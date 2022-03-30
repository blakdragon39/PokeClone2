using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType { Magic, Smack, Throw }

public class EnemyUnit : MonoBehaviour {

    public EnemyBase EnemyBase => _base;
    public EnemyStats EnemyStats => enemyStats;
    
    [SerializeField] private GameObject arrow;

    private EnemyBase _base;
    private Image image;
    private EnemyStats enemyStats;

    private void Awake() {
        image = GetComponentInChildren<Image>();
        enemyStats = GetComponentInChildren<EnemyStats>();
    }

    public void Setup(Enemy enemy) {
        _base = enemy.Base;
        image.sprite = enemy.Base.Sprite;
        enemyStats.Setup(enemy);
    }

    public void SetArrowVisible(bool visible) {
        arrow.SetActive(visible);
    }

    public EnemyAttackResults Attack(AttackType type) {
        var damage = 2f; // todo calculate damage
        var effectiveness = EnemyBase.GetEffectiveness(type, _base.Type);
        damage *= EnemyBase.GetEffectivenessMultiplier(effectiveness);

        var newHealth = enemyStats.HPStats.CurrentHealth - damage;
        if (newHealth < 0) newHealth = 0;

        return new EnemyAttackResults(this, damage, newHealth, effectiveness);
    }

    public IEnumerator Blink() {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(true);
    }
}