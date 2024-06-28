using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawn;
    public Camera cam;

    public int enemiesPerRow = 6;
    public int rows = 3;
    public List<GameObject> clones = new List<GameObject>();
    public float moveInterval = 1.5f;
    public Dictionary<GameObject, Vector3> startPositions;
    public Dictionary<GameObject, Vector3> endPositions;

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
    private Vector3 direction = Vector3.right;
    private Vector3 nextDirection;

    // Start is called before the first frame update
    void Start()
    {
        enemies = enemiesPerRow * rows;

        enemiesPerRowBefore = enemiesPerRow;
        rowsBefore = rows;

        startPositions = new Dictionary<GameObject, Vector3>();
        endPositions = new Dictionary<GameObject, Vector3>();

        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        //if (enemiesPerRowBefore != enemiesPerRow || rowsBefore != rows)
        //{
        //    enemiesPerRowBefore = enemiesPerRow;
        //    rowsBefore = rows;
        //    enemies = enemiesPerRow * rows;
        //    spawned = false;

        //    foreach(GameObject clone in clones)
        //    {
        //        Destroy(clone);
        //        clones.Remove(clone);
        //    };
        //}

        //if (!spawned)
        //{
        //    spawned = true;

        //    SpawnEnemies();
        //}

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

            moving = true;
            StartCoroutine(MoveEnemies());
        };
    }

    void SpawnEnemies()
    {
        int row = 0;

        for (int i = 1; i < enemies + 1; i++)
        {
            float offset = ((i - row * enemiesPerRow) - (enemiesPerRow + 1) * 0.5f) * padding;
            GameObject clone;
            clone = Instantiate(enemy, new Vector3(spawn.transform.position.x + offset, spawn.transform.position.y - row * padding), enemy.transform.rotation);
            SpriteRenderer renderer = clone.GetComponent<SpriteRenderer>();
            renderer.enabled = true;
            if (i == enemies)
            {
                renderer.color = Color.red;
            }
            clones.Add(clone);
            if (i != enemies && i % enemiesPerRow == 0)
            {
                row += 1;
            }
        }
    }

    IEnumerator MoveEnemies()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            GameObject clone = clones[i];
            startPositions[clone] = clone.transform.position;
        };

        bool moved = false;

        while (!moved)
        {
            if (lerpElapsed >= moveDuration)
            {
                moved = true;
            } else
            {
                for (int i = 0; i < clones.Count; i++)
                {
                    GameObject clone = clones[i];
                    Vector3 startPos = startPositions[clone];
                    clone.transform.position = Vector3.Lerp(startPos, startPos + direction * moveDistance, lerpElapsed / moveDuration);
                    endPositions[clone] = clone.transform.position;
                };
            }

            yield return null;
        }

        bool outOfBounds = false;

        foreach (Vector3 endPos in endPositions.Values)
        {
            if (IsOutOfBounds(endPos))
            {
                outOfBounds = true;
            };
        };

        if (outOfBounds)
        {
            if (nextDirection == Vector3.zero)
            {
                nextDirection = direction == Vector3.right ? Vector3.left : Vector3.right;
                direction = Vector3.down;
            }
            else
            {
                direction = nextDirection;
                nextDirection = Vector3.zero;
            }
        };

        moving = false;
    }

    bool IsOutOfBounds(Vector3 position)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(position);
        float x = screenPos.x;

        return x > Screen.width || x < 0;
    }
}
