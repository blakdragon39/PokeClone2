using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private EnemyUnit enemyUnitBackLeft;
    [SerializeField] private EnemyUnit enemyUnitBackRight;
    [SerializeField] private EnemyUnit enemyUnitFront;
    
    private void Start()
    {
        SetupBattle();
    }

    private void SetupBattle()
    {
        enemyUnitBackLeft.Setup();
        enemyUnitBackRight.Setup();
        enemyUnitFront.Setup();
    }
}
