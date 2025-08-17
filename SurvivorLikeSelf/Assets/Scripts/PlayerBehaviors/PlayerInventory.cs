using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int _gold = 0;

    private void OnEnable()
    {
        EventManager.OnPickedUpInteractable += PickedUpInteractable;
        EventManager.OnPurchasedItem += PurchasedItem;
        
    }

    private void OnDisable()
    {
        EventManager.OnPickedUpInteractable -= PickedUpInteractable;
        EventManager.OnPurchasedItem -= PurchasedItem;
    }

    // private void Start()
    // {
    //     _gold = 100;
    //     EventManager.OnGoldChange?.Invoke(_gold);
    // }

    private void PickedUpInteractable(Interactable interactable)
    {
        // _gold += interactable.goldValue
        _gold++;
        EventManager.OnGoldChange?.Invoke(_gold);
    }

    private void PurchasedItem(int itemCost)
    {
        _gold -= itemCost;
        EventManager.OnGoldChange?.Invoke(_gold);
    }
}
