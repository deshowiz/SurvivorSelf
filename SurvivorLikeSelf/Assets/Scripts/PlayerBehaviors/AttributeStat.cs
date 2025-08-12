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
    public float _baseValue;

    //private bool _isDirty = true;
    //private float _lastBaseValue;
    [SerializeField]
    private float _value;

    public AttributeStat(float newBaseValue)
    {
        this._baseValue = newBaseValue;
        this._value = newBaseValue;
    }

    public float Value
    {
        get
        {
            // if(_isDirty || _lastBaseValue != BaseValue) {
            // 		lastBaseValue = BaseValue;
            // 		_value = CalculateFinalValue();
            // 		isDirty = false;
            // 	}
            _value = CalculateFinalValue();
            return _value;
        }
    }

    private readonly List<StatModifier> _statmodifiers = new List<StatModifier>();
    // Similar to a property List<StatModifier> that only has a getter
    public ReadOnlyCollection<StatModifier> StatModifiers;

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

    // Make an assignment where a new subclass AttributeStatPercent is made for the StatModType.PercentAdd
    public float CalculateFinalValue()
    {
        float finalValue = _baseValue;
        float sumPercentAdd = 1f;

        for (int i = 0; i < _statmodifiers.Count; i++)
        {
            StatModifier currentMod = _statmodifiers[i];

            // Can be done with a simple bool, but keeping enums for expansion later if needed, and to reiterate the lesson on enums
            if (currentMod.Type == StatModType.Flat)
            {
                finalValue += currentMod.Value;
            }
            else // Can be done with a simple bool, but keeping enums for expansion later if needed, and to reiterate the lesson on enums
            {
                sumPercentAdd += currentMod.Value;
            }
        }

        finalValue *= sumPercentAdd;

        return finalValue;
    }

    public void ReCalculateValue()
    {
        _value = CalculateFinalValue();
    }
}
