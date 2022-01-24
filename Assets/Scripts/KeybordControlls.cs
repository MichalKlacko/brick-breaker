using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybordControlls : MonoBehaviour
{
    public BatScript swingScript;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //LEFT
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-.4f, 0f, 0f));
        }
        //RIGHT
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(.4f, 0f, 0f));
        }
        //LEFT
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0f, 0f, 0.2f));
        }
        //RIGHT
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0f, -0.2f));
        }
        //HIT
        if (Input.GetKey(KeyCode.Space))
        {

        }
    }
}
