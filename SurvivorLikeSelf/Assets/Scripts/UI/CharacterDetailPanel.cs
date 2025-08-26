using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailPanel : MonoBehaviour
{
    [SerializeField]
    private Image _characterImage = null;
    [SerializeField]
    private TextMeshProUGUI _nametext = null;
    [SerializeField]
    private TextMeshProUGUI _descriptionText = null;

    public void PopulateDetails(PlayerAttributesContainer newPlayerCharacter)
    {
        _characterImage.sprite = newPlayerCharacter.CharacterSprite;
        _nametext.text = newPlayerCharacter.name;
        _descriptionText.text = newPlayerCharacter.CharacterDescription;
    }
}
