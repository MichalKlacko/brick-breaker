using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    //VERTICAL
    //private Vector3 offset = new Vector3(0f, -6.38f, -9.5f);

    //HORIZONTAL
    //private Vector3 offset = new Vector3(0f, 8, -8);
    private Vector3 offset = new Vector3(0f, 15, -10);
    private float smoothSpeed = 0.06f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -4.5f, 4.5f);
        transform.position = desiredPosition;//player.position + offset;
    }
}
