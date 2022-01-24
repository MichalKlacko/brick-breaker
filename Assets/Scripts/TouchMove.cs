using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMove : MonoBehaviour
{
    int layerMask = 1 << 7;
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {

        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    if(Time.timeScale == 0)
                    {
                        Time.timeScale = 1;
                    }
                    break;
                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    if(Time.timeScale == 0)
                    {
                        return;
                    }
                    //rb.MovePosition(new Vector3(rb.position.x + (touch.deltaPosition.x * .04f), rb.position.y, rb.position.z + (touch.deltaPosition.y * .03f)));
                    Vector3 target = transform.position + new Vector3(touch.deltaPosition.x * .05f, 0, touch.deltaPosition.y * .03f);
                    RaycastHit hit;
                    //transform.position = target;
                    if (Physics.Raycast(target, Vector3.down, Mathf.Infinity, layerMask))
                    {
                        target = new Vector3(target.x, target.y, Mathf.Max(target.z, -2f));
                        transform.position = target;
                    }
                    else
                    {
                        if (Physics.Raycast(transform.position, transform.TransformDirection(target), out hit, Mathf.Infinity, layerMask))
                        {
                            if (hit.distance > 1f)
                            {
                                target = Vector3.MoveTowards(transform.position, target, hit.distance-1f);
                                target = new Vector3(target.x, target.y, Mathf.Max(target.z, -2f));
                                transform.position = target;
                            }
                            //Debug.DrawRay(transform.position, transform.TransformDirection(target) * hit.distance, Color.yellow);
                        }
                    }
                    //transform.Translate(Vector3.Lerp(transform.position, new Vector3(Mathf.Min(touch.deltaPosition.x * .04f, 1f), 0, touch.deltaPosition.y * .03f)), 3f * Time.deltaTime);
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }
}
