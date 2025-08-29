using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Scriptable Objects/Player Attributes")]
public class PlayerAttributesContainer : ScriptableObject
{
    [Header("Character Details")]
    [SerializeField]
    private Sprite _characterSprite = null;
    public Sprite CharacterSprite { get { return _characterSprite; } }
    [SerializeField]
    private string _characterDescription = "";
    public string CharacterDescription { get { return _characterDescription; } }
    [SerializeField]
    private WeaponItem[] _startingWeapons = null;
    public WeaponItem[] StartingWeapons { get { return _startingWeapons; } }

    [Header("Character Stats")]
    public AttributeStat _maxHP;
    [SerializeField]
    private int _defaultMaxHP = 10;
    [SerializeField]
    private float _multiplierMaxHP = 1;

    public AttributeStat _speed;
    [SerializeField]
    private float _defaultSpeed = 1;
    [SerializeField]
    private float _multiplierSpeed = 1;

    public void ResetAttributesToDefaultValues()
    {
        _maxHP.FullReset(_defaultMaxHP, _multiplierMaxHP);
        _speed.FullReset(_defaultSpeed, _multiplierSpeed);
    }

    public void UpdateAllAttributes()
    {
        _maxHP.ReCalculateValue();
        _speed.ReCalculateValue();
    }

    public void UpdateAllChangedValues(List<AttributeStat> changedAttributes)
    {
        for (int i = 0; i < changedAttributes.Count; i++)
        {
            changedAttributes[i].ReCalculateValue();
        }
        EventManager.OnAttributeChange?.Invoke();
    }
}
