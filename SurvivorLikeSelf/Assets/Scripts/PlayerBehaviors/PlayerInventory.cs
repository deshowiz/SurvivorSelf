using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField]
    private int _gold = 0;

    [Header("Equipped Items")]
    [SerializeField]
    private List<WeaponItem> _equippedWeapons;
    [SerializeField]
    private List<Item> _equippedItems;

    private void OnEnable()
    {
        EventManager.OnPickedUpInteractable += PickedUpInteractable;
        EventManager.OnPurchasedItem += PurchasedItem;
        EventManager.OnItemEquipped += EquippedItem;
        EventManager.OnWeaponEquipped += EquippedWeapon;
    }

    private void OnDisable()
    {
        EventManager.OnPickedUpInteractable -= PickedUpInteractable;
        EventManager.OnPurchasedItem -= PurchasedItem;
        EventManager.OnItemEquipped -= EquippedItem;
        EventManager.OnWeaponEquipped -= EquippedWeapon;
    }

    private void Start()
    {
        _gold = 200;
        EventManager.OnGoldChange?.Invoke(_gold);
    }

    private void PickedUpInteractable(Interactable interactable)
    {
        _gold++;
        EventManager.OnGoldChange?.Invoke(_gold);
    }

    private void PurchasedItem(int itemCost)
    {
        _gold -= itemCost;
        EventManager.OnGoldChange?.Invoke(_gold);
    }

    private void EquippedItem(Item newItem)
    {
        _equippedItems.Add(newItem);
    }

    private void EquippedWeapon(WeaponItem newWeapon)
    {
        _equippedWeapons.Add(newWeapon);
    }
}
