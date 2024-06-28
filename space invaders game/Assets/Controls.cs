using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] Spawn_Enemies spawnEnemies;

    public int speed = 10;
    public float fireRate = 0.5f;

    public GameObject bullet;

    private List<GameObject> bullets = new List<GameObject>();
    private float elapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + x * speed * dt, -8, 8), transform.position.y);

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            GameObject bullet = bullets[i];

            bullet.transform.position = bullet.transform.position + bullet.transform.up * speed * dt;

            Bounds bulletBounds = bullet.GetComponent<CircleCollider2D>().bounds;

            for (int j = spawnEnemies.clones.Count - 1; j >= 0; j--)
            {
                GameObject clone = spawnEnemies.clones[j];

                if (bulletBounds.Intersects(clone.GetComponent<BoxCollider2D>().bounds))
                {
                    Destroy(bullet);
                    bullets.RemoveAt(i);

                    Destroy(clone);
                    spawnEnemies.clones.RemoveAt(j);

                    break;
                }
            }
        }

        elapsed += dt;

        while (elapsed >= 1 / fireRate)
        {
            elapsed -= 1 / fireRate;

            if (Input.GetButton("Fire1"))
            {
                GameObject clone;
                clone = Instantiate(bullet, new Vector3(transform.position.x, bullet.transform.position.y), bullet.transform.rotation);
                clone.GetComponent<SpriteRenderer>().enabled = true;
                bullets.Add(clone);
            };
        }
    }
}
