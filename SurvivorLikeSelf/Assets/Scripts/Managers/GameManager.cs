using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private EnemyWave[] _enemyWaves;

    private int _currentWaveIndex = 0;

    private Coroutine _waveTimerRoutine = null;

    private PlayerAttributesContainer _chosenCharacter = null;

    public static bool IsInGameplayScene = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        EventManager.OnStartWave += StartNewWave;
        EventManager.OnDeath += Restart;
        EventManager.OnSelectedPlayer += LoadGameplayScene;
        EventManager.OnFullInitialization += SetPlayerCharacter;
        EventManager.OnFullInitialization += StartNewWave;
    }

    void OnDisable()
    {
        EventManager.OnStartWave -= StartNewWave;
        EventManager.OnDeath -= Restart;
        EventManager.OnSelectedPlayer -= LoadGameplayScene;
        EventManager.OnFullInitialization -= SetPlayerCharacter;
        EventManager.OnFullInitialization -= StartNewWave;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        //StartNewWave();
    }

    private void SetPlayerCharacter()
    {
        EventManager.OnSetPlayerCharacter?.Invoke(_chosenCharacter);
    }

    private void StartNewWave()
    {
        // Write a check for if in shop to return on this function call, for when loading back into a save
        // if (_currentWaveIndex == 0)
        // {
        //     EventManager.OnFullInitialization?.Invoke();
        // }
        SpawnManager.Instance.SetSpawnWave(_enemyWaves[_currentWaveIndex]);
        if (_waveTimerRoutine != null)
        {
            StopCoroutine(_waveTimerRoutine);
            _waveTimerRoutine = null;
        }
        _waveTimerRoutine = StartCoroutine(WaveTimer());
    }

    private IEnumerator WaveTimer()
    {
        int waveTime = _enemyWaves[_currentWaveIndex].WaveLengthSeconds;
        EventManager.OnTimerChange?.Invoke(waveTime);
        float timeElapsed = waveTime;
        int lastSecondValue = waveTime;
        while (timeElapsed > 0)
        {
            timeElapsed -= Time.deltaTime;
            if (Mathf.CeilToInt(timeElapsed) < lastSecondValue)
            {
                lastSecondValue--;
                EventManager.OnTimerChange?.Invoke(lastSecondValue);
            }
            yield return null;
        }
        // Wave Over
        EventManager.OnWaveTimerZero?.Invoke();
        _currentWaveIndex++;
    }

    private void LoadGameplayScene(PlayerAttributesContainer chosenCharacter)
    {
        this._chosenCharacter = chosenCharacter;
        SceneManager.LoadScene(1);
        IsInGameplayScene = true;
    }

    private void LoadMenuScene()
    {
        IsInGameplayScene = false;
        SceneManager.LoadScene(1);
    }
}
