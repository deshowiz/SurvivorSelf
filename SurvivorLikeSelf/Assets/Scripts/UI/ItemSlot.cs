using UnityEngine.UI;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image _slotThumbnail = null;
    [SerializeField]
    private Item _heldItem = null;

    public void Initialize(Item newItem)
    {
        _heldItem = newItem;
        _slotThumbnail.sprite = _heldItem.ItemImage;
    }

    public void Hovered()
    {
        _slotThumbnail.color = Color.gray;
    }

    public void ExitHover()
    {
        _slotThumbnail.color = Color.white;
    }
}
