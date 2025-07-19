using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SpawnSets : MonoBehaviour
{
    public string setName;
    public SpawnPoint[] spawnPoints;
    void Awake()
    {
        var list = new System.Collections.Generic.List<SpawnPoint>();
        foreach (Transform child in transform)
        {
            SpawnPoint spawnPoint = child.GetComponent<SpawnPoint>();
            if (spawnPoint != null)
            {
                list.Add(spawnPoint);
            }
        }
        spawnPoints = list.ToArray();
    }
}
