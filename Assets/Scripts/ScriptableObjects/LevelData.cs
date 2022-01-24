using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    private List<GameObject> blocks = new List<GameObject>();
    private int blockCount;
    // Start is called before the first frame update
    public void Init()
    {
        blocks = new List<GameObject>();
        blockCount = 0;
    }

    public void AddBlock(GameObject block)
    {
        blocks.Add(block);
        blockCount++;
    }

    public void DestroyBlock()
    {
        blockCount--;
    }

    public int GetBlockCount()
    {
        return blockCount;
    }

    public Transform FindClosestBlock(Vector3 currentPos)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject block in blocks)
        {
            if (block == null || !block.activeSelf)
            {
                continue;
            }
            float dist = Vector3.Distance(block.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = block.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    public int CountBlocks()
    {
        return blocks.Count;
    }
}
