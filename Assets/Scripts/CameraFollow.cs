using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    // Start is called before the first frame update
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
