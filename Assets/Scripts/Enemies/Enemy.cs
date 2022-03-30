using UnityEngine;

[System.Serializable]
public class Enemy {
    public EnemyBase Base => _base;

    public int HP { get; set; }

    [SerializeField] private EnemyBase _base;
    
    public void Init() {
        HP = _base.MaxHealth;
    }
}