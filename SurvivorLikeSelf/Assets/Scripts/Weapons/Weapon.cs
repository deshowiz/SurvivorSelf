using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    // [SerializeField]
    // private SpriteRenderer _weaponSprite = null;
    [SerializeField]
    protected Transform _swivelTransform = null;
    [SerializeField]
    protected Transform _visualTransform = null;
    [SerializeField]
    protected SpriteRenderer _weaponSprite = null;
    [SerializeField]
    protected AudioClip _weaponAudio = null;
    
    [Header("Settings")]
    [SerializeField]
    public bool _onlyWeapon = true;
    [SerializeField]
    protected LayerMask _enemyLayer = 7;
    [SerializeField]
    protected Vector3 _baseRotation = new Vector3(0f, 0f, -90f);
    [SerializeField]
    protected float _range = 3f;
    [SerializeField]
    protected float _detectionRangeMultiplier = 1.5f;
    [SerializeField]
    protected float _fireRate = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    protected float _animationPercentage = 0.5f;
    

    protected float _timeSinceLastFiring = 0f;

    protected float _sqrRange = 1f;

    protected float _trackingRange = 0f;

    [SerializeField]
    protected Enemy _closestEnemy = null;

    protected Vector3 _currentTargetPosition = Vector3.positiveInfinity;

    protected Coroutine _firingRoutine = null;

    protected bool _targeting = true;

    protected Vector3 _currentAttackDirection = Vector3.zero;

    protected Vector2 _startingWeaponPosition = Vector2.zero;

    protected float _lastPosX = 0f;

    protected void Start()
    {
        InitializeWeapon();
    }

    protected virtual void InitializeWeapon()
    {
        _sqrRange = _range * _range; // sqrMagnitude is faster than magnitude, so set up comparison float
        _trackingRange = _range * _detectionRangeMultiplier;
        _startingWeaponPosition = transform.localPosition;
        _lastPosX = transform.position.x;
        ResetRotation();
    }

    protected void Update()
    {
        _timeSinceLastFiring += Time.deltaTime;
        if (!_targeting)
        {
            if (_closestEnemy)
            {
                _currentTargetPosition = _closestEnemy.transform.position;
            }
            RotateWeapon();
        }
        else if (_closestEnemy)
        {
            TryFireWeapon();
        }
        else
        {
            ResetRotation();
        }
    }

    protected void OnDrawGizmos()
    {
        if (!_closestEnemy) return;

        Gizmos.color = new Color(1f, 1f, 1f, 1f);
        Gizmos.DrawSphere(transform.position, _trackingRange);

        if ((_closestEnemy.transform.position - transform.position).sqrMagnitude <= _sqrRange)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        }
        else
        {
            if (_timeSinceLastFiring >= _fireRate)
            {
                Gizmos.color = new Color(0.5f, 0.5f, 0f, 0.1f);
            }
            else
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
            }
        }

        Gizmos.DrawSphere(transform.position, _range);
        
        
    }

    private void TryFireWeapon()
    {
        if (_closestEnemy != null)
        {
            _currentTargetPosition = _closestEnemy.transform.position;

            if (_timeSinceLastFiring >= _fireRate)
            {
                if ((_closestEnemy.transform.position - transform.position).sqrMagnitude <= _sqrRange)
                {
                    _timeSinceLastFiring = 0f;
                    RotateWeapon();
                    FireWeapon();
                }
                else if (_targeting)
                {
                    RotateWeapon();
                }
            }
            else
            {
                RotateWeapon();
            }
        }
    }

    protected virtual void RotateWeapon()
    {
        Vector3 direction = _currentTargetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _swivelTransform.rotation = Quaternion.AngleAxis(angle - _baseRotation.z, Vector3.forward);
        if (_onlyWeapon)
        {
            if (_weaponSprite.flipX)
            {
                _weaponSprite.flipX = false;
                _weaponSprite.flipY = true;
            }
            float transformLocalX = transform.localPosition.x;
            if (transformLocalX == _startingWeaponPosition.x && transform.position.x - 0.1f > _currentTargetPosition.x)
            {
                _weaponSprite.flipY = true;
                transform.localPosition = new Vector3(-_startingWeaponPosition.x, _startingWeaponPosition.y, 0f);
            }
            else if (transformLocalX != _startingWeaponPosition.x && _currentTargetPosition.x - 0.1f > transform.position.x)
            {
                _weaponSprite.flipY = false;
                transform.localPosition = _startingWeaponPosition;
            }
        }
        else
        {
            _weaponSprite.flipY = transform.position.x >= _currentTargetPosition.x;
            _weaponSprite.flipX = false;
        }
        _lastPosX = transform.position.x;
    }

    protected virtual void ResetRotation()
    {
        _swivelTransform.rotation = Quaternion.Euler(_baseRotation);
    }

    protected virtual void FireWeapon(){}

    #region Deprecated Enemy Tracking
    // Important note, this is relative to the weapon transform, not the player transform
    // We can later do this calculation once if we don't want to do per weapon position comparisons
    // Which would mean we're opting for the player position comparison instead
    // This is all before partitioning and Jobs and other methods to make this optimal
    // private bool GetClosestEnemy(out Enemy closestEnemy)
    // {
    //     List<Enemy> currentEnemies = SpawnManager.Instance.AliveEnemyList;
    //     Vector3 thisPosition = transform.position;
    //     int closestIndex = -1;
    //     float closestDistance = Mathf.Infinity;

    //     for (int i = 0; i < currentEnemies.Count; i++)
    //     {
    //         float currentDistance = (currentEnemies[i].transform.position - thisPosition).sqrMagnitude;

    //         if ((currentEnemies[i].transform.position - thisPosition).sqrMagnitude < closestDistance)
    //         {
    //             closestDistance = currentDistance;
    //             closestIndex = i;
    //         }
    //     }
    //     if (closestIndex != -1)
    //     {
    //         closestEnemy = currentEnemies[closestIndex];
    //         return true;
    //     }
    //     else
    //     {
    //         closestEnemy = null;
    //         return false;
    //     }
    // }
    #endregion

    private void FixedUpdate()
    {
        #region Disabled Physics
        // Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 3f, _enemyLayer);
        // if (hitColliders.Length > 0)
        // {
        //     // Aim Weapon
        //     int closestIndex = 0;
        //     float closestDistance = Mathf.Infinity;

        //     for (int i = 0; i < hitColliders.Length; i++)
        //     {
        //         float currentDistanceToCollider = (hitColliders[i].transform.position - transform.position).sqrMagnitude;
        //         if (currentDistanceToCollider < closestDistance)
        //         {
        //             closestIndex = i;
        //         }
        //     }

        //     _currentlyTargetedCollider = hitColliders[closestIndex];
        // }
        // else
        // {
        //     transform.rotation = Quaternion.identity; // Reset rotation if missed? maybe change late to not change at all
        //     _currentlyTargetedCollider = null;
        // }
        // //TryFireWeapon();
        #endregion
        if (_targeting)
        {
            // Cast on the enemy layer looking for enemies to hit?
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, _trackingRange, _enemyLayer);

            if (enemiesHit.Length != 0)
            {
                int closestIndex = -1;
                float closestDistance = Mathf.Infinity;
                for (int i = 0; i < enemiesHit.Length; i++)
                {
                    float currentDistance = (enemiesHit[i].transform.position - transform.position).sqrMagnitude;

                    if ((enemiesHit[i].transform.position - transform.position).sqrMagnitude < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestIndex = i;
                    }
                }

                if (closestIndex != -1)
                {
                    _closestEnemy = enemiesHit[closestIndex].transform.GetComponent<Enemy>();
                }
                else
                {
                    _closestEnemy = null;
                }
            }
            else
            {
                _closestEnemy = null;
            }
        }
    }

    //protected virtual void OnTriggerEnter2D(Collider2D collision) {}
}
