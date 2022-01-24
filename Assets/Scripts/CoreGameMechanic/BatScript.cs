using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BatScript : MonoBehaviour
{
    private Vector3 defaultRotationRight = new Vector3(0, 10, 0);
    private Vector3 defaultRotationLeft = new Vector3(0, 90, 0);
    private Vector3 swingEndRotationRight = new Vector3(0, 200, 0);
    private Vector3 swingEndRotationLeft = new Vector3(0, 270, 0);
    //private Rigidbody rb;
    private bool defaultPost = true;
    public GameObject respawnPrefab;
    public List<TrailRenderer> trails = new List<TrailRenderer>();
    // Start is called before the first frame update
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!defaultPost)
        {
            return;
        }
        if (transform.position.x < FindClosestBall().position.x)
        {
            transform.rotation = Quaternion.Euler(0, 10, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    public void SetDefault(bool right)
    {
        if (right)
        {
            transform.DORotate(defaultRotationRight, 1f).OnComplete(() => {
                defaultPost = true;
            });
        }
        else
        {
            transform.DORotate(defaultRotationLeft, 1f).OnComplete(() => {
                defaultPost = true;
            });
        }
    }

    public void SwingNow(bool right)
    {
        defaultPost = false;
        EnableTrail();
        if (right)
        {
            transform.DORotate(swingEndRotationRight, 0.2f).OnComplete(() =>
            {
                DisableTrail();
                SetDefault(right);
            });
        } else {
            transform.DORotate(swingEndRotationLeft, 0.2f).OnComplete(() =>
            {
                DisableTrail();
                SetDefault(right);
            });
        }
        
    }

    private Transform FindClosestBall()
    {
        GameObject[] balls;
        balls =  GameObject.FindGameObjectsWithTag("Ball");
        GameObject closestBall = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject ball in balls)
        {
            if (ball == null || !ball.activeSelf)
            {
                continue;
            }
            float dist = Vector3.Distance(ball.transform.position, transform.position);
            if (dist < minDist)
            {
                closestBall = ball;
            }
        }
        return closestBall ? closestBall.transform : null;
    }

    private void DisableTrail()
    {
        trails.ForEach((trail)=>
        {
            trail.enabled = false;
        });
    }

    private void EnableTrail()
    {
        trails.ForEach((trail) =>
        {
            trail.enabled = true;
        });
    }
}
