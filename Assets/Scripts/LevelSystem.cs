using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public LevelData levelData;

    private void Awake()
    {
        levelData.Init();
    }
}
