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

    [SerializeField]
    private EnemyWave[] _enemyWaves;

    private int _currentWaveIndex = 0;

    private Coroutine _waveTimerRoutine = null;

    void OnEnable()
    {
        EventManager.StartListening("gameover", Restart);
    }

    void OnDisable()
    {
        EventManager.StopListening("gameover", Restart);
    }

    private void Restart(Dictionary<string, object> message)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        StartNewWave();
    }

    private void StartNewWave()
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
        float timeElapsed = 30f;
        int lastSecondValue = 30;
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
    }
}
