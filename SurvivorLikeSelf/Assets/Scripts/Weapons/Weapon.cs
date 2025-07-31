using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SpriteRenderer _weaponSprite = null;
    [SerializeField]
    private Transform _swivelTransform = null;
    [SerializeField]
    private Transform _visualTransform = null;

    [Header("Settings")]
    [SerializeField]
    private LayerMask _enemyLayer = 7;
    [SerializeField]
    private float _range = 3f;
    [SerializeField]
    private float _attackDistance = 0f;
    [SerializeField]
    private float _fireRate = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _animationPercentage = 0.5f;
    [SerializeField]
    private float _baseDamage = 1f;

    private float _timeSinceLastFiring = 0f;

    private float _sqrRange = 1f;

    private float _animationTotalTime = Mathf.Infinity;

    private Collider2D _currentlyTargetedCollider = null;

    private Enemy _closestEnemy = null;

    private Coroutine _firingRoutine = null;

    private void Start()
    {
        _sqrRange = _range * _range; // sqrMagnitude is faster than magnitude, so set up comparison float
        _animationTotalTime = _fireRate * _animationPercentage;
    }

    private void Update()
    {
        _timeSinceLastFiring += Time.deltaTime;
        if (GetClosestEnemy(out _closestEnemy))
        {
            TryFireWeapon();
        }
        
    }

    void OnDrawGizmos()
    {
        if (!_closestEnemy) return;

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
        if (_closestEnemy)
        {
            Vector3 direction = _closestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _swivelTransform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            //Debug.Log(_currentlyTargetedCollider.transform.position);
            if (_timeSinceLastFiring >= _fireRate)
            {
                if ((_closestEnemy.transform.position - transform.position).sqrMagnitude <= _sqrRange)
                {
                    _timeSinceLastFiring = 0f;
                    Debug.Log("Boom");
                    FireWeapon();
                }
            }
        }
    }

    private void FireWeapon()
    {
        if (_firingRoutine != null)
        {
            StopCoroutine(_firingRoutine);
            _firingRoutine = null;
        }
        _firingRoutine = StartCoroutine(Firing());
    }

    private IEnumerator Firing()
    {
        float startTime = Time.time;
        float halfTime = _animationTotalTime * 0.5f;
        
        Debug.Log("begin while check");
        Debug.Log(halfTime);
        Vector3 destination = new Vector3(0f, _attackDistance, 0f);
        while (halfTime > Time.time - startTime)
        {

            _visualTransform.localPosition = Vector3.Lerp(Vector3.zero, destination, (Time.time - startTime) / halfTime);
            yield return null;
        }

        startTime = Time.time;

        while (halfTime > Time.time - startTime)
        {
            //Vector3 destination = _visualTransform.up * _attackDistance;
            _visualTransform.localPosition = Vector3.Lerp(destination, Vector3.zero, (Time.time - startTime) / halfTime);
            yield return null;
        }
    }

    // Important note, this is relative to the weapon transform, not the player transform
    // We can later do this calculation once if we don't want to do per weapon position comparisons
    // Which would mean we're opting for the player position comparison instead
    // This is all before partitioning and Jobs and other methods to make this optimal
    private bool GetClosestEnemy(out Enemy closestEnemy)
    {
        List<Enemy> currentEnemies = SpawnManager.Instance.AliveEnemyList;
        Vector3 thisPosition = transform.position;
        int closestIndex = -1;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            float currentDistance = (currentEnemies[i].transform.position - thisPosition).sqrMagnitude;

            if ((currentEnemies[i].transform.position - thisPosition).sqrMagnitude < closestDistance)
            {
                closestDistance = currentDistance;
                closestIndex = i;
            }
        }
        if (closestIndex != -1)
        {
            closestEnemy = currentEnemies[closestIndex];
            return true;
        }
        else
        {
            closestEnemy = null;
            return false;
        }
    }

    // private void FixedUpdate()
    // {
    //     Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 3f, _enemyLayer);
    //     if (hitColliders.Length > 0)
    //     {
    //         // Aim Weapon
    //         int closestIndex = 0;
    //         float closestDistance = Mathf.Infinity;

    //         for (int i = 0; i < hitColliders.Length; i++)
    //         {
    //             float currentDistanceToCollider = (hitColliders[i].transform.position - transform.position).sqrMagnitude;
    //             if (currentDistanceToCollider < closestDistance)
    //             {
    //                 closestIndex = i;
    //             }
    //         }

    //         _currentlyTargetedCollider = hitColliders[closestIndex];
    //     }
    //     else
    //     {
    //         transform.rotation = Quaternion.identity; // Reset rotation if missed? maybe change late to not change at all
    //         _currentlyTargetedCollider = null;
    //     }
    //     //TryFireWeapon();
    //}
}
