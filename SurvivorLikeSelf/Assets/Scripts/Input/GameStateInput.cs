using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateInput : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _inputActionsAsset = null;
    private InputAction _pauseAction;
    private bool _isPaused = false;

    private void OnEnable()
    {
        EventManager.OnResumedGame += Resume;
    }

    private void OnDisable()
    {
        EventManager.OnResumedGame -= Resume;
    }

    private void Awake()
    {
        _pauseAction = InputSystem.actions.FindAction("Pause");
    }


    private void Update()
    {
        if (_pauseAction.WasPressedThisFrame())
        {
            if (!GameManager.IsInGameplayScene) return;

            _isPaused = !_isPaused;
            if (_isPaused)
            {
                Time.timeScale = 0f;
                EventManager.OnPausedGame?.Invoke();
            }
            else
            {
                Time.timeScale = 1f;
                EventManager.OnResumedGame?.Invoke();
            }
        }
    }

    private void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
    }

}
