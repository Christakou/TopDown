using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    bool mouseDown = false;
    public float bulletForce = 20f;
    float perpendicularForceCounter;
    public float maxCurvatureForce = 100f;

    float angleAtStartOfClick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            angleAtStartOfClick = FindDegree(transform.up.x, transform.up.y);
          
            mouseDown = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            float angleDeviation = angleAtStartOfClick-FindDegree(transform.up.x, transform.up.y);
            mouseDown = false;
            Debug.Log("fire");
            Shoot(angleDeviation);
            Debug.Log("Angle deviation:" + angleDeviation);

            perpendicularForceCounter = 0;
        }
        if (mouseDown)
        {
            StartCoroutine(perpendicularForceCounterUp());
        }
    }

    IEnumerator perpendicularForceCounterUp()
    {
        yield return perpendicularForceCounter++;
        Debug.Log(perpendicularForceCounter);
    }
    void Shoot(float angleDifference)
    {
        Debug.Log(angleDifference);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        float deviationMagnitude = Mathf.Lerp(0f, maxCurvatureForce, Mathf.Abs(angleDifference)/ 180);
        float deviationDirection = Mathf.Sign(angleDifference);
        float deviation = deviationMagnitude * deviationDirection;
        Debug.Log("Deviation = " + deviation.ToString());
        
        bullet.GetComponent<Bullet>().perpendicularForce = deviation;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    public static float FindDegree(float x, float y)
    {
        float value = (float)((Mathf.Atan2(x, y) / Mathf.PI) * 180f);
        if (value < 0) value += 360f;

        return value;
    }
}
