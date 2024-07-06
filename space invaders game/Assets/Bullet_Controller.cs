using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    [SerializeField] Spawn_Asteroids spawnAsteroids;
    [SerializeField] Controls controls;

    public float speed = 4;
    public GameObject bullet;
    public AudioSource explosionSFX;
    public AudioSource explosion2SFX;
    public AudioSource asteroidHitSFX;
    public Camera cam;

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

            Bounds bounds = bullet.GetComponent<CircleCollider2D>().bounds;

            (bool, GameObject, int) data = CheckCollisions(bullet, i, bounds, typeof(BoxCollider2D), enemyController.clones, true);
            if (data.Item1)
            {
                Dictionary<GameObject, Vector3> startPositions = enemyController.startPositions;
                Dictionary<GameObject, Vector3> endPositions = enemyController.endPositions;

                GameObject clone = data.Item2;

                if (startPositions.ContainsKey(clone))
                {
                    startPositions.Remove(clone);
                }

                if (endPositions.ContainsKey(clone))
                {
                    endPositions.Remove(clone);
                }

                clone.GetComponent<SpriteRenderer>().enabled = false;

                explosionSFX.Play();

                enemyController.OnEnemyDestroyed(data.Item3);

                break;
            }
            (bool, GameObject, int) data2 = (CheckCollisions(bullet, i, bounds, typeof(CircleCollider2D), spawnAsteroids.asteroids, false));
            if (data2.Item1)
            {
                GameObject asteroid = data2.Item2;
                int j = data2.Item3;

                spawnAsteroids.health[asteroid] -= 1;

                TMPro.TextMeshProUGUI textMesh = spawnAsteroids.textMeshes[asteroid];
                textMesh.text = spawnAsteroids.health[asteroid].ToString();
                if (spawnAsteroids.health[asteroid] == 0)
                {
                    Destroy(asteroid);
                    spawnAsteroids.asteroids.RemoveAt(j);

                    Destroy(textMesh.gameObject);
                    spawnAsteroids.textMeshes.Remove(asteroid);

                    spawnAsteroids.health.Remove(asteroid);

                    explosion2SFX.Play();
                }

                asteroidHitSFX.Play();

                break;
            }
            if (CheckCollision(bullet, i, bounds, typeof(BoxCollider2D), controls.gameObject, false))
            {
                break;
            }
            CheckOutOfBounds(bullet, i);
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

    private (bool, GameObject, int) CheckCollisions(GameObject bullet, int i, Bounds bounds, System.Type boundsType, List<GameObject> objects, bool destroyObject)
    {
        bool collided = false;
        GameObject collidedWith = default;
        int rJ = 0;

        for (int j = objects.Count - 1; j >= 0; j--)
        {
            if (bounds.Intersects((objects[j].GetComponent(boundsType) as Collider2D).bounds))
            {
                Destroy(bullet);
                bullets.RemoveAt(i);

                GameObject obj = objects[j];

                if (destroyObject)
                {
                    Destroy(obj);
                    objects.RemoveAt(j);
                }

                collided = true;
                collidedWith = obj;
                rJ = j;

                break;
            }
        }

        return (collided, collidedWith, rJ);
    }

    private (bool, GameObject, int) CheckCollisions(GameObject bullet, int i, Bounds bounds, System.Type boundsType, GameObject[] objects, bool destroyObject)
    {
        bool collided = false;
        GameObject collidedWith = default;
        int rJ = 0;

        for (int j = objects.Length - 1; j >= 0; j--)
        {
            GameObject obj = objects[j];

            if (obj == null)
            {
                continue;
            }

            if (bounds.Intersects((obj.GetComponent(boundsType) as Collider2D).bounds))
            {
                Destroy(bullet);
                bullets.RemoveAt(i);

                if (destroyObject)
                {
                    Destroy(obj);
                    objects[j] = null;
                }

                collided = true;
                collidedWith = obj;
                rJ = j;

                break;
            }
        }

        return (collided, collidedWith, rJ);
    }

    private bool CheckCollision(GameObject bullet, int i, Bounds bounds, System.Type boundsType, GameObject obj, bool destroyObject)
    {
        if (bounds.Intersects((obj.GetComponent(boundsType) as Collider2D).bounds))
        {
            Destroy(bullet);
            bullets.RemoveAt(i);

            if (destroyObject)
            {
                Destroy(obj);
            }

            return true;
        }

        return false;
    }

    private void CheckOutOfBounds(GameObject bullet, int i)
    {
        Vector3 direction = directions[bullet];
        Vector2 point = default;
        if (direction == Vector3.up)
        {
            point = bullet.transform.position + Vector3.down * bullet.transform.localScale.y;
        } else
        {
            point = bullet.transform.position + Vector3.up * bullet.transform.localScale.y;
        }
        Vector3 screenPos = cam.WorldToScreenPoint(point);
        float y = screenPos.y;

        if (direction == Vector3.up && y > Screen.height || direction == Vector3.down && y < 0)
        {
            Destroy(bullet);
            bullets.RemoveAt(i);
        }
    }
}
