using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour
{
    [SerializeField] float force = 0;
    [SerializeField] bool powerUp = false;
    [SerializeField] LevelData levelData;

    //private GameObject[] blocks;
    // Start is called before the first frame update
    void Start()
    {
        //blocks = GameObject.FindGameObjectsWithTag("Block");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ball"))
        {
            BallScript ballScript = collider.gameObject.GetComponent<BallScript>();
            Vector3 targetVelocity;
            Transform tMin = levelData.FindClosestBlock(transform.position);
            //float minDist = Mathf.Infinity;
            //Vector3 currentPos = transform.position;
            //foreach (GameObject block in blocks)
            //{
            //    if(block == null || !block.activeSelf)
            //    {
            //        continue;
            //    }
            //    float dist = Vector3.Distance(block.transform.position, currentPos);
            //    if (dist < minDist)
            //    {
            //        tMin = block.transform;
            //        minDist = dist;
            //    }
            //}
            if (tMin != null)
            {
                targetVelocity = (tMin.transform.position - transform.position).normalized * force;
                //collision.collider.attachedRigidbody.AddForce((tMin.transform.position - transform.position).normalized * force, ForceMode.Impulse); 
            }
            else
            {
                targetVelocity = new Vector3(0, 0, 1) * force;
                //collision.collider.attachedRigidbody.AddForce(new Vector3(0, 0, 1) * force, ForceMode.Impulse);
            }
            if (ballScript.WasHitRecently())
            {
                return;
            }
            ballScript.HitByBat(force, powerUp);
            collider.attachedRigidbody.velocity = targetVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            BallScript ballScript = collision.collider.gameObject.GetComponent<BallScript>();
            Vector3 targetVelocity;
            Transform tMin = levelData.FindClosestBlock(transform.position);
            //float minDist = Mathf.Infinity;
            //Vector3 currentPos = transform.position;
            //foreach (GameObject block in blocks)
            //{
            //    if(block == null || !block.activeSelf)
            //    {
            //        continue;
            //    }
            //    float dist = Vector3.Distance(block.transform.position, currentPos);
            //    if (dist < minDist)
            //    {
            //        tMin = block.transform;
            //        minDist = dist;
            //    }
            //}
            if (tMin != null)
            {
                targetVelocity = (tMin.transform.position - transform.position).normalized * force;
                //collision.collider.attachedRigidbody.AddForce((tMin.transform.position - transform.position).normalized * force, ForceMode.Impulse); 
            }
            else
            {
                targetVelocity = new Vector3(0, 0, 1) * force;
                //collision.collider.attachedRigidbody.AddForce(new Vector3(0, 0, 1) * force, ForceMode.Impulse);
            }
            if (ballScript.WasHitRecently() && collision.collider.attachedRigidbody.velocity.magnitude > force)
            {
                return;
            }
            ballScript.HitByBat(force, powerUp);
            collision.collider.attachedRigidbody.velocity = targetVelocity;
        }
    }
}
