using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    [Header("Settings")]
    [SerializeField]
    private int _waveLengthSeconds = 30;
    public int WaveLengthSeconds {get{return _waveLengthSeconds;}}
    
    [Header("Wave Enemies")]
    [SerializeField]
    private Enemy[] _enemyData = null;
    public Enemy[] EnemyData {get { return _enemyData; }}

}
