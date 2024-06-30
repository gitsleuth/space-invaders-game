using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Asteroids : MonoBehaviour
{
    public GameObject asteroid;
    public Camera cam;

    public int numAsteroids = 3;

    private Transform asteroidTrans;
    private float asteroidY;
    private Quaternion asteroidRot;
    private float sectionWidth;
    private int beforeNumAsteroids;
    private List<GameObject> asteroids = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        asteroidTrans = asteroid.transform;
        asteroidY = asteroidTrans.position.y;
        asteroidRot = asteroidTrans.rotation;
        sectionWidth = Screen.width / (numAsteroids + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeNumAsteroids != numAsteroids)
        {
            if (beforeNumAsteroids != default)
                DestroyAsteroids();

            beforeNumAsteroids = numAsteroids;

            if (numAsteroids > 0)
            {
                sectionWidth = Screen.width / (numAsteroids + 1);

                DrawAsteroids();
            }
        }
    }

    void DrawAsteroids()
    {
        for (int i = 1; i < numAsteroids + 1; i++)
        {
            float x = i * sectionWidth;
            float worldX = cam.ScreenToWorldPoint(new Vector3(x, 0)).x;
            GameObject clonedAsteroid = Instantiate(asteroid, new Vector3(worldX, asteroidY), asteroidRot);
            clonedAsteroid.GetComponent<SpriteRenderer>().enabled = true;
            asteroids.Add(clonedAsteroid);
        }
    }

    void DestroyAsteroids()
    {
        for (int i = asteroids.Count - 1; i >= 0; i--)
        {
            Destroy(asteroids[i]);
            asteroids.RemoveAt(i);
        }
    }
}
