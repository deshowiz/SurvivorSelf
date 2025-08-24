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

    public int GetSubWaveCount {get { return enemyData.Length; } }

    public SubWaveData GetNextSubWave(int subWaveIndex)
    {
        return enemyData[subWaveIndex];
    }
    [Serializable]
    public struct SubWaveData
    {
        public float _triggerDelay; // additive from last sub wave
        public Enemy[] _enemyMakeup;
    }

}
