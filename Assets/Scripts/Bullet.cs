using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject hitEffect;
    public GameObject tracerEffect;
    public float timeTillSelfDestroy;
    public float perpendicularForce;
    public bool tracer;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
        
    }

    private void FixedUpdate()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.Perpendicular(transform.up) * perpendicularForce * Time.fixedDeltaTime,ForceMode2D.Impulse);
        if (tracer)
        {
            GameObject trace = Instantiate(tracerEffect, transform.position, Quaternion.identity);
            Destroy(trace, 1f);
        }
    }
    private void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
