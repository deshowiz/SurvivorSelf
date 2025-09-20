using Unity.Mathematics;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Reference")]
    [SerializeField]
    private Rigidbody2D _rigidbody = null;
    [SerializeField]
    private Interactable _droppedInteractable = null;
    [Header("InitialStats")]
    [SerializeField]
    private float _baseSpeed = 1f;
    [SerializeField]
    private float _baseDamage = 1f;
    [SerializeField]
    private float _baseMaximumHealth = 1f;
    private float _baseCurrentHealth = 0f;

    public int _enemyId = 0;
    private Player _player = null;

    public void Initialize(Player newPlayer)
    {
        _baseCurrentHealth = _baseMaximumHealth;
        this._player = newPlayer;
        Vector2 travelVector = (_player.transform.position - transform.position).normalized;
        _rigidbody.linearVelocity = travelVector * _baseSpeed;
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            Vector2 travelVector = (_player.transform.position - transform.position).normalized;
            _rigidbody.linearVelocity += travelVector * (_baseSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogError("Enemy can't see player");
        }
    }

    public void Damage(float incomingDamage, Vector2 incomingKnockback)
    {
        _baseCurrentHealth -= incomingDamage;
        if (_baseCurrentHealth <= 0f)
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

    public void Die()
    {
        EventManager.OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(_baseDamage, Vector2.zero);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(_baseDamage, Vector2.zero);
        }
    }

}
