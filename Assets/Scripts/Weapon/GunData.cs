using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GunData : MonoBehaviour
{
    [Header("Info")]
    public new string name;
    
    [Header("Shooting")]
    public float damage;
    public int currentAmmo;
    public float bulletSpeed;
    public float impactForce;
    
    [Header("Reloading")]
    public int magSize;
    public float fireRate;
    public float reloadTime;
    private bool _reloading;

    [Header("Debugging")] 
    public bool DrawRaycastInScene;

    public bool GetReloadingStatus()
    {
        return this._reloading;
    }

    public void SetReloadingStatus(bool reloading)
    {
        this._reloading = reloading;
    }
    
}
