using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private CharacterDetailPanel _characterDetailPanel = null;
    [SerializeField]
    private PlayerAttributesContainer _currentlySelectedCharacter;

    public void SwitchHoveredCharacter(PlayerAttributesContainer newCharacterSelection)
    {
        _currentlySelectedCharacter = newCharacterSelection;
        _characterDetailPanel.PopulateDetails(_currentlySelectedCharacter);
    }

    public void SelectedCharacter(PlayerAttributesContainer newCharacterSelection)
    {
        _currentlySelectedCharacter = newCharacterSelection;
        EventManager.OnSelectedPlayer?.Invoke(_currentlySelectedCharacter);
    }
}
