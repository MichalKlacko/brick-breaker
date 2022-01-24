using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwingTriggerScript : MonoBehaviour
{
    public BatScript swingScript;
    public bool right = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            swingScript.SwingNow(right);
        }
    }
}
