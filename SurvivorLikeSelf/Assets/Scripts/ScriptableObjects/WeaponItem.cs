using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Scriptable Objects/WeaponItem")]
public class WeaponItem : Item
{
    [SerializeField]
    private Weapon _weaponPrefab = null;
    public Weapon WeaponPrefab { get { return _weaponPrefab;}}
}
