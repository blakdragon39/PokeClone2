using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Create new enemy")]
public class EnemyBase : ScriptableObject {
    
    [SerializeField] private Sprite sprite;
    [SerializeField] private string displayName;

    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defence;
    [SerializeField] private int speed;
    [SerializeField] private EnemyType type;

    public Sprite Sprite => sprite;

    public string DisplayName => displayName;
    
    public int MaxHealth => maxHealth;

    public int Attack => attack;

    public int Defence => defence;

    public int Speed => speed;

    public EnemyType Type => type;
    
    public static AttackEffectiveness GetEffectiveness(AttackType attackType, EnemyType enemyType) {
        return chart[attackType][enemyType];
    }

    public static float GetEffectivenessMultiplier(AttackEffectiveness effectiveness) {
        switch (effectiveness) {
            case AttackEffectiveness.Twice:
                return 2f;
            case AttackEffectiveness.Half:
                return 0.5f;
            case AttackEffectiveness.Normal:
            default:
                return 1f;
        }
    }
    
    private static Dictionary<AttackType, Dictionary<EnemyType, AttackEffectiveness>> chart =
        new Dictionary<AttackType, Dictionary<EnemyType, AttackEffectiveness>> {
            {
                AttackType.Magic, new Dictionary<EnemyType, AttackEffectiveness> {
                    { EnemyType.Undead, AttackEffectiveness.Twice },
                    { EnemyType.Magic, AttackEffectiveness.Half },
                    { EnemyType.Monster, AttackEffectiveness.Normal }
                }
            },
            {
                AttackType.Smack, new  Dictionary<EnemyType, AttackEffectiveness> {
                    { EnemyType.Magic, AttackEffectiveness.Twice },
                    { EnemyType.Monster, AttackEffectiveness.Half },
                    { EnemyType.Undead, AttackEffectiveness.Normal }
                }
            },
            {
                AttackType.Throw, new Dictionary<EnemyType, AttackEffectiveness> {
                    { EnemyType.Monster, AttackEffectiveness.Twice },
                    { EnemyType.Undead, AttackEffectiveness.Half },
                    { EnemyType.Magic, AttackEffectiveness.Normal }
                }
            }
        };
}

public enum EnemyType {
    Undead,
    Magic,
    Monster
}

public enum AttackEffectiveness {
    Twice,
    Half,
    Normal
}

public class EnemyAttackResults {
    
    public EnemyUnit Enemy { get; }
    public float Damage { get;  }
    public float NewHealth { get; }
    public AttackEffectiveness Effectiveness { get; }
    
    public EnemyAttackResults(EnemyUnit enemy, float damage, float newHealth, AttackEffectiveness effectiveness) {
        Enemy = enemy;
        Damage = damage;
        NewHealth = newHealth;
        Effectiveness = effectiveness;
    }
}