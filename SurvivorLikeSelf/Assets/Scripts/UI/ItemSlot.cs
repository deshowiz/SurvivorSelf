using UnityEngine.UI;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image _slotThumbnail = null;
    [SerializeField]
    private Item _heldItem = null;
    public Item HeldItem { get { return _heldItem; } }

    public RectTransform GetRectTransform {get { return _slotThumbnail.rectTransform; }}

    public void Initialize(Item newItem)
    {
        _heldItem = newItem;
        _slotThumbnail.sprite = _heldItem.ItemImage;
    }

    public void Hovered()
    {
        _slotThumbnail.color = Color.gray;
        EventManager.OnItemHovered?.Invoke(this);
    }

    public void ExitHover()
    {
        _slotThumbnail.color = Color.white;
        EventManager.OnItemExitHover?.Invoke();
    }
}
