using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();
[System.Serializable]
public class ModifiableInt
{
    [SerializeField]
    private float baseValue;
    public float BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }

    [SerializeField]
    private float modifiedValue;
    public float ModifiedValue { get { return modifiedValue; } private set { modifiedValue = value; } }

    public List<IModifiers> modifiers = new List<IModifiers>();
    public event ModifiedEvent ValueModified;
    public ModifiableInt(ModifiedEvent method = null)
    {
        modifiedValue = BaseValue;
        if (method != null)
            ValueModified += method;
    }

    public void RegisterModEvent(ModifiedEvent method)
    {
        ValueModified += method;
    }
    public void UnRegisterModEvent(ModifiedEvent method)
    {
        ValueModified -= method;
    }

    public void UpdateModifiedValue()
    {
        float valueToAdd = 0;
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].AddValue(ref valueToAdd);
        }
        ModifiedValue = baseValue + valueToAdd;
        if (ValueModified != null)
            ValueModified.Invoke();
    }

    public void AddModifier(IModifiers _modifiers)
    {
        modifiers.Add(_modifiers);
        UpdateModifiedValue();
    }
    public void RemoveModifier(IModifiers _modifiers)
    {
        modifiers.Remove(_modifiers);
        UpdateModifiedValue();
    }

}

