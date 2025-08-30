using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private CanvasGroup _mainCanvasGroup = null;
    [SerializeField]
    private CanvasGroup _settingsGroup = null;

    private void OnEnable()
    {
        EventManager.OnPausedGame += BecomeVisible;
        EventManager.OnResumedGame += CloseMenu;
    }

    private void OnDisable()
    {
        EventManager.OnPausedGame -= BecomeVisible;
        EventManager.OnResumedGame -= CloseMenu;
    }

    private void BecomeVisible()
    {
        _mainCanvasGroup.alpha = 1;
        _mainCanvasGroup.interactable = true;
        _mainCanvasGroup.blocksRaycasts = true;
    }
    private void CloseMenu()
    {
        _mainCanvasGroup.alpha = 0;
        _mainCanvasGroup.interactable = false;
        _mainCanvasGroup.blocksRaycasts = false;
    }

    public void OpenSettings()
    {
        CloseMenu();
        _settingsGroup.alpha = 1;
        _settingsGroup.interactable = true;
        _settingsGroup.blocksRaycasts = true;
    }

    public void CloseSettings()
    {
        _settingsGroup.alpha = 0;
        _settingsGroup.interactable = false;
        _settingsGroup.blocksRaycasts = false;
        BecomeVisible();
    }

    public void ResumeGame()
    {
        EventManager.OnResumedGame?.Invoke();
        CloseMenu();
    }

    public void RestartRun()
    {
        EventManager.OnRestartGame?.Invoke();
    }

    public void BackToMainMenu()
    {
        GameManager.LoadMenuScene();
    }
}
