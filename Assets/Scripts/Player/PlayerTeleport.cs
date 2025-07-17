using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerTeleport : MonoBehaviour
{
    public GameObject mainCitySpawn;
    public GameObject arena01Spawn;
    private PlayerStats playerStats;
    private PlayerDebug playerDebug;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerDebug = GetComponent<PlayerDebug>();
    }

    public void TeleportToCity()
    {
        transform.position = mainCitySpawn.transform.position;
        playerStats.IncreaseQDamage(playerDebug.kills);
    }
    public void TeleportToArena01()
    {
        transform.position = arena01Spawn.transform.position;
    }
}
