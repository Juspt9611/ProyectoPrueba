using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public static float score;

    public Camera fpsCam;
    public ParticleSystem Disparo;
    public GameObject impactEffect;

    private AndroidJavaClass plugin;

    AudioSource GunAudio;

    private float nextTimeToFire = 0f;


    private void Start()
    {
        plugin = new AndroidJavaClass("com.ite.argunlibrary.BluetoothService");


        if (plugin.CallStatic<string>("getARGUN").Trim().Equals("0"))
        {
            GunAudio.Play();
        }

        score = 0;

    }

    private void Update()
    {
        if (plugin.CallStatic<string>("getARGUN").Trim().Equals("1") && Time.time >= nextTimeToFire)//if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }


    
    private void Awake()
    {
        GunAudio = GetComponent<AudioSource>();
    }
    void Shoot()
    {
        GunAudio.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            Target2 target2 = hit.transform.GetComponent<Target2>();
            TargetSA target3 = hit.transform.GetComponent<TargetSA>();

            if (target != null)
            {
                target.TakeDamage(damage);
                score += 1f; 

            }
            if (target2 != null)
            {
                target2.TakeDamage(damage);
                score -= 2f; 

            }

            if (target3 != null)
            {
                target3.TakeDamage(damage);
                score += 10f;


            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}

