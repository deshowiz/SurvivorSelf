using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _testEnemy = null;
    [SerializeField]
    private int _enemiesToSpawn = 10;

    private void Start()
    {
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
            for (int j = 0; j < _enemiesToSpawn; j++)
            {
                GameObject newEnemy = Instantiate(_testEnemy, new Vector3(i, j, 0f), Quaternion.identity, transform);
            }
        }
    }
}
