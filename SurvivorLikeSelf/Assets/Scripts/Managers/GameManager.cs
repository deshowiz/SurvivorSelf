using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("References")]
    [SerializeField]
    private EnemyWave[] _enemyWaves;

    private int _currentWaveIndex = 0;

    private PlayerAttributesContainer _chosenCharacter = null;

    public static bool IsInGameplayScene = false;

    CancellationToken _waveTimerCancellation = new CancellationToken();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        EventManager.OnStartWave += StartNewWave;
        EventManager.OnDeath += Restart;
        EventManager.OnRestartGame += Restart;
        EventManager.OnSelectedPlayer += LoadGameplayScene;
        EventManager.OnFullInitialization += SetPlayerCharacter;
        EventManager.OnFullInitialization += StartNewWave;
        
    }

    void OnDisable()
    {
        EventManager.OnStartWave -= StartNewWave;
        EventManager.OnDeath -= Restart;
        EventManager.OnRestartGame -= Restart;
        EventManager.OnSelectedPlayer -= LoadGameplayScene;
        EventManager.OnFullInitialization -= SetPlayerCharacter;
        EventManager.OnFullInitialization -= StartNewWave;
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        _currentWaveIndex = 0;
        LoadGameplayScene(_chosenCharacter);
    }

    private void SetPlayerCharacter()
    {
        EventManager.OnSetPlayerCharacter?.Invoke(_chosenCharacter);
        _waveTimerCancellation = SpawnManager.Instance.GetCancellationTokenOnDestroy();
    }

    private void StartNewWave()
    {
        SpawnManager.Instance.SetSpawnWave(_enemyWaves[_currentWaveIndex]);
        WaveTimer().Forget();
    }

    private async UniTaskVoid WaveTimer()
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
            await UniTask.Yield(_waveTimerCancellation);
        }
        // Wave Over
        EventManager.OnWaveTimerZero?.Invoke();
        _currentWaveIndex++;
    }

    private void LoadGameplayScene(PlayerAttributesContainer chosenCharacter)
    {
        this._chosenCharacter = chosenCharacter;
        LoadGameplaySceneTask().Forget();
    }

    private async UniTaskVoid LoadGameplaySceneTask()
    {
        await SceneManager.LoadSceneAsync(1);
        IsInGameplayScene = true;
    }

    public static void LoadMenuScene()
    {
        Time.timeScale = 1f;
        IsInGameplayScene = false;
        SceneManager.LoadScene(0);
    }
}
