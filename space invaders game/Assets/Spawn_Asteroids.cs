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
    private float screenWidth = Screen.width;
    private int beforeNumAsteroids;
    private List<GameObject> asteroids = new List<GameObject>();
    private float asteroidWidth;

    // Start is called before the first frame update
    void Start()
    {
        asteroidTrans = asteroid.transform;
        asteroidY = asteroidTrans.position.y;
        asteroidRot = asteroidTrans.rotation;
        asteroidWidth = asteroidTrans.localScale.x;
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
                DrawAsteroids();
            }
        }
    }

    void DrawAsteroids()
    {
        float a = (screenWidth - numAsteroids * asteroidWidth) / (numAsteroids + 1);

        for (int i = 1; i < numAsteroids + 1; i++)
        {
            float x = (i - (numAsteroids + 1) * 0.5f) * a;
            float worldX = cam.ScreenToWorldPoint(new Vector3(screenWidth / 2 + x, 0)).x;
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
