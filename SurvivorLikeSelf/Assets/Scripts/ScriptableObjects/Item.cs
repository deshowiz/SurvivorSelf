using System;
using System.Collections.Generic;
using System.Linq;
using Kryz.CharacterStats;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("References")]
    [SerializeField]
    private Sprite _itemImage = null;
    public Sprite ItemImage { get { return _itemImage; } }

    [SerializeField]
    [TextArea(10, 10)]
    private string _itemDescriptionText = "";
    public string ItemDescriptionText { get { return _itemDescriptionText; } }

    [Header("Settings")]

    [SerializeField]
    private Rarity _itemRarity = null;
    public Rarity Rarity { get { return _itemRarity; } }
    public enum ItemType { None, Test }
    [SerializeField]
    private ItemType _itemType = ItemType.None;
    public ItemType ItemTypes { get { return _itemType; } }

    [Header("Stat Modifiers")]
    [SerializeField]
    private List<StatModifierInfo> _statModifiers = new List<StatModifierInfo>();

    [Serializable]
    private struct StatModifierInfo
    {
        public StatTag _statTag;
        public StatModifier _modifier;
    }

    private enum StatTag { MaxHP, Speed }

    private delegate void EquipStatRefresh();

    private EquipStatRefresh _OnEquippedItem;

    public virtual bool Equip(Player player)
    {
        if (_statModifiers == null) return true;

        List<AttributeStat> _playerStatsChanged = new List<AttributeStat>();

        for (int i = 0; i < _statModifiers.Count; i++)
        {
            switch (_statModifiers[i]._statTag)
            {
                case StatTag.Speed:
                    player._speed.AddModifier(_statModifiers[i]._modifier);
                    _playerStatsChanged.Add(player._speed);
                    break;
                case StatTag.MaxHP:
                    player._maxHP.AddModifier(_statModifiers[i]._modifier);
                    _playerStatsChanged.Add(player._maxHP);
                    break;
            }
        }

        player.ReCalcAllChangedStats(_playerStatsChanged);

        return true;
    }

    public virtual string GetItemDescriptionText()
    {
        string _descriptionAdditive = _itemDescriptionText;
        if (!String.IsNullOrEmpty(_descriptionAdditive))
        {
            _descriptionAdditive += "\n";
        }

        for (int i = 0; i < _statModifiers.Count; i++)
        {
            string signString;
            float modValue = _statModifiers[i]._modifier.Value;


            if (Mathf.Sign(modValue) == 1f)
            {
                signString = "<color=#09C829>+";
            }
            else
            {
                signString = "<color=#ff0000ff>";
            }
            switch (_statModifiers[i]._modifier.Type)
            {
                case StatModType.Flat:
                    _descriptionAdditive += signString + modValue.ToString() + " " + _statModifiers[i]._statTag.ToString() + "</color> " + "\n";
                    break;
                case StatModType.PercentAdd:
                    _descriptionAdditive += signString + (modValue * 100f).ToString() + "% " + _statModifiers[i]._statTag.ToString() + "</color> " + "\n";
                    break;
            }
        }
        
        return _descriptionAdditive;
    }
}
