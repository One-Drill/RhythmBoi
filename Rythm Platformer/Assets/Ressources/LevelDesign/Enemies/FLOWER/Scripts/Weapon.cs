using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private float tempsReel;
    public FollowerOfTheRhythm tempo;

    void Start()
    {
        tempo = GetComponent<FollowerOfTheRhythm>();
        tempsReel = 60f / tempo.getBpm();
    }

    void Update()
    {
        tempsReel -= Time.deltaTime;
        if (tempsReel <= 0 && Time.time >= 1)
        {
            tempsReel = 240f / tempo.getBpm();
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
