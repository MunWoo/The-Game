using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public BaseObject itemInfo;
    public MeshRenderer mesh;
    public Material material;

    void Start()
    {
        if (itemInfo.itemRarity == ItemRarity.Mythical)
        {
            mesh.enabled = true;
            mesh.material = material;
        }
        else
        {
            mesh.enabled = false;
        }
    }



    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
    }
}
