using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text bulletCount;
    public GameObject mag;
    public GameObject magazineHolder;
    public float horizontalOffset;
    public Vector3 startingPoint;
    // Update is called once per frame
    void UpdateBulletCount()
    {
        bulletCount.text = FindObjectOfType<Shooting>().bulletCount.ToString();
    }


    void DrawMags()
    {
        foreach (Transform child in magazineHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < FindObjectOfType<Shooting>().magazines; i++)
        {
            GameObject magazine = Instantiate(mag, magazineHolder.transform.position+ Vector3.right*i*horizontalOffset, transform.rotation);
            magazine.transform.parent = magazineHolder.transform;
        }

    }
    private void Start()
    {
        DrawMags();
        UpdateBulletCount();

        Shooting.OnHUDUpdate += DrawMags;
        Shooting.OnHUDUpdate += UpdateBulletCount;

    }
}
