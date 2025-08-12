using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("References")]
    [SerializeField]
    private Enemy _testEnemy = null;
    [SerializeField]
    private Player _player = null;
    [SerializeField]
    private Transform _interactablesHolderTransform = null;
    [SerializeField]
    private EnemyWave _currentWave = null;

    [Header("Settings")]
    [SerializeField]
    private List<InteractableSpawnData> _interactablesSpawnData = new List<InteractableSpawnData>();

    [Serializable]
    private struct InteractableSpawnData
    {
        public int numToSpawn;
        public Interactable prefabToSpawn;
    }
    
    private Queue<Interactable> _interactableQueue = new Queue<Interactable>();

    private float _xAxisSpawnSize = 2.5f;
    private float _yAxisSpawnSize = 1.40625f;
    

    private List<Enemy> _aliveEnemyList = new List<Enemy>();
    // Ref is used here to avoid creating copies of the data every time we want to iterate through it
    public ref List<Enemy> AliveEnemyList { get { return ref _aliveEnemyList; } }

    private List<Enemy> _enemiesToBeCleared = new List<Enemy>();

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

        InitializeSpawns();
    }

    void OnEnable()
    {
        EventManager.OnEndWave += ClearWave;
        EventManager.OnEnemyDeath += SpawnInteractable;
        EventManager.OnPickedUpInteractable += PoolInteractable;
    }

    void OnDisable()
    {
        EventManager.OnEndWave -= ClearWave;
        EventManager.OnEnemyDeath -= SpawnInteractable;
        EventManager.OnPickedUpInteractable -= PoolInteractable;
    }

    private void InitializeSpawns()
    {
        for (int i = 0; i < _interactablesSpawnData.Count; i++)
        {
            Interactable currentInteractableToSpawn = _interactablesSpawnData[i].prefabToSpawn;
            for (int j = 0; j < _interactablesSpawnData[i].numToSpawn; j++)
            {
                _interactableQueue.Enqueue(Instantiate(currentInteractableToSpawn, _interactablesHolderTransform));
            }
        }
    }

    private void SpawnInteractable(Vector2 spawnPos)
    {
        Interactable spawnedInteractable = _interactableQueue.Dequeue();
        spawnedInteractable.transform.position = spawnPos;
        spawnedInteractable.gameObject.SetActive(true);
    }

    private void PoolInteractable(Interactable pickup)
    {
        pickup.gameObject.SetActive(false);
        _interactableQueue.Enqueue(pickup);
    }

    public void SpawnWave(EnemyWave newWave)
    {
        _currentWave = newWave;
        if (_testEnemy == null)
        {
            Debug.LogError("Enemy Reference is Empty");
            return;
        }
        Enemy[] enemiesToSpawn = _currentWave.EnemyData;

        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            Enemy newEnemy = Instantiate(
                enemiesToSpawn[i],
                new Vector3(UnityEngine.Random.Range(-_xAxisSpawnSize, _xAxisSpawnSize),
                    UnityEngine.Random.Range(-_yAxisSpawnSize, _yAxisSpawnSize), 0f),
                Quaternion.identity,
                transform
                );
            newEnemy.Initialize(_player, (uint)i);
            _aliveEnemyList.Add(newEnemy);
        }
    }

    private void ClearWave()
    {
        for (int i = 0; i < _aliveEnemyList.Count; i++)
        {
            if (_aliveEnemyList[i] != null)
            {
                Destroy(_aliveEnemyList[i].gameObject);
            }
        }
        _aliveEnemyList.Clear();
    }

    public void ClearEnemy(Enemy deadEnemy) // can later create paralellized int IDs to speed up
    {
        _enemiesToBeCleared.Add(deadEnemy);
    }

    void LateUpdate() // Slow but changing later
    {
        if (_enemiesToBeCleared.Count != 0)
        {
            for (int i = 0; i < _enemiesToBeCleared.Count; i++)
            {
                _aliveEnemyList.Remove(_enemiesToBeCleared[i]);
            }
        }
    }
}
