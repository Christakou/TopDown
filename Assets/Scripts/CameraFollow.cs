using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
    public Vector3 offset;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
