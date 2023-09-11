using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    public GunData data;
    private Recoil recoilScript;
    private AudioSource audioSrc;
    public AudioClip hitClip;
    public Camera playerCam;
    public GameObject bulletHole;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    
    private float _timeSinceLastShot = 0;
    public GameObject bulletPrefab;
    
    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        recoilScript = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();
        audioSrc = GetComponent<AudioSource>();
    }

    private bool CanShoot() => !data.GetReloadingStatus() && _timeSinceLastShot > 1f / (data.fireRate / 60f);
    
    public void Shoot()
    {
        if (data.currentAmmo > 0)
        {
            if (CanShoot())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                audioSrc.PlayOneShot(hitClip);
                recoilScript.RecoilFire();
                
                muzzleFlash.Play();
                
                //var bullet = Instantiate(bulletPrefab, muzzle.position, GameObject.Find("CameraHolder/PlayerCam/CameraRot/CameraRecoil/WeaponHolder").transform.rotation);
                
                Vector3 origin = playerCam.transform.position;
                
                Vector3 screenCenter = playerCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
                
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
                {
                    // bullet.GetComponent<Rigidbody>().transform.LookAt(screenCenter);
                    // bullet.GetComponent<Rigidbody>().AddForce(screenCenter * data.bulletSpeed, ForceMode.Impulse);
                    var bulletHole = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForce(-hit.normal * data.impactForce);
                    }
                    
                }
                
                // Vector3 screenCenter = playerCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
                // bullet.GetComponent<Rigidbody>().transform.LookAt(screenCenter);
                // bullet.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * data.bulletSpeed, ForceMode.Impulse);
                
                // easter eggas 
                //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * data.bulletSpeed, ForceMode.Impulse);
                
                data.currentAmmo--;
                _timeSinceLastShot = 0;
            }
        }
        }
    

    private void Update()
    {
        if (data.DrawRaycastInScene)
        {
            Debug.DrawRay(muzzle.position, muzzle.forward * 100, Color.green);
        }
        _timeSinceLastShot += Time.deltaTime;
    }
}
