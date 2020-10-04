using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public Transform pathHolder;
    public float viewAngle = 10f;
    public float viewDistance = 10f;
    public string state = "None";
    public float speed;
    public float waitTime;
    public float rotationSpeed;
    public float chaseDistance;
    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        Vector2[] waypoints = new Vector2[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector2(waypoints[i].x, waypoints[i].y);
        }
        StartCoroutine(Move(waypoints,true));
    }

    private void OnDrawGizmos()
    {
        Vector2 startPosition = pathHolder.GetChild(0).position;
        Vector2 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawLine(previousPosition, waypoint.position);
            Gizmos.DrawSphere(waypoint.position, 0.2f);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0,0, -viewAngle / 2) * transform.up * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0,0, viewAngle / 2) * transform.up * viewDistance);
    }

    private void Update()
    {
        Debug.Log(CanSeePlayer());
        if (state == "Scout")
        {
            if (CanSeePlayer())
            {
                state = "None";   
            }
        }
        if (state == "Chase")
        {
            if (!CanSeePlayer()){
                state = "None";
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Chase());
            }
        }
        if (state == "None")
        {
            if (!CanSeePlayer())
            {
                state = "Scout";
                Debug.Log("Scouting");
                Scout();
            }
            if (CanSeePlayer())
            {
                state = "Chase";
                Debug.Log("Chasing");
                StopAllCoroutines();
                StartCoroutine(Chase());
            }
        }
    }


    IEnumerator Chase()
    {
        Vector3 currentPlayerPosition = player.transform.position;

        while ((transform.position-currentPlayerPosition).magnitude > chaseDistance + 2f)
                {
                    currentPlayerPosition = player.transform.position;
                    yield return transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    Vector3 direction = (currentPlayerPosition - transform.position).normalized;             
        }
        while ((transform.position - currentPlayerPosition).magnitude < chaseDistance +2f)
        {
            currentPlayerPosition = player.transform.position;
            yield return transform.position = Vector3.MoveTowards(transform.position, -player.transform.position, speed * Time.deltaTime);
            Vector3 direction = (currentPlayerPosition - transform.position).normalized;
        }
    }
    private void Scout()
    {
        StopAllCoroutines();
        Vector2[] waypoints = new Vector2[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector2(waypoints[i].x, waypoints[i].y);
        }
        StartCoroutine(Move(waypoints, true));
    }

    IEnumerator Move(Vector2[] targetPositions, bool repeat)
    {
        while (true)
        {
            foreach (Vector2 targetPosition in targetPositions)
            {
                float boundary = 2f;
                Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.y);
                Vector2 direction = (targetPosition - transformPosition2D).normalized;
                float lookRotation = GetAngle(Vector2.up, direction);

                while ((GetAngle(Vector2.up, transform.up) > lookRotation + boundary) || (GetAngle(Vector2.up, transform.up) < lookRotation - boundary))
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(Vector3.forward*lookRotation),rotationSpeed*Time.deltaTime);
                    yield return null;
                }

                while (transformPosition2D != targetPosition)
                {

                    yield return transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    transformPosition2D = new Vector2(transform.position.x, transform.position.y);

                }


                yield return new WaitForSeconds(waitTime);
            }
            if (!repeat)
            {
                break;
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        if ((player.transform.position - transform.position).magnitude < viewDistance)
        {
            if (Vector3.Angle(transform.up, direction) < viewAngle / 2)
            {
                Ray2D ray = new Ray2D(transform.position, direction);
                RaycastHit2D hitInfo;
                hitInfo = Physics2D.Raycast(transform.position, direction, viewDistance);
                Debug.Log(hitInfo);
                if (hitInfo.collider != null)
                {
                    Debug.Log("Colission detected");
                    
                    Debug.DrawLine(transform.position, hitInfo.transform.position, Color.blue);
                    if (hitInfo.transform.gameObject.tag == "Player")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    private static float GetAngle(Vector2 v1, Vector2 v2)
    {
        var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
        return Vector2.Angle(v1, v2) * sign;
    }
}
