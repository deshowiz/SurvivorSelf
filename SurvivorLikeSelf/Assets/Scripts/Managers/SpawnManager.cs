using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _testEnemy = null;

    [Header("Settings")]
    [SerializeField]
    private int _enemiesToSpawn = 10;
    [SerializeField]
    private float _xAxisSpawnSize = 2.5f;
    [SerializeField]
    private float _yAxisSpawnSize = 1.40625f;

    private void Start()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks); // Simple initialization for randomness
        // References docs: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Camera-orthographicSize.html
        _yAxisSpawnSize = Camera.main.orthographicSize/* * 2f*/;
        _xAxisSpawnSize = _yAxisSpawnSize * Camera.main.aspect;
        SpawnWave();
    }

    public void SpawnWave()
    {
        if (_testEnemy == null)
        {
            Debug.LogError("Creature Reference is Empty");
            return;
        }

        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            GameObject newEnemy = Instantiate(
                _testEnemy,
                new Vector3(UnityEngine.Random.Range(-_xAxisSpawnSize, _xAxisSpawnSize),
                    UnityEngine.Random.Range(-_yAxisSpawnSize, _yAxisSpawnSize), 0f),
                Quaternion.identity,
                transform
                );
        }
    }
}
