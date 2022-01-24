using System.Collections;
using System.Collections.Generic;
using MilkShake;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public GameObject broken;
    public LevelData levelData;
    private bool alive = true;
    public ShakePreset blockShake;
    public SoundDef blockBreakSound;


    private void Start()
    {
        alive = true;
        levelData.AddBlock(gameObject);
    }
    // Update is called once per frame
    public void BreakMe()
    {
        if (!alive)
        {
            return;
        }
        gameObject.SetActive(false);
        gameObject.tag = "Untagged";
        //Destroy(gameObject);
        if (broken != null)
        {
            Instantiate(broken, transform.position, transform.rotation);
            if(blockShake != null)
            {
                Shaker.ShakeAll(blockShake);
            }
            SoundPlayer.PlaySound(blockBreakSound);
        }
        levelData.DestroyBlock();
        alive = false;
    }
}
