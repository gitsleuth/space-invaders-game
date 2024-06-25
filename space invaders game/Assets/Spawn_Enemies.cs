using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawn;

    public int enemiesPerRow = 6;
    public int rows = 3;
    public List<GameObject> clones = new List<GameObject>();
    public int moveInterval = 1;

    private int padding = 2;
    private bool spawned = false;
    private int enemies;
    private int enemiesPerRowBefore;
    private int rowsBefore;
    private float elapsed = 0;
    private float moveDistance = 1;
    private float lerpElapsed = 0;
    private float moveDuration = 0.5f;
    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        enemies = enemiesPerRow * rows;

        enemiesPerRowBefore = enemiesPerRow;
        rowsBefore = rows;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesPerRowBefore != enemiesPerRow || rowsBefore != rows)
        {
            enemiesPerRowBefore = enemiesPerRow;
            rowsBefore = rows;
            enemies = enemiesPerRow * rows;
            spawned = false;

            for (int i = clones.Count - 1; i >= 0; i--)
            {
                Destroy(clones[i]);
                clones.RemoveAt(i);
            };
        }

        if (!spawned)
        {
            spawned = true;

            int row = 0;

            for (int i = 1; i < enemies + 1; i++)
            {
                float offset = ((i - row * enemiesPerRow) - (enemiesPerRow + 1) * 0.5f) * padding;
                GameObject clone;
                clone = Instantiate(enemy, new Vector3(spawn.transform.position.x + offset, spawn.transform.position.y - row * padding), enemy.transform.rotation);
                clone.GetComponent<SpriteRenderer>().enabled = true;
                clones.Add(clone);
                if (i != enemies && i % enemiesPerRow == 0)
                {
                    row += 1;
                }
            }
        }

        elapsed += Time.deltaTime;

        if (moving)
        {
            lerpElapsed += Time.deltaTime;
        }
        else
        {
            lerpElapsed = 0;
        };

        while (elapsed >= moveInterval)
        {
            elapsed -= moveInterval;

            StartCoroutine(MoveEnemies());
        };
    }

    IEnumerator MoveEnemies()
    {
        int numClones = clones.Count;
        Vector3[] startPositions = new Vector3[numClones];

        for (int i = 0; i < numClones; i++)
        {
            startPositions[i] = clones[i].transform.position;
        };

        moving = true;

        while (lerpElapsed < moveDuration)
        {
            for (int i = 0; i < numClones; i++)
            {
                GameObject clone = clones[i];

                Transform transform = clone.gameObject.transform;

                Vector3 startPosition = startPositions[i];

                transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.right * moveDistance, lerpElapsed / moveDuration);
            };
        }

        moving = false;
    }
}
