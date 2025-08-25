using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Scriptable Objects/Player Attributes")]
public class PlayerAttributesContainer : ScriptableObject
{
    public AttributeStat _maxHP;
    [SerializeField]
    private int _defaultMaxHP = 10;
    [SerializeField]
    private int _multiplierMaxHP = 1;
    
    public AttributeStat _speed;
    [SerializeField]
    private float _defaultSpeed = 1;
    [SerializeField]
    private int _multiplierSpeed = 1;

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
}
