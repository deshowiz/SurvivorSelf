using UnityEngine;
using UnityEngine.UI;

public class ItemChoice : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image _itemImage = null;

    public void SetChoice(Item newItem)
    {
        _itemImage.sprite = newItem.ItemImage;
    }
}
