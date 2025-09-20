using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    [Header("Settings")]
    [SerializeField]
    private int _waveLengthSeconds = 30;
    public int WaveLengthSeconds { get { return _waveLengthSeconds; } }
    [Header("Wave Enemies")]
    [SerializeField]
    private SubWaveData[] enemyData = null;
    public SubWaveData[] EnemyData { get { return enemyData; } }

    public int GetSubWaveCount { get { return enemyData.Length; } }

    public enum SpawnType { Random, Box }

    public SubWaveData GetNextSubWave(int subWaveIndex)
    {
        return enemyData[subWaveIndex];
    }

    public List<Vector2[]> GetFullWavePositions(float xHalf, float yHalf)
    {
        List<Vector2[]> positions = new List<Vector2[]>();
        for (int i = 0; i < enemyData.Length; i++)
        {
            positions.Add(enemyData[i].GetSubWavePositions(xHalf, yHalf));
        }

        return positions;
    }


    [Serializable]
    public class SubWaveData
    {
        public float _triggerDelay; // additive from last sub wave
        [SerializeField]
        private SpawnType _spawnType;
        [SerializeField]
        private Vector2 _boxHalfSizeXY;
        [SerializeField]
        private Enemy[] _enemyMakeup;
        public Enemy[] EnemyMakeup { get { return _enemyMakeup; }}

        public SubWaveData()
        {
            _triggerDelay = 4f;
            _spawnType = SpawnType.Random;
            _boxHalfSizeXY = new Vector2(0.5f, 0.5f);
        }
        

        // Calc is done pre wave now
        public Vector2[] GetSubWavePositions(float xHalf, float yHalf)
        {
            Vector2[] generatedPositions = new Vector2[_enemyMakeup.Length];
            switch (_spawnType) // use switch for future expansion instead of if else
            {
                case SpawnType.Random:
                    for (int i = 0; i < generatedPositions.Length; i++)
                    {
                        generatedPositions[i] = new Vector2(UnityEngine.Random.Range(-xHalf, xHalf),
                        UnityEngine.Random.Range(-yHalf, yHalf));
                    }
                    break;
                case SpawnType.Box:
                        float xCenter = UnityEngine.Random.Range(-xHalf + _boxHalfSizeXY.x, xHalf - _boxHalfSizeXY.x);
                    float yCenter = UnityEngine.Random.Range(-yHalf + _boxHalfSizeXY.y, yHalf - _boxHalfSizeXY.y);
                    for (int i = 0; i < generatedPositions.Length; i++)
                    {
                        generatedPositions[i] =
                        new Vector2(xCenter + UnityEngine.Random.Range(-_boxHalfSizeXY.x, _boxHalfSizeXY.x),
                        yCenter + UnityEngine.Random.Range(-_boxHalfSizeXY.y, _boxHalfSizeXY.y));
                    }
                    break;
            }

            return generatedPositions;
        }
    }
}
