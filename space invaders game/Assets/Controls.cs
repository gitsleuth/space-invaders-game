using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] BulletController bulletController;

    public int speed = 10;
    public float fireRate = 0.5f;

    public GameObject gBullet;
    public Camera cam;
    public AudioSource explosionSFX;

    private float elapsed = 0;
    private AudioSource laserSFX;

    // Start is called before the first frame update
    void Start()
    {
        laserSFX = transform.GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + x * speed * dt, -8, 8), transform.position.y);

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
