using UnityEngine;

public class StatDisplayPanel : MonoBehaviour
{
    [Header("Stats Container")]
    [SerializeField]
    private PlayerAttributesContainer _playerAttributesContainer;
    [Header("Text References")]
    [SerializeField]
    private AttributeText _maxHPText = null;
    [SerializeField]
    private AttributeText _speedText = null;

    private void OnEnable()
    {
        PopulateStatTexts();
        EventManager.OnItemEquipped += PopulateStatTexts;
        EventManager.OnWeaponEquipped += PopulateStatTexts;
    }

    private void OnDisable()
    {
        EventManager.OnItemEquipped -= PopulateStatTexts;
        EventManager.OnWeaponEquipped -= PopulateStatTexts;
    }

    private void PopulateStatTexts(Item sentItem = null)
    {
        _maxHPText.PopulateText((int)_playerAttributesContainer._maxHP.Value);
        _speedText.PopulateText((int)_playerAttributesContainer._speed.Value);
    }
}
