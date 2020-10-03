using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Animator animator;
    public static event System.Action OnHUDUpdate;

    bool mouseDown = false;
    public float bulletForce = 20f;
    public float maxCurvatureForce = 5000f;
    public int bulletCount = 25;
    public int magazines = 10;
    public int magSize = 25;

    private float fixedDeltatime;

    Vector2 directionAtStartOfClick;
    Vector2 directionAtEndOfClick;


    private void Awake()
    {
        fixedDeltatime = Time.fixedDeltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            directionAtStartOfClick = transform.up;
          
            mouseDown = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {

            if (bulletCount > 0)
            {
                directionAtEndOfClick = transform.up;
                float angleDeviation = GetAngle(directionAtStartOfClick, directionAtEndOfClick);
                Shoot(-angleDeviation);
            }
            else
            {
                Debug.Log("out of ammo");
            }
            mouseDown = false;

        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {

            if (magazines > 0 && bulletCount < magSize)
            {
                Reload();
            }
            else
            {
                Debug.Log("out of mags");
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 0.12f;
            Time.fixedDeltaTime = Time.timeScale * fixedDeltatime;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.timeScale * fixedDeltatime;
        }
    }


    void Reload()
    {      
        animator.SetTrigger("ReloadTrigger");
        magazines--;
        bulletCount = magSize;
        OnHUDUpdate();
    }
    void Shoot(float angleDifference)
    {
        animator.SetTrigger("FireTrigger");
        Debug.Log(angleDifference);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        float deviationMagnitude = Mathf.Lerp(0f, maxCurvatureForce, Mathf.Abs(angleDifference)/ 180);
        float deviationDirection = Mathf.Sign(angleDifference);
        float deviation = deviationMagnitude * deviationDirection;
        Debug.Log("Deviation = " + deviation.ToString());
        
        bullet.GetComponent<Bullet>().perpendicularForce = deviation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        bulletCount--;
        OnHUDUpdate();
    }

    private static float GetAngle(Vector2 v1, Vector2 v2)
    {
        var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
        return Vector2.Angle(v1, v2) * sign;
    }
}
