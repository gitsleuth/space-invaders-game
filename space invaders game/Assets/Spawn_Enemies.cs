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

            moving = true;
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

        bool moved = false;
        Vector3[] lastPositions = new Vector3[numClones];

        while (!moved)
        {
            if (lerpElapsed >= moveDuration)
            {
                moved = true;
            } else
            {
                for (int i = 0; i < numClones; i++)
                {
                    GameObject clone = clones[i];
                    Vector3 startPos = startPositions[i];
                    clone.transform.position = Vector3.Lerp(startPos, startPos + direction * moveDistance, lerpElapsed / moveDuration);
                    lastPositions[i] = clone.transform.position;
                };
            }

            yield return null;
        }

        bool outOfBounds = false;

        foreach (Vector3 lastPos in lastPositions)
        {
            if (IsOutOfBounds(lastPos))
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
