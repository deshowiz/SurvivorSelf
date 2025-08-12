using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Scriptable Objects/WeaponItem")]
public class WeaponItem : Item
{
    [Header("Weapon")]
    [SerializeField]
    private Weapon _weaponPrefab = null;
    public Weapon WeaponPrefab { get { return _weaponPrefab; } }

    public override string GetItemDescriptionText()
    {
        string descriptionAdditive = base.GetItemDescriptionText();

        if (_weaponPrefab is MeleeWeapon)
        {
            MeleeWeapon meleeWeapon = (MeleeWeapon)_weaponPrefab;
            descriptionAdditive += meleeWeapon.BaseDamage.ToString() + " damage";
        }
        else
        {
            RangedWeapon rangedWeapon = (RangedWeapon)_weaponPrefab;
            descriptionAdditive += rangedWeapon.Projectile.BaseDamage.ToString() + " damage";
        }

        return descriptionAdditive;
        //return base.GetItemDescriptionText();
    }
}
