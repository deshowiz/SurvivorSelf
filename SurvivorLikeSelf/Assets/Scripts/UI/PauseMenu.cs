using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    private void OnEnable()
    {
        EventManager.OnPausedGame += BecomeVisibile;
        EventManager.OnResumedGame += CloseMenu;
    }

    private void OnDisable()
    {
        EventManager.OnPausedGame -= BecomeVisibile;
        EventManager.OnResumedGame -= CloseMenu;
    }

    private void BecomeVisibile()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }
    private void CloseMenu()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void ResumeGame()
    {
        EventManager.OnResumedGame?.Invoke();
        CloseMenu();
    }
}
