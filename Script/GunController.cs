using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform gun;
    [SerializeField] Animator animatorGun;
    [SerializeField] float gunDistance = 0.8f;
    bool gunRight = true;
    Vector3 direction;
    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 15f;
    [SerializeField] int maxBullets = 20;
    int currentBullets;
    bool _reloadingBullet;
    public bool reloadingBullet 
    { 
        get { return _reloadingBullet;}
        set { _reloadingBullet = value;}
    }
    void Start() 
    {
        ReloadBullets();
    }
    void Update()
    {
        RotationGun();
    }
    void OnFire(InputValue inputValue)
    {
        if(inputValue.isPressed && HaveBullets() && !_reloadingBullet && !UIDisplay.instance.isGameOver)
        {
            Shoot();
        }
    }
    // void OnReloadBullets(InputValue inputValue)
    // {
    //     if(inputValue.isPressed)
    //         ReloadBullets();
    // }
    // public bool GetStateReloadBullet()
    // {
    //     return reloadingBullet;
    // }
    // public void SetStateReloadBullet(bool value)
    // {
    //     reloadingBullet = value;
    // }
    void RotationGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        gun.position = transform.position + Quaternion.Euler(new Vector3(0, 0, angle)) * new Vector3(gunDistance, 0f, 0f);
        FlipGunController(mousePos);
        
    }
    void FlipGunController(Vector3 mousePos)
    {
        if(mousePos.x < gun.position.x && gunRight)
        {
            FlipGun();
        }
        else if(mousePos.x > gun.position.x && !gunRight)
        {
            FlipGun();
        }
    }
    void FlipGun()
    {
        gunRight = !gunRight;
        gun.localScale = new Vector3(gun.localScale.x, gun.localScale.y * - 1, gun.localScale.z);
    }
    void Shoot()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        animatorGun.SetTrigger("Shooting");

        UIDisplay.instance.UpdateAmmo(currentBullets, maxBullets);

        GameObject newBullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
        Debug.Log(direction.normalized);
        Destroy(newBullet, 7);
    }

    bool HaveBullets()
    {
        if(currentBullets <= 0)
            return false;
        currentBullets--;
        return true;  
    }
    public void ReloadBullets()
    {
        Time.timeScale = 1;
        currentBullets = maxBullets;
        UIDisplay.instance.UpdateAmmo(currentBullets, maxBullets);
    }
}
