using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Node Settings")]
    public float sphereSize;

    public void SpawnEnemy(BaseEnemy baseEnemy)
    {
        Instantiate(baseEnemy, transform.position, Quaternion.identity);
    }
}
