using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun02 : MonoBehaviour
{
    //Bullet
    public GameObject bullet;
    //Gun Damage
    public int gunDamage;
    //Bullet force
    public float shootForce, upwardForce;

    //GunStats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShoots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    //Bullets on Magazine, Bullets Shot
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public TextMeshProUGUI ammunitionDisplay;


    //Bug Fixing
    public bool allowInvoke = true;


    private void Awake()
    {
        //Make sure magazine is full and ready to shoot
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Set ammo display if exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }

    public void MyInput()
    {
        //Check if allowed to hold down fire button
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload Automaticaly when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullet shot to 0
            bulletsShot = 0;

            Shoot();

        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        // Check if the raycast hit anything
        Vector3 targetPoint;

        targetPoint = fpsCam.transform.position + fpsCam.transform.forward * 75; // A point far from the camera if not hitting anything

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with Spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to the last direction

        //Instantiate Bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //Rotate bullet to shoot Direction
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<bullet>().damage = gunDamage;

        //Add Forces to Bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.//up * shootForce, ForceMode.Impulse); //Add force up, or another direction


        bulletsLeft--;
        bulletsShot++;

        //Invoke reset shot (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //If more than one bulletPerTap repeat shoot function
        //if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        //    Invoke("Shoot", timeBetweenShoots);
    }

    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}
