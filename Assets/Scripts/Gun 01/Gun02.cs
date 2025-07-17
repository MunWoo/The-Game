using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun02 : MonoBehaviour
{
    public RectTransform crosshairUi;

    public PlayerStats playerStats;
    //Bullet
    public GameObject visualBullet;
    //Gun Damage
    public int gunDamage;
    //Bullet force
    public float shootForce, upwardForce;

    //GunStats
    public float timeBetweenShooting, timeBetweenShootingQ, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap, bulletsPerQ;
    public bool allowButtonHold;

    //Bullets on Magazine, Bullets Shot
    int bulletsLeft, bulletsShot, bulletsQShot;

    //bools
    bool shooting, readyToShoot, reloading, readyToShootQ;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public TextMeshProUGUI ammunitionDisplay;


    //Bug Fixing
    public bool allowInvoke = true;
    public bool allowInvokeQ = true;


    private void Awake()
    {
        //Make sure magazine is full and ready to shoot
        bulletsLeft = magazineSize;
        readyToShoot = true;
        readyToShootQ = true;
        playerStats = transform.parent.parent.GetComponent<PlayerStats>();
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
            //Set bullet shot to 033
            bulletsShot = 0;

            Shoot();

        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        int layerToIgnore = LayerMask.NameToLayer("whatIsPlayer");
        int layerMask = ~(1 << layerToIgnore); // Ignore player layer

        // Ray from center of screen
        Ray ray = fpsCam.ScreenPointToRay(crosshairUi.position);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
        }

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Perform raycast from attackPoint in the directionWithSpread
        if (Physics.Raycast(attackPoint.position, directionWithSpread.normalized, out hit, 100f, layerMask))
        {
            Debug.DrawLine(attackPoint.position, hit.point, Color.red, 100f);

            // Apply damage
            var damageable = hit.collider.GetComponent<BaseEnemy>();
            if (damageable != null)
            {
                damageable.TakeDamage(gunDamage);
            }

            //Instantiate visual bullet (tracer)
            SpawnVisualBullet(attackPoint.position, hit.point);

            // Optional: Spawn hit effect
            // Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else
        {
            Debug.DrawRay(attackPoint.position, directionWithSpread.normalized * 100f, Color.yellow, 1f);

            SpawnVisualBullet(attackPoint.position, attackPoint.position + directionWithSpread.normalized * 100f);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //If more than one bulletPerTap repeat shoot function
        //if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        //    Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
    private void ResetShotQ()
    {
        //Allow shooting and invoking again
        readyToShootQ = true;
        allowInvokeQ = true;
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
    public void ShootQ()
    {
        if (readyToShootQ)
        {
            readyToShootQ = false;
            int layerToIgnore = LayerMask.NameToLayer("whatIsPlayer");
            int layerMask = ~(1 << layerToIgnore); // Ignore player layer

            // Ray from center of screen
            Ray ray = fpsCam.ScreenPointToRay(crosshairUi.position);
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * 100f;
            }

            // Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            // Perform raycast from attackPoint in the directionWithSpread
            if (Physics.Raycast(attackPoint.position, directionWithSpread.normalized, out hit, 100f, layerMask))
            {
                Debug.DrawLine(attackPoint.position, hit.point, Color.red, 100f);

                // Apply damage
                var damageable = hit.collider.GetComponent<BaseEnemy>();
                if (damageable != null)
                {
                    damageable.TakeDamage(gunDamage);
                }

                //Instantiate visual bullet (tracer)
                SpawnVisualBullet(attackPoint.position, hit.point);

                // Optional: Spawn hit effect
                // Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Debug.DrawRay(attackPoint.position, directionWithSpread.normalized * 100f, Color.yellow, 1f);

                SpawnVisualBullet(attackPoint.position, attackPoint.position + directionWithSpread.normalized * 100f);
            }

            bulletsLeft--;
            bulletsShot++;

            //Invoke reset shot (if not already invoked)        Add cooldown here 
            if (allowInvokeQ)
            {
                Invoke("ResetShotQ", timeBetweenShootingQ);
                allowInvokeQ = false;
            }
        }

    }


    //Visual effects for the bullet to travel from the muzzle to the HitPoint
    private void SpawnVisualBullet(Vector3 start, Vector3 end)
    {
        GameObject visual = Instantiate(visualBullet, start, Quaternion.identity);
        visual.transform.forward = (end - start).normalized;
        float travelTime = 0.05f; // how fast tracer travels

        StartCoroutine(MoveAndDestroyBullet(visual, start, end, travelTime));
    }
    private IEnumerator MoveAndDestroyBullet(GameObject bullet, Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            bullet.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bullet.transform.position = end;
        Destroy(bullet);
    }

}




/*
 private void Shoot()
    {
        readyToShoot = false;
        int layerToIgnore = LayerMask.NameToLayer("whatIsPlayer");
        int layerMask = ~(1 << layerToIgnore); // invert bitmask to ignore Player layer

        Ray ray = fpsCam.ScreenPointToRay(crosshairUi.position);
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            targetPoint = hit.point; // Where the crosshair hits in the world
        }
        else
        {
            // fallback point far away (e.g. if aiming at sky)
            targetPoint = ray.origin + ray.direction * 1000f;
        }

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        Debug.DrawRay(attackPoint.position, directionWithoutSpread * 100f, Color.red);

        //Calculate Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with Spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to the last direction

        //Instantiate Bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //Rotate bullet to shoot Direction
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponentInChildren<bullet>().damage = gunDamage;

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
        //    Invoke("Shoot", timeBetweenShots);
    }
    */