using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryDisplay : MonoBehaviour
{
    [SerializeField]
    private ItemSlot _itemThumbnailPrefab = null;
    [SerializeField]
    private RectTransform _contentTransform = null;
    [SerializeField]
    private bool _isWeaponInventory = false;

    private void OnEnable()
    {
        if (_isWeaponInventory)
        {
            EventManager.OnWeaponEquipped += AddNewItem;
        }
        else
        {
            EventManager.OnItemEquipped += AddNewItem;
        }
        
    }

    private void OnDisable()
    {
        if (_isWeaponInventory)
        {
            EventManager.OnWeaponEquipped -= AddNewItem;
        }
        else
        {
            EventManager.OnItemEquipped -= AddNewItem;
        }
    }

    private void AddNewItem(Item newItem)
    {
        ItemSlot newThumbnail = Instantiate(_itemThumbnailPrefab, _contentTransform);
        newThumbnail.Initialize(newItem);
    }
}
