using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Scriptable Objects/Player Attributes")]
public class PlayerAttributesContainer : ScriptableObject
{
    public AttributeStat _maxHP;
    public AttributeStat _speed;

    public void UpdateAllAttributes()
    {
        _maxHP.ReCalculateValue();
        _speed.ReCalculateValue();
    }
}
