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
    public string state = "Scout";
    public float speed;
    public float waitTime;
    public float rotationSpeed;
    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(Move(waypoints,false));
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
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
        if (state == "Scount")
        {

        }
    }

    IEnumerator Move(Vector3[] targetPositions, bool repeat)
    {
        while (true)
        {
            foreach (Vector3 targetPosition in targetPositions)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                while (transform.rotation != lookRotation)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                    yield return null;
                }

                while (transform.position != targetPosition)
                {

                    yield return transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                }


                yield return new WaitForSeconds(waitTime);
            }
            if (!repeat)
            {
                break;
            }
        }
    }
}
