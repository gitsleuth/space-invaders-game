using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 4;
    public GameObject bullet;

    private List<GameObject> bullets = new List<GameObject>();
    private Dictionary<GameObject, Vector3> directions = new Dictionary<GameObject, Vector3>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            GameObject bullet = bullets[i];

            bullet.transform.position = bullet.transform.position + Time.deltaTime * speed * directions[bullet];
        }
    }

    public void SpawnBullet(Vector3 origin, Vector3 direction)
    {
        GameObject clone;
        clone = Instantiate(bullet, origin, bullet.transform.rotation);
        clone.GetComponent<SpriteRenderer>().enabled = true;

        bullets.Add(clone);
        directions.Add(clone, direction);
    }
}
