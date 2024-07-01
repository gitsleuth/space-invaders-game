using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] Spawn_Enemies spawnEnemies;
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

            (bool, GameObject) data = CheckCollisions(bullet, i, bounds, 0, spawnEnemies.clones);
            if (data.Item1)
            {
                Dictionary<GameObject, Vector3> startPositions = spawnEnemies.startPositions;
                Dictionary<GameObject, Vector3> endPositions = spawnEnemies.endPositions;

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

                break;
            }
            (bool, GameObject) data2 = (CheckCollisions(bullet, i, bounds, 1, spawnAsteroids.asteroids));
            if (data2.Item1)
            {
                GameObject asteroid = data2.Item2;

                spawnAsteroids.health[asteroid] -= 1;

                TMPro.TextMeshProUGUI textMesh = spawnAsteroids.textMeshes[asteroid];
                textMesh.text = spawnAsteroids.health[asteroid].ToString();
                if (spawnAsteroids.health[asteroid] == 0)
                {
                    Destroy(asteroid);
                    spawnAsteroids.asteroids.RemoveAt(i);

                    Destroy(textMesh.gameObject);
                    spawnAsteroids.textMeshes.Remove(asteroid);

                    spawnAsteroids.health.Remove(asteroid);

                    explosion2SFX.Play();
                }

                asteroidHitSFX.Play();

                break;
            }
            if (CheckCollision(bullet, i, bounds, controls.gameObject))
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

    private (bool, GameObject) CheckCollisions(GameObject bullet, int i, Bounds bounds, int boundsType, List<GameObject> objects)
    {
        bool collided = false;
        GameObject collidedWith = default;

        for (int j = objects.Count - 1; j >= 0; j--)
        {
            if (bounds.Intersects(objects[j].GetComponent<boundsType>().bounds))
            {
                Destroy(bullet);
                bullets.RemoveAt(i);

                GameObject obj = objects[j];

                Destroy(obj);
                objects.RemoveAt(j);

                collided = true;
                collidedWith = obj;

                break;
            }
        }

        return (collided, collidedWith);
    }

    private bool CheckCollision(GameObject bullet, int i, Bounds bounds, GameObject obj)
    {
        if (bounds.Intersects(obj.GetComponent<CircleCollider2D>().bounds))
        {
            Destroy(bullet);
            bullets.RemoveAt(i);

            Destroy(obj);

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
