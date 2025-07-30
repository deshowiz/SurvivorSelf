using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    [Header("Wave Enemies")]
    [SerializeField]
    private Enemy[] _enemyData = null;
    public Enemy[] EnemyData {get { return _enemyData; }}

}
