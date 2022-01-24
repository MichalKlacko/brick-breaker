using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyMeDelay());
    }

    private IEnumerator DestroyMeDelay()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
