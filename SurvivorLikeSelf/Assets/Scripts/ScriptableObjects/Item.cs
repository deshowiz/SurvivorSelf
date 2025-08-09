using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("References")]
    [SerializeField]
    private Sprite _itemImage = null;
    public Sprite ItemImage { get { return _itemImage; } }
    public enum ItemType { None, Test }
    [SerializeField]
    private ItemType _itemType = ItemType.None;
    public ItemType ItemTypes { get { return _itemType; } }
    [SerializeField]
    private string _itemDescriptionText = "";
    public string ItemDescriptionText { get { return _itemDescriptionText; } }

    [SerializeField]
    private Rarity _itemRarity = null;
    public Rarity Rarity { get { return _itemRarity; }}
    
}
