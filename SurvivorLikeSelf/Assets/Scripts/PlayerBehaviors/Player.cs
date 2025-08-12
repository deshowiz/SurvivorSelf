using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField]
    private SpriteRenderer _playerSprite = null;
    
    [Header("Stats")]
    [SerializeField]
    public AttributeStat _maxHP = new AttributeStat(0);
    private float _currentHealth;
    [SerializeField]
    public AttributeStat _speed = new AttributeStat(0);
    

    [Header("Settings")]
    [SerializeField]
    private float _invulnTimer = 0.5f;
    [SerializeField]
    [Tooltip("Flash duration should be lower than invuln timer")]
    private float _flashDuration = 0.1f;

    private Vector2 _currentMovementVector = Vector2.zero;
    private bool _isPlaying = false;
    private float _lastTimeHit = -Mathf.Infinity;

    private Coroutine _flashRoutine = null;

    // Point for later, use structs as stat modifiers for scaling enemies later per wave

    void Start()
    {
        Initialize();
    }

    void OnEnable()
    {
        EventManager.OnStartWave += ReInitialize;
        EventManager.OnEndWave += DisablePlayer;
        EventManager.OnItemEquipped += EquipItem;
    }

    void OnDisable()
    {
        EventManager.OnStartWave -= ReInitialize;
        EventManager.OnEndWave -= DisablePlayer;
        EventManager.OnItemEquipped -= EquipItem;
    }

    private void DisablePlayer()
    {
        _isPlaying = false;
    }

    private void ReInitialize()
    {
        Initialize();
    }

    private void Initialize()
    {
        _currentHealth = _maxHP.Value;
        EventManager.OnPlayerHealthInitialization?.Invoke(_maxHP.Value);
        transform.position = Vector2.zero;
        _isPlaying = true;
    }

    void Update()
    {
        if (_isPlaying)
        {
            transform.Translate(_currentMovementVector * _speed.Value * Time.deltaTime);
        }
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

        _currentHealth -= incomingDamage;
        EventManager.OnPlayerHealthChange?.Invoke(_currentHealth);
        if (_currentHealth <= 0f)
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

    private void EquipItem(Item newItem)
    {
        if (newItem.Equip(this))
        {
            _speed.ReCalculateValue();
        }
    }

    public void Die()
    {
        EventManager.OnDeath?.Invoke();
    }
}
