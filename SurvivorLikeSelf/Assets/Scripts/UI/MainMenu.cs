using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup[] _menuGroups = null;

    // Setting up 1 function for a generalized call cause why not
    public void NewMenu(int menuIndex)
    {
        for (int i = 0; i < _menuGroups.Length; i++)
        {
            if (menuIndex == i)
            {
                _menuGroups[i].alpha = 1f;
                _menuGroups[i].interactable = true;
                _menuGroups[i].blocksRaycasts = true;
            }
            else
            {
                _menuGroups[i].alpha = 0f;
                _menuGroups[i].interactable = false;
                _menuGroups[i].blocksRaycasts = false;
            }
        }
    }

    public void QuitApplication()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
