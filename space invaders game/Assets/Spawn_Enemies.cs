using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawn;
    public int enemies = 4;

    private float padding = 2;

    private int enemiesBefore;

    private bool spawned = false;

    private List<GameObject> clones = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        enemiesBefore = enemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies != enemiesBefore)
        {
            enemiesBefore = enemies;
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

            for (int i = 1; i < enemies + 1; i++)
            {
                double a = (i - (enemies + 1) * 0.5) * padding;
                GameObject clone;
                clone = Instantiate(enemy, new Vector3(spawn.transform.position.x + ((float)a), spawn.transform.position.y), enemy.transform.rotation);
                clone.transform.localScale = new Vector3(1, 1, 1);
                clones.Add(clone);
            }
        }
    }
}
