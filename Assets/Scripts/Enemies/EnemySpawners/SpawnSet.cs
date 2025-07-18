using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn Set", menuName = "Enemy Array/New Spawn Set")]
public class SpawnSets : ScriptableObject
{
    public string setName;
    public GameObject[] spawnPoints;
}
