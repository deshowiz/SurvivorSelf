using UnityEngine;

[CreateAssetMenu(fileName = "Rarity", menuName = "Scriptable Objects/Rarity")]
public class Rarity : ScriptableObject
{
    [SerializeField]
    private Color _rarityColor = Color.gray;
    public Color RarityColor {get { return _rarityColor; }}
}
