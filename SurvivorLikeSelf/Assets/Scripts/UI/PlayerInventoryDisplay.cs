using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryDisplay : MonoBehaviour
{
    [SerializeField]
    private ItemSlot _itemThumbnailPrefab = null;
    [SerializeField]
    private RectTransform _contentTransform = null;

    private void OnEnable()
    {
        EventManager.OnItemEquipped += AddNewItem;
    }

    private void OnDisable()
    {
        EventManager.OnItemEquipped -= AddNewItem;
    }

    private void AddNewItem(Item newItem)
    {
        ItemSlot newThumbnail = Instantiate(_itemThumbnailPrefab, _contentTransform);
        newThumbnail.Initialize(newItem);
    }
}
