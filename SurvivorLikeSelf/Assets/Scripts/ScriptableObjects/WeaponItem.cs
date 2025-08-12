using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Scriptable Objects/WeaponItem")]
public class WeaponItem : Item
{
    [Header("Weapon")]
    [SerializeField]
    private Weapon _weaponPrefab = null;
    public Weapon WeaponPrefab { get { return _weaponPrefab; } }

    public override string GetItemDescriptionText(bool fullDescription = true)
    {
        string descriptionAdditive = "";
        if (fullDescription)
        {
            descriptionAdditive = _itemDescriptionText;
        }

        if (!String.IsNullOrEmpty(descriptionAdditive))
        {
            descriptionAdditive += "\n";
        }

        if (_weaponPrefab is MeleeWeapon)
        {
            MeleeWeapon meleeWeapon = (MeleeWeapon)_weaponPrefab;
            descriptionAdditive += meleeWeapon.BaseDamage.ToString() + " damage\n";
        }
        else
        {
            RangedWeapon rangedWeapon = (RangedWeapon)_weaponPrefab;
            descriptionAdditive += rangedWeapon.Projectile.BaseDamage.ToString() + " damage\n";
        }

        return descriptionAdditive + base.GetItemDescriptionText(false);
    }
}
