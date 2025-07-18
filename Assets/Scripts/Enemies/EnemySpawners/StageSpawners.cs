using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage Spawners", menuName = "Enemy Array/New Stage Spawners")]
public class StageSpawners : ScriptableObject
{
    public SpawnSets[] spawnSets;
}
