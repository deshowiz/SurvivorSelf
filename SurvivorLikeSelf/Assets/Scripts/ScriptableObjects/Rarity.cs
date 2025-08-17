using UnityEngine;

[CreateAssetMenu(fileName = "Rarity", menuName = "Scriptable Objects/Rarity")]
public class Rarity : ScriptableObject
{
    [SerializeField]
    private Color _rarityColor = Color.gray;
    public Color RarityColor { get { return _rarityColor; } }
    [SerializeField]
    [Range(0f, 400f)]
    private int _priceRangeMinimum = 0;
    [SerializeField]
    [Range(0f, 400f)]
    private int _priceRangeMaximum = 100;

    public int NewPrice{get{ return Random.Range(_priceRangeMinimum, _priceRangeMaximum); }}
}
