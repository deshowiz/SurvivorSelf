using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField]
    private float _baseMaximumHealth = 10f;
    private float _baseCurrentHealth;
    [SerializeField]
    private float _speedMultiplier = 1f;

    [Header("Settings")]
    [SerializeField]
    private float _invulnTimer = 0.5f;

    private Vector2 _currentMovementVector = Vector2.zero;
    private float _lastTimeHit = -Mathf.Infinity;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _baseCurrentHealth = _baseMaximumHealth;
    }

    void Update()
    {
        transform.Translate(_currentMovementVector * _speedMultiplier * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        _currentMovementVector = value.Get<Vector2>();
    }

    public void Damage(float incomingDamage, Vector2 incomingKnockback)
    {
        if (Time.time - _lastTimeHit < _invulnTimer)
        {
            return;
        }
        _lastTimeHit = Time.time;

        _baseCurrentHealth -= incomingDamage;
        if (_baseCurrentHealth <= 0f)
        {
            Die();
            return;
        }
        //Push(incomingKnockback);
    }

    public void Die()
    {
        Debug.Log("Dead");
        //Destroy(gameObject);
        Time.timeScale = 0f;
    }
}
