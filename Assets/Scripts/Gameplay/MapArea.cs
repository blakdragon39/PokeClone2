using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour {

    [SerializeField] private List<Enemy> enemies;

    public List<Enemy> GetRandomEnemyParty() {
        var numEnemies = Random.Range(2, 4);
        var enemyParty = new List<Enemy>();

        for (int i = 0; i < numEnemies; i++) {
            enemyParty.Add(enemies[Random.Range(0, enemies.Count)]);
        }

        return enemyParty;
    }
}
