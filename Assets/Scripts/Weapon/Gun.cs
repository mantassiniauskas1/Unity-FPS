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
                
                //var bullet = Instantiate(bulletPrefab, muzzle.position, bulletPrefab.transform.rotation);
                
                Vector3 origin = playerCam.transform.position;
                
                Vector3 screenCenter = playerCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
                
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
                {
                    // bullet.GetComponent<Rigidbody>().transform.LookAt(screenCenter);
                    // bullet.GetComponent<Rigidbody>().AddForce(screenCenter * data.bulletSpeed, ForceMode.Impulse);
                    Instantiate(bulletHole, hit.point + (hit.normal * 0.1f),
                        Quaternion.FromToRotation(Vector3.up, hit.normal));
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
