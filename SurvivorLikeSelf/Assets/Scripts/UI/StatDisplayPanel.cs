using UnityEngine;

public class StatDisplayPanel : MonoBehaviour
{
    [Header("Canvas Group Reference")]
    [SerializeField]
    private CanvasGroup _alphaCheckGroup = null;
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
        EventManager.OnSetPlayerCharacter += SetCharacterAttributes;
        EventManager.OnEndWave += PopulateStatTexts;
        EventManager.OnAttributeChange += PopulateStatTexts;
    }

    private void OnDisable()
    {
        EventManager.OnSetPlayerCharacter -= SetCharacterAttributes;
        EventManager.OnEndWave -= PopulateStatTexts;
        EventManager.OnAttributeChange -= PopulateStatTexts;
    }

    private void SetCharacterAttributes(PlayerAttributesContainer newPlayerAttributes)
    {
        this._playerAttributesContainer = newPlayerAttributes;
        PopulateStatTexts(null);
    }

    private void PopulateStatTexts(Item sentItem = null)
    {
        if (_alphaCheckGroup.alpha == 0) return;

        _maxHPText.PopulateText((int)_playerAttributesContainer._maxHP.Value);
        _speedText.PopulateText((int)_playerAttributesContainer._speed.Value);
    }

    private void PopulateStatTexts()
    {
        PopulateStatTexts(null);
    }
}
