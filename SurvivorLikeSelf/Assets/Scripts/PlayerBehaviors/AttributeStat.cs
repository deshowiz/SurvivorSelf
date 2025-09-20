using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Kryz.CharacterStats;
using UnityEditor.AnimatedValues;
using UnityEngine;

[Serializable]
public class AttributeStat
{
    private float _baseValue;

    [SerializeField]
    private float _value;
    public float Value { get { return _value; } }

    private float _multiplier = 1f;

    public AttributeStat(float newBaseValue)
    {
        this._baseValue = newBaseValue;
        this._value = newBaseValue;
    }


    private List<StatModifier> _statmodifiers = new List<StatModifier>();

    public void AddModifier(StatModifier mod)
    {
        //_isDirty = true;
        _statmodifiers.Add(mod);
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if (_statmodifiers.Remove(mod))
        {
            //_isDirty = true;
            return true;
        }
        return false;
    }

    public float CalculateFinalValue()
    {
        float finalValue = _baseValue;
        float additive = 0f;
        float sumPercentAdd = 0f;

        for (int i = 0; i < _statmodifiers.Count; i++)
        {
            StatModifier currentMod = _statmodifiers[i];

            // Can be done with a simple bool, but keeping enums for expansion later if needed, and to reiterate the lesson on enums
            if (currentMod.Type == StatModType.Flat)
            {
                additive += currentMod.Value;
            }
            else // Can be done with a simple bool, but keeping enums for expansion later if needed, and to reiterate the lesson on enums
            {
                sumPercentAdd += currentMod.Value;
            }
        }

        additive *= _multiplier;
        sumPercentAdd = 1 + (sumPercentAdd * _multiplier);

        finalValue += additive;
        finalValue *= sumPercentAdd;
        

        return finalValue;
    }

    public void ReCalculateValue()
    {
        _value = CalculateFinalValue();
    }

    public void FullReset(float resetValue, float newMultiplier)
    {
        _multiplier = newMultiplier;
        _baseValue = resetValue;
        _value = resetValue;
        _statmodifiers.Clear();
    }
}
