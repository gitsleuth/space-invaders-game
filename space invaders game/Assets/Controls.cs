using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public int speed = 10;
    public float fireRate = 0.5f;

    public GameObject bullet;

    public List<GameObject> bullets = new List<GameObject>();

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

        foreach (GameObject bullet in bullets)
        {
            bullet.transform.position = bullet.transform.position + bullet.transform.up * speed * dt;
        };

        elapsed += dt;

        while (elapsed >= 1 / fireRate)
        {
            elapsed -= 1 / fireRate;

            if (Input.GetButton("Fire1"))
            {
                GameObject clone;
                clone = Instantiate(bullet, new Vector3(transform.position.x, bullet.transform.position.y), bullet.transform.rotation);
                clone.transform.localScale = new Vector3(1, 1, 1);
                bullets.Add(clone);
            };
        }
    }
}
