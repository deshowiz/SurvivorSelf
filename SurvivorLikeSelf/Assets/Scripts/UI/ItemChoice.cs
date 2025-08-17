using System.Runtime.CompilerServices;
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
    [SerializeField]
    private Button _purchaseButton = null;
    [SerializeField]
    private TextMeshProUGUI _purchasePriceText = null;

    private Item _currentItem = null;
    private int _currentPrice = 0;

    private void OnEnable()
    {
        EventManager.OnGoldChange += SetPurchaseButtonFromPrice;
    }

    private void OnDisable()
    {
        EventManager.OnGoldChange -= SetPurchaseButtonFromPrice;
    }

    public void SetChoice(Item newItem, int heldGold)
    {
        _itemImage.sprite = newItem.ItemImage;
        _backgroundImage.color = newItem.Rarity.RarityColor;
        _itemNameText.text = newItem.name;
        _itemSubtypeText.text = newItem.ItemTypes.ToString();
        _itemDescriptionText.text = newItem.GetItemDescriptionText();
        _currentItem = newItem;
        _currentPrice = newItem.Rarity.NewPrice;
        _purchasePriceText.text = _currentPrice.ToString();
        SetPurchaseButtonFromPrice(heldGold);
        gameObject.SetActive(true);
    }

    private void SetPurchaseButtonFromPrice(int heldGold)
    {
        if (heldGold >= _currentPrice)
        {
            _purchaseButton.interactable = true;
            _purchasePriceText.color = Color.white;
        }
        else
        {
            _purchaseButton.interactable = false;
            _purchasePriceText.color = Color.red;
        }
    }

    public void PurchaseItem()
    {
        if (_currentItem != null)
        {
            if (_currentItem is WeaponItem)
            {
                EventManager.OnWeaponEquipped?.Invoke((WeaponItem)_currentItem);
            }
            else
            {
                EventManager.OnItemEquipped?.Invoke(_currentItem);
            }
            EventManager.OnPurchasedItem?.Invoke(_currentPrice);
        }
        else
        {
            Debug.LogError("ItemChoice is missing item ref");
        }
        _currentItem = null;
        gameObject.SetActive(false);
    }
}
