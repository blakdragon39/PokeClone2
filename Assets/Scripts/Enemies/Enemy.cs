using UnityEngine;

public class Enemy
{
    private EnemyBase Base { get; set; }
    
    private int HP { get; set; }
    
    public Enemy(EnemyBase eBase)
    {
        Base = eBase;
        HP = eBase.MaxHealth;
    }
}
