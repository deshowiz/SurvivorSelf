using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private Rigidbody2D _rigidbody = null;
    [Header("InitialStats")]
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _maximumHealth = 1f;
    private float _currentHealth = 0f;

    private Player _player = null;

    public void Initialize(Player newPlayer)
    {
        _currentHealth = _maximumHealth;
        this._player = newPlayer;
        Vector2 travelVector = (_player.transform.position - transform.position).normalized;
        _rigidbody.linearVelocity = travelVector * _speed;
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            Vector2 travelVector = (_player.transform.position - transform.position).normalized;
            _rigidbody.linearVelocity += travelVector * (_speed * Time.fixedDeltaTime);

            //transform.position += new Vector3(travelVector.x, travelVector.y, 0f) * (_speed * Time.fixedDeltaTime);

        }
        else
        {
            Debug.LogError("Enemy can't see player");
        }
    }

    public void Damage(float incomingDamage, Vector2 incomingKnockback)
    {
        _currentHealth -= incomingDamage;
        if (_currentHealth <= 0f)
        {
            Die();
            return;
        }
        Push(incomingKnockback);
    }

    private void Push(Vector2 incomingKnockback)
    {
        _rigidbody.AddForce(incomingKnockback);
    }

    private void Die()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
