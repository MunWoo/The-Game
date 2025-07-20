using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageSpawners : MonoBehaviour
{
    public SpawnSets[] spawnSets;
    void Awake()
    {
        var list = new System.Collections.Generic.List<SpawnSets>();
        foreach (Transform child in transform)
        {
            SpawnSets spawnSets = child.GetComponent<SpawnSets>();
            if (spawnSets != null)
            {
                list.Add(spawnSets);
            }
        }
        spawnSets = list.ToArray();
    }
}
