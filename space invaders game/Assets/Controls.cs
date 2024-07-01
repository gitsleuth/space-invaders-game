using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] Spawn_Enemies spawnEnemies;
    [SerializeField] Spawn_Asteroids spawnAsteroids;
    [SerializeField] BulletController bulletController;

    public int speed = 10;
    public float fireRate = 0.5f;

    public GameObject gBullet;
    public Camera cam;
    public AudioSource explosionSFX;

    private List<GameObject> bullets = new List<GameObject>();
    private float elapsed = 0;
    private List<GameObject> clones;
    private Dictionary<GameObject, Vector3> startPositions;
    private Dictionary<GameObject, Vector3> endPositions;
    private List<GameObject> asteroids;
    private AudioSource laserSFX;

    // Start is called before the first frame update
    void Start()
    {
        clones = spawnEnemies.clones;

        startPositions = spawnEnemies.startPositions;
        endPositions = spawnEnemies.endPositions;

        asteroids = spawnAsteroids.asteroids;

        laserSFX = transform.GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + x * speed * dt, -8, 8), transform.position.y);

        //for (int i = bullets.Count - 1; i >= 0; i--)
        //{
        //    GameObject bullet = bullets[i];

        //    bullet.transform.position = bullet.transform.position + dt * speed * bullet.transform.up;

        //    Bounds bulletBounds = bullet.GetComponent<CircleCollider2D>().bounds;
        //    bool destroyed = false;

        //    for (int j = clones.Count - 1; j >= 0; j--)
        //    {
        //        GameObject clone = clones[j];

        //        if (bulletBounds.Intersects(clone.GetComponent<BoxCollider2D>().bounds))
        //        {
        //            Destroy(bullet);
        //            bullets.RemoveAt(i);

        //            Destroy(clone);
        //            clones.RemoveAt(j);

        //            if (startPositions.ContainsKey(clone))
        //            {
        //                startPositions.Remove(clone);
        //            }

        //            if (endPositions.ContainsKey(clone))
        //            {
        //                endPositions.Remove(clone);
        //            }

        //            clone.GetComponent<SpriteRenderer>().enabled = false;

        //            destroyed = true;

        //            explosionSFX.Play();

        //            break;
        //        }
        //    }

        //    if (destroyed)
        //    {
        //        break;
        //    }

        //    for (int l = asteroids.Count - 1; l >= 0; l--)
        //    {
        //        GameObject asteroid = asteroids[l];
        //        Bounds asteroidBounds = asteroid.GetComponent<CircleCollider2D>().bounds;
        //        if (bulletBounds.Intersects(asteroidBounds))
        //        {
        //            Destroy(bullet);
        //            bullets.RemoveAt(i);

        //            spawnAsteroids.OnAsteroidHit(asteroid, l);

        //            destroyed = true;

        //            break;
        //        }
        //    }

        //    if (destroyed)
        //    {
        //        break;
        //    }

        //    Vector3 screenPos = cam.WorldToScreenPoint(bullet.transform.position + Vector3.down * bullet.transform.localScale.y);
        //    float y = screenPos.y;

        //    if (y > Screen.height)
        //    {
        //        bullets.RemoveAt(i);
        //        Destroy(bullet);
        //    }
        //}

        elapsed += dt;

        while (elapsed >= 1 / fireRate)
        {
            elapsed -= 1 / fireRate;

            if (Input.GetButton("Fire1"))
            {
                bulletController.SpawnBullet(new Vector3(transform.position.x, gBullet.transform.position.y), Vector3.up);

                laserSFX.Play();
            }
        }
    }
}
