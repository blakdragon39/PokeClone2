using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Create new enemy")]
public class EnemyBase : ScriptableObject {
    [SerializeField] private Sprite sprite;
    [SerializeField] private string name;

    [SerializeField] private int maxHealth;
    [SerializeField] private int attack;
    [SerializeField] private int defence;
    [SerializeField] private int speed;
    [SerializeField] private EnemyType type;

    public Sprite Sprite => sprite;

    public int MaxHealth => maxHealth;

    public int Attack => attack;

    public int Defence => defence;

    public int Speed => speed;

    public EnemyType Type => type;
}

public enum EnemyType {
    Undead,
    Magic,
    Monster
}