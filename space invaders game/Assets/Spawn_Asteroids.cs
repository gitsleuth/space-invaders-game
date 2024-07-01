using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Asteroids : MonoBehaviour
{
    public GameObject asteroid;
    public Camera cam;
    public TMPro.TextMeshProUGUI textMesh;
    public GameObject parent;

    public int numAsteroids = 3;
    public int defHealth = 20;
    public List<GameObject> asteroids = new List<GameObject>();

    private Transform asteroidTrans;
    private float asteroidY;
    private Quaternion asteroidRot;
    private float screenWidth = Screen.width;
    private int beforeNumAsteroids;
    private float asteroidWidth;
    private Dictionary<GameObject, int> health = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, TMPro.TextMeshProUGUI> textMeshes = new Dictionary<GameObject, TMPro.TextMeshProUGUI>();
    private Transform canvasT;

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
            health.Add(clonedAsteroid, defHealth);

            TMPro.TextMeshProUGUI cTextMesh = Instantiate(textMesh);
            cTextMesh.GetComponent<TMPro.TMP_Text>().enabled = true;
            cTextMesh.transform.SetParent(parent.transform);
            float sY = cam.WorldToScreenPoint(new Vector3(0, asteroidY)).y;
            cTextMesh.transform.position = Vector3.up * (Screen.height - (Screen.height - sY)) + Vector3.right * (screenWidth / 2 + x);
            textMeshes.Add(clonedAsteroid, cTextMesh);
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

    public void OnAsteroidHit(GameObject asteroid, int i)
    {
        health[asteroid] -= 1;
        TMPro.TextMeshProUGUI textMesh = textMeshes[asteroid];
        textMesh.text = health[asteroid].ToString();
        if (health[asteroid] == 0)
        {
            Destroy(asteroid);
            asteroids.RemoveAt(i);

            Destroy(textMesh.gameObject);
            textMeshes.Remove(asteroid);

            health.Remove(asteroid);
        }
    }
}
