using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemies : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawn;

    public int enemies = 4;
    public List<GameObject> clones = new List<GameObject>();

    private int padding = 2;
    private int enemiesBefore;
    private bool spawned = false;

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
                float offset = (i - (enemies + 1) * 0.5f) * padding;
                GameObject clone;
                clone = Instantiate(enemy, new Vector3(spawn.transform.position.x + offset, spawn.transform.position.y), enemy.transform.rotation);
                clone.GetComponent<SpriteRenderer>().enabled = true;
                clones.Add(clone);
            }
        }
    }
}
