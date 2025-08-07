using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static EventManager gameManager;
    public static EventManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindFirstObjectByType<EventManager>();

                if (!gameManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {

                    //  Sets this to not be destroyed when reloading scene
                    DontDestroyOnLoad(gameManager);
                }
            }
            return gameManager;
        }
    }
    [Header("References")]
    [SerializeField]
    private EnemyWave[] _enemyWaves;
    [SerializeField]
    private ItemList _fullItemList = null;

    private int _currentWaveIndex = 0;

    private Coroutine _waveTimerRoutine = null;

    void OnEnable()
    {
        EventManager.StartListening("StartWave", StartNewWave);
        EventManager.StartListening("gameover", Restart);
    }

    void OnDisable()
    {
        EventManager.StopListening("StartWave", StartNewWave);
        EventManager.StopListening("gameover", Restart);
    }

    private void Restart(Dictionary<string, object> message)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        StartNewWave(null);
    }

    private void StartNewWave(Dictionary<string, object> message)
    {
        SpawnManager.Instance.SpawnWave(_enemyWaves[_currentWaveIndex]);
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
        EventManager.TriggerEvent("SetSecondDisplay", new Dictionary<string, object> { { "secondValue", waveTime } });
        float timeElapsed = waveTime;
        int lastSecondValue = waveTime;
        while (timeElapsed > 0)
        {
            timeElapsed -= Time.deltaTime;
            if (Mathf.CeilToInt(timeElapsed) < lastSecondValue)
            {
                lastSecondValue--;
                EventManager.TriggerEvent("SetSecondDisplay", new Dictionary<string, object> { { "secondValue", lastSecondValue } });
            }
            yield return null;
        }
        // Wave Over
        _currentWaveIndex++;
        EventManager.TriggerEvent("WaveEnd", new Dictionary<string, object>() { { "rolledItems", _fullItemList.RollNextItems(1) } });
    }
}
