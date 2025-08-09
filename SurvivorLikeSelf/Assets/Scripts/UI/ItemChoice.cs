using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemChoice : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image _itemImage = null;
    [SerializeField]
    private Image _backgroundImage = null;
    [SerializeField]
    private TextMeshProUGUI _itemNameText = null;
    [SerializeField]
    private TextMeshProUGUI _itemSubtypeText = null;
    [SerializeField]
    private TextMeshProUGUI _itemDescriptionText = null;

    public void SetChoice(Item newItem)
    {
        _itemImage.sprite = newItem.ItemImage;
        _backgroundImage.color = newItem.Rarity.RarityColor;
        _itemNameText.text = newItem.name;
        _itemSubtypeText.text = newItem.ItemTypes.ToString();
        _itemDescriptionText.text = newItem.ItemDescriptionText;
    }
}
