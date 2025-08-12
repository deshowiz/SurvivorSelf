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

    public virtual bool Equip(Player player)
    {
        for (int i = 0; i < _statModifiers.Count; i++)
        {
            switch (_statModifiers[i]._statTag)
            {
                case StatTag.Speed:
                    player._speed.AddModifier(_statModifiers[i]._modifier);
                    break;
                case StatTag.MaxHP:
                    player._maxHP.AddModifier(_statModifiers[i]._modifier);
                    break;
            }
        }

        return true;
    }

    public string GetItemDescriptionText()
    {
        string descriptionAdditive = _itemDescriptionText;
        if (!String.IsNullOrEmpty(descriptionAdditive))
        {
            descriptionAdditive += "\n";
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
                    descriptionAdditive += signString + modValue.ToString() + " " + _statModifiers[i]._statTag.ToString() + "</color> " + "\n";
                    break;
                case StatModType.PercentAdd:
                    descriptionAdditive += signString + (modValue * 100f).ToString() + "% " + _statModifiers[i]._statTag.ToString() + "</color> " + "\n";
                    break;
            }
        }

        return descriptionAdditive;
    }
}
