using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerData : MonoBehaviour
{
    public static GlobalPlayerData instance;

    public int playerLevel = 1;
    public int playerExperience = 0;

    void Awake()
    {
        instance = this;
    }
}
