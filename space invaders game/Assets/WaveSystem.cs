using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;

    public int curWave = 0;

    // Start is called before the first frame update
    void Start()
    {
        NewWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if all enemies are dead
        int deadEnemyTally = 0;

        foreach (GameObject clone in enemyController.clones)
        {
            if (clone == null)
            {
                deadEnemyTally += 1;
            }
        }

        if (deadEnemyTally == enemyController.enemies)
        {
            //NewWave();
        }
    }

    void NewWave()
    {
        enemyController.SpawnEnemies();
        curWave += 1;
    }
}
