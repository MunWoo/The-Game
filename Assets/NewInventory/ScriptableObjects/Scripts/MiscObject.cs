using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New X Inventory", menuName = "X Inventory System/Misc Object")]
public class MiscObject : BaseObject
{
    [TextArea(15, 20)]
    public string Description;
}