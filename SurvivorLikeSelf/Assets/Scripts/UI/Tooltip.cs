using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Image _panelImage = null;
    [SerializeField]
    private Image _itemImage = null;
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _descriptionText = null;
    [SerializeField]
    private CanvasGroup _tooltipVisibleMask = null;

    private void Start()
    {
        EventManager.OnItemHovered += PopulateTooltip;
        EventManager.OnItemExitHover += DisableTooltip;
    }

    private void OnDestroy()
    {
        EventManager.OnItemHovered -= PopulateTooltip;
        EventManager.OnItemExitHover -= DisableTooltip;
    }

    private void OnApplicationQuit()
    {
        EventManager.OnItemHovered -= PopulateTooltip;
        EventManager.OnItemExitHover -= DisableTooltip;
    }

    public void PopulateTooltip(ItemSlot newItemSlot)
    {
        Item heldItem = newItemSlot.HeldItem;
        _panelImage.color = heldItem.Rarity.RarityColor;
        _itemImage.sprite = heldItem.ItemImage;
        _nameText.text = heldItem.name;
        _descriptionText.text = heldItem.GetItemDescriptionText();

        CanvasScaler canvasScaler = _panelImage.canvas.gameObject?.GetComponent<CanvasScaler>();
        Vector2 resMultiplier;
        if (canvasScaler != null)
        {
            Vector2 referenceResolution = canvasScaler.referenceResolution;
            resMultiplier = canvasScaler.referenceResolution / _panelImage.canvas.renderingDisplaySize;
            float aspectDiff = (referenceResolution.x / referenceResolution.y) / Camera.main.aspect;
            resMultiplier = new Vector2(resMultiplier.x, resMultiplier.y * aspectDiff);
        }
        else
        {
            Debug.LogError("Root canvas is missing scaler");
            return;
        }
        RectTransform targetRect = newItemSlot.GetRectTransform;
        // Don't feel like doing the conversion here so setting it again after getting transform screen position
        _panelImage.rectTransform.anchoredPosition = (targetRect.position * resMultiplier)
         + new Vector2(-targetRect.rect.width * 0.5f, targetRect.rect.height * 0.5f);

        Debug.Log(_panelImage.transform.position);
        Vector2 convertedPosition = _panelImage.transform.position;
        Vector2 finalOffset = Vector2.zero;
        if (convertedPosition.x > Camera.main.pixelWidth - _panelImage.rectTransform.rect.width)
        {
            finalOffset.x = -_panelImage.rectTransform.rect.width;
        }

        if (convertedPosition.y > Camera.main.pixelHeight - _panelImage.rectTransform.rect.height)
        {
            finalOffset.y = -_panelImage.rectTransform.rect.height;
        }

        _panelImage.rectTransform.anchoredPosition += finalOffset;

        _tooltipVisibleMask.alpha = 1;
    }

    private void DisableTooltip()
    {
        _panelImage.rectTransform.anchoredPosition = Vector2.positiveInfinity;
        _tooltipVisibleMask.alpha = 0;
    }
}
