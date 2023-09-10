using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    public GunData data;
    private float _timeSinceLastShot = 0;
    
    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
    }

    private bool CanShoot() => !data.GetReloadingStatus() && _timeSinceLastShot > 1f / (data.fireRate / 60f);
    
    public void Shoot()
    {
        if (data.currentAmmo > 0)
        {
            if (CanShoot())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log("shot object" + hit.transform.name);
                }

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
