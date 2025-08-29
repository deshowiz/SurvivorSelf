using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField]
    private SpriteRenderer _playerSprite = null;
    [SerializeField]
    private WeaponPositions _weaponPositions = null;
    [SerializeField]
    private List<Weapon> _equippedWeapons = new List<Weapon>();

    [Header("Stats")]
    [SerializeField]
    public PlayerAttributesContainer _playerAttributes = null;

    [Header("Settings")]
    [SerializeField]
    private float _invulnTimer = 0.5f;
    [SerializeField]
    [Tooltip("Flash duration should be lower than invuln timer")]
    private float _flashDuration = 0.1f;

    private float _currentHealth = 10f;

    private Vector2 _currentMovementVector = Vector2.zero;
    private bool _isPlaying = false;
    private float _lastTimeHit = -Mathf.Infinity;

    private Coroutine _flashRoutine = null;

    // Point for later, use structs as stat modifiers for scaling enemies later per wave

    // void Start()
    // {
    //     Initialize();
    // }

    void OnEnable()
    {
        EventManager.OnSetPlayerCharacter += SetPlayerCharacter;
        EventManager.OnStartWave += ReInitialize;
        EventManager.OnEndWave += DisablePlayer;
        EventManager.OnItemEquipped += EquipItem;
        EventManager.OnWeaponEquipped += WeaponEquipItem;
    }

    void OnDisable()
    {
        //EventManager.OnFullInitialization -= FullInitialization;
        EventManager.OnStartWave -= ReInitialize;
        EventManager.OnEndWave -= DisablePlayer;
        EventManager.OnItemEquipped -= EquipItem;
        EventManager.OnWeaponEquipped -= WeaponEquipItem;
    }

    private void DisablePlayer()
    {
        _isPlaying = false;
    }

    public void SetPlayerCharacter(PlayerAttributesContainer newCharacter)
    {
        this._playerAttributes = newCharacter;
        AttributeReset();
        WeaponItem[] startingWeapons = _playerAttributes.StartingWeapons;

        if (startingWeapons == null) return;
        int lastIndex = startingWeapons.Length - 1;
        for (int i = 0; i < startingWeapons.Length; i++)
        {
            bool isLastWeapon = i == lastIndex ? true : false;
            WeaponSetOnCharacter(startingWeapons[i], isLastWeapon);
        }
        
        Initialize();
    }

    private void AttributeReset()
    {
        _playerAttributes.ResetAttributesToDefaultValues();
    }

    private void ReInitialize()
    {
        Initialize();
    }

    private void Initialize()
    {
        //_playerAttributes.UpdateAllAttributes();
        _currentHealth = _playerAttributes._maxHP.Value;
        EventManager.OnPlayerHealthInitialization?.Invoke(_currentHealth);
        transform.position = Vector2.zero;
        _isPlaying = true;
    }

    void Update()
    {
        if (_isPlaying)
        {
            transform.Translate(_currentMovementVector * _playerAttributes._speed.Value * Time.deltaTime);
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
        newItem.Equip(_playerAttributes);
    }

    private void WeaponEquipItem(WeaponItem newWeapon)
    {
        if (newWeapon.Equip(_playerAttributes))
        {
            Weapon weaponObject = Instantiate(newWeapon.WeaponPrefab,
             transform.position,
             Quaternion.identity,
              transform);
            _equippedWeapons.Add(weaponObject);
        }

        // Recalculate Weapon positions
        _weaponPositions.SetNewPositions(_equippedWeapons);
    }

    private void WeaponSetOnCharacter(WeaponItem newWeapon, bool isLastWeapon)
    {
        if (newWeapon.Equip(_playerAttributes, isLastWeapon))
        {
            Weapon weaponObject = Instantiate(newWeapon.WeaponPrefab,
             transform.position,
             Quaternion.identity,
              transform);
            _equippedWeapons.Add(weaponObject);
        }

        // Recalculate Weapon positions
        if (isLastWeapon)
        {
            _weaponPositions.SetNewPositions(_equippedWeapons);
        }
    }

    // public void ReCalcAllChangedStats(List<AttributeStat> changedStats)
    // {
    //     for (int i = 0; i < changedStats.Count; i++)
    //     {
    //         changedStats[i].ReCalculateValue();
    //     }
    //     EventManager.OnAttributeChange?.Invoke();
    // }

    public void Die()
    {
        EventManager.OnDeath?.Invoke();
    }
}
