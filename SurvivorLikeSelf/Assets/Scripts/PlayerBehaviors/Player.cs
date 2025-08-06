using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField]
    private SpriteRenderer _playerSprite = null;
    [Header("Stats")]
    [SerializeField]
    private float _baseMaximumHealth = 10f;
    private float _baseCurrentHealth;
    [SerializeField]
    private float _speedMultiplier = 1f;

    [Header("Settings")]
    [SerializeField]
    private float _invulnTimer = 0.5f;
    [SerializeField]
    [Tooltip("Flash duration should be lower than invuln timer")]
    private float _flashDuration = 0.1f;

    private Vector2 _currentMovementVector = Vector2.zero;
    private float _lastTimeHit = -Mathf.Infinity;

    private Coroutine _flashRoutine = null;

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
        DamageFlash();
        //Push(incomingKnockback);
    }

    private void DamageFlash()
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }
        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float startTime = Time.time;
        float progress = 0f;

        while (Time.time - startTime < _flashDuration)
        {
            progress += Time.deltaTime;
            _playerSprite.color = Color.Lerp(Color.white, Color.red, progress / _flashDuration);
            yield return null;
        }

        startTime = Time.time;
        progress = 0f;

        while (Time.time - startTime < _flashDuration)
        {
            progress += Time.deltaTime;
            _playerSprite.color = Color.Lerp(Color.red, Color.white, progress / _flashDuration);
            yield return null;
        }

        _playerSprite.color = Color.white;
    }

    public void Die()
    {
        EventManager.TriggerEvent("gameover", null);
    }
}
