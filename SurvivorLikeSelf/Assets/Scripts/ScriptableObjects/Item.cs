using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("References")]
    [SerializeField]
    private Sprite _itemImage = null;
    public Sprite ItemImage {get { return _itemImage; }}
    
}
