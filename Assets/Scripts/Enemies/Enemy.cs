using UnityEngine;

public class Enemy {
    public EnemyBase Base { get; set; }

    public int HP { get; set; }

    public Enemy(EnemyBase eBase) {
        Base = eBase;
        HP = eBase.MaxHealth;
    }
}