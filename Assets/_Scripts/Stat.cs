using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] public int baseValue;
    [SerializeField] private int lastBaseValue;
    private float finalValue;
    private readonly List<StatModifier> modifiers;
    public readonly ReadOnlyCollection<StatModifier> statModifiers;
    [SerializeField] private bool isChanged = true; //tells when we need to recalculate finalvalue
    public int bonusValue = 0; //this is added from core stats, for example: for each str, 1 att is added, if we have 100 str
                               //then the bonus value for att will be 100 (added after all modifiers)
    private int lastBonusValue = 0;

    //the order in which the modifier list is arrange (Flat => PercentAdd => PercentMul)
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMul = 300,
    }

    public Stat()
    {
        modifiers = new List<StatModifier>();
        statModifiers = modifiers.AsReadOnly();
    }
    public Stat(int baseValue) : this()
    {
        this.baseValue = baseValue;
        lastBaseValue = baseValue;

    }
    //calculate the final value with all the modifiers (calculating flat values first and then percentage values
    public float GetFinalValue()
    {
        if (isChanged == true || baseValue != lastBaseValue)
        {
            finalValue = baseValue;
            lastBaseValue = baseValue;
            float sumPercentAdd = 0f;
            for(int i = 0; i < modifiers.Count; i++)
            {
                StatModifier mod = modifiers[i];
                if (mod.type == StatModType.Flat)
                {
                    finalValue += mod.value;
                }
                else if(mod.type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.value;
                    //if its the last modifier in the list or last PercentAdd modifier in the list
                    if (i + 1 >= modifiers.Count || modifiers[i + 1].type != StatModType.PercentAdd)
                        finalValue *= sumPercentAdd;
                }
                else if(mod.type == StatModType.PercentMul)
                {
                    finalValue *= (1 + mod.value);
                }
            }
            finalValue += bonusValue;
            lastBonusValue = bonusValue;
            isChanged = false;
        }
        else
        {
            finalValue -= lastBonusValue;
            finalValue += bonusValue;
            lastBonusValue = bonusValue;
        }
        return finalValue;
    }
    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.order == b.order)
            return 0;
        else if (a.order < b.order)
            return -1;
        return 1;// if (a.order > b.order
    }
    public void AddModifier(StatModifier mod)
    {
        if (mod.value != 0)
        {
            modifiers.Add(mod);
            isChanged = true;
            modifiers.Sort(CompareModifierOrder);
        }
    }
    public bool RemoveModifier(StatModifier mod)
    {
        if (modifiers.Remove(mod))
        {
            isChanged = true;
            return true;
        }
        return false;
    }
    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for(int i = modifiers.Count - 1;i >= 0; i--)
        {
            StatModifier mod = modifiers[i];
            if(mod.source != null && mod.source == source)
            {
                modifiers.RemoveAt(i);
                isChanged = true;
                didRemove = true;
            }
        }
        return didRemove;
    }

}
