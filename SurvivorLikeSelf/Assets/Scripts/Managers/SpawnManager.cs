using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("References")]
    [SerializeField]
    private Player _player = null;
    [SerializeField]
    private Transform _interactablesHolderTransform = null;
    [SerializeField]
    private Transform _enemiesHolderTransform = null;
    [SerializeField]
    private Transform _visualsHolderTransform = null;
    [SerializeField]
    private EnemyWave _currentWave = null;

    [Header("Settings")]
    [SerializeField]
    private float _endWaveCollectionDelay = 2f;
    [SerializeField]
    private float _enemySpawnDelay = 2f;
    [SerializeField]
    private List<InteractableSpawnData> _interactablesSpawnData = new List<InteractableSpawnData>();
    [SerializeField]
    private List<EnemySpawnData> _enemiesSpawnData = new List<EnemySpawnData>();
    [SerializeField]
    private VisualSpawnData _visualSpawnData;

    [Serializable]
    private struct InteractableSpawnData
    {
        public int numToSpawn;
        public Interactable prefabToSpawn;
    }

    [Serializable]
    private struct EnemySpawnData
    {
        public int numToSpawn;
        public Enemy prefabToSpawn;
    }

    [Serializable]
    private struct VisualSpawnData
    {
        public int numToSpawn;
        public GameObject prefabToSpawn;
    }

    private Queue<Interactable> _interactableQueue = new Queue<Interactable>();
    private List<Queue<Enemy>> _enemyQueues = new List<Queue<Enemy>>();
    private Queue<GameObject> _visualQueue = new Queue<GameObject>();

    private float _xAxisSpawnSize = 2.5f;
    private float _yAxisSpawnSize = 1.40625f;

    private List<Enemy> _enemiesToBeCleared = new List<Enemy>();

    private int _numInteractablesEnabled = 0;

    private List<Vector2[]> _subWavePositionData;
    private CancellationToken _taskCancellationToken;

    private void Awake()
    {
        Instance = this;
        _player = FindFirstObjectByType<Player>();
        if (_player == null)
        {
            Debug.LogError("Player can't be found in scene!");
            return;
        }
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks); // Simple initialization for randomness works for now
        // References docs: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Camera-orthographicSize.html
        _yAxisSpawnSize = Camera.main.orthographicSize/* * 2f*/;
        _xAxisSpawnSize = _yAxisSpawnSize * Camera.main.aspect;
        _taskCancellationToken = this.GetCancellationTokenOnDestroy();
    }

    void Start()
    {
        InitializeSpawns();
    }

    void OnEnable()
    {
        EventManager.OnWaveTimerZero += ClearWave;
        EventManager.OnEnemyDeath += SpawnInteractable;
        EventManager.OnEnemyDeath += PoolEnemy;
        EventManager.OnPickedUpInteractable += PoolInteractable;
    }

    void OnDisable()
    {
        EventManager.OnWaveTimerZero -= ClearWave;
        EventManager.OnEnemyDeath -= SpawnInteractable;
        EventManager.OnEnemyDeath -= PoolEnemy;
        EventManager.OnPickedUpInteractable -= PoolInteractable;
    }

    private void InitializeSpawns()
    {
        _interactableQueue.Clear();
        for (int i = 0; i < _interactablesSpawnData.Count; i++)
        {
            Interactable currentInteractableToSpawn = _interactablesSpawnData[i].prefabToSpawn;
            for (int j = 0; j < _interactablesSpawnData[i].numToSpawn; j++)
            {
                _interactableQueue.Enqueue(Instantiate(currentInteractableToSpawn, _interactablesHolderTransform));
            }
        }
        _enemyQueues.Clear();
        for (int i = 0; i < _enemiesSpawnData.Count; i++)
        {
            _enemyQueues.Add(new Queue<Enemy>());
            Enemy currentEnemyTospawn = _enemiesSpawnData[i].prefabToSpawn;
            for (int j = 0; j < _enemiesSpawnData[i].numToSpawn; j++)
            {
                Enemy currentEnemy = Instantiate(currentEnemyTospawn, _enemiesHolderTransform);
                _enemyQueues[i].Enqueue(currentEnemy);
            }
        }
        _visualQueue.Clear();
        GameObject visualToSpawn = _visualSpawnData.prefabToSpawn;
        for (int i = 0; i < _visualSpawnData.numToSpawn; i++)
        {
            GameObject currentVisual = Instantiate(visualToSpawn, _visualsHolderTransform);
            _visualQueue.Enqueue(currentVisual);
        }

        EventManager.OnFullInitialization?.Invoke();
    }

    private void SpawnInteractable(Enemy deadEnemy)
    {
        Interactable spawnedInteractable = _interactableQueue.Dequeue();
        spawnedInteractable.transform.position = deadEnemy.transform.position;
        spawnedInteractable.gameObject.SetActive(true);
        _numInteractablesEnabled++;
    }

    private void PoolInteractable(Interactable pickup)
    {
        pickup.gameObject.SetActive(false);
        _interactableQueue.Enqueue(pickup);
        _numInteractablesEnabled--;
    }

    private void PoolEnemy(Enemy enemyToPool)
    {
        enemyToPool.gameObject.SetActive(false);
        _enemyQueues[enemyToPool._enemyId].Enqueue(enemyToPool);
    }

    private void PoolVisual(GameObject visual)
    {
        visual.SetActive(false);
        _visualQueue.Enqueue(visual);
    }

    public void SetSpawnWave(EnemyWave newWave)
    {
        _currentWave = newWave;
        int subWaveCount = _currentWave.GetSubWaveCount;
        _subWavePositionData = _currentWave.GetFullWavePositions(_xAxisSpawnSize, _yAxisSpawnSize);
        WaveSpawnRoutine(_currentWave, subWaveCount).Forget();
    }
    
    // Note that async won't work with timescale things like pausing
    // here's a possible async based replacement
    // https://openupm.com/packages/com.cysharp.unitask/
    private async UniTaskVoid WaveSpawnRoutine(EnemyWave newWave, int numSubWaves)
    {
        int currentSubWave = 0;

        while (currentSubWave < numSubWaves)
        {
            EnemyWave.SubWaveData currentsubWaveData = newWave.GetNextSubWave(currentSubWave);
            SpawnWave(currentsubWaveData, _subWavePositionData[currentSubWave]);
            currentSubWave++;
            await UniTask.Delay((int)(currentsubWaveData._triggerDelay * 1000f), false, PlayerLoopTiming.Update, _taskCancellationToken);
        }
    }

    public void SpawnWave(EnemyWave.SubWaveData newSubWave, Vector2[] positions)
    {
        Enemy[] enemyMakeup = newSubWave.EnemyMakeup; // cache since it is a getter property?
        GameObject[] spawnedVisuals = new GameObject[enemyMakeup.Length];
        for (int i = 0; i < enemyMakeup.Length; i++)
        {
            GameObject spawnedVisual = _visualQueue.Dequeue();
            spawnedVisual.transform.position = positions[i];
            spawnedVisuals[i] = spawnedVisual;
            spawnedVisual.SetActive(true);
        }
        EnemyDelayedSpawns(enemyMakeup, spawnedVisuals, positions).Forget();
    }

    private async UniTaskVoid EnemyDelayedSpawns(Enemy[] enemyMakeup, GameObject[] visuals, Vector2[] positions)
    {
        await UniTask.Delay((int)(_enemySpawnDelay * 1000f), false, PlayerLoopTiming.Update, _taskCancellationToken);

        for (int i = 0; i < enemyMakeup.Length; i++)
        {
            Enemy spawnedEnemy = _enemyQueues[enemyMakeup[i]._enemyId].Dequeue();
            spawnedEnemy.Initialize(_player);
            spawnedEnemy.transform.position = positions[i];

            spawnedEnemy.gameObject.SetActive(true);
            visuals[i].gameObject.SetActive(false);
            _visualQueue.Enqueue(visuals[i]);
        }

    }

    private void ClearWave()
    {
        Enemy[] clearableEnemies = _enemiesHolderTransform.GetComponentsInChildren<Enemy>();

        for (int i = 0; i < clearableEnemies.Length; i++)
        {
            Enemy _currentEnemy = clearableEnemies[i];
            _currentEnemy.gameObject.SetActive(false);
            _enemyQueues[_currentEnemy._enemyId].Enqueue(_currentEnemy);
        }

        CheckForCollection();
    }

    private void CheckForCollection()
    {
        if (_numInteractablesEnabled != 0)
        {
            WaitForCollection().Forget();
        }
        else
        {
            EventManager.OnEndWave?.Invoke();
        }
        _numInteractablesEnabled = 0;
    }

    private async UniTaskVoid WaitForCollection()
    {
        await UniTask.Delay((int)(_endWaveCollectionDelay * 1000f), false, PlayerLoopTiming.Update, _taskCancellationToken);
        EventManager.OnEndWave?.Invoke();
    }
}
