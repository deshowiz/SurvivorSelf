using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("InitialStats")]
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _maximumHealth = 1f;
    private float _currentHealth = 0f;

    private Player _player = null;

    public void Initialize(Player newPlayer)
    {
        _maximumHealth = _currentHealth;
        this._player = newPlayer;
    }

    private void Update()
    {
        if (_player != null)
        {
            Vector2 travelVector = (_player.transform.position - transform.position).normalized;
            // direction = travelVector.normalized;

            transform.position += new Vector3(travelVector.x, travelVector.y, 0f) * (_speed * Time.deltaTime);
        }
        else
        {
            Debug.LogError("Enemy can't see player");
        }
    }

}
