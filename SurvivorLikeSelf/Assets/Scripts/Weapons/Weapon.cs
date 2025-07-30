using System;
using System.Xml.Serialization;
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
    private float _fireRate = 1f;
    [SerializeField]
    private float _baseDamage = 1f;

    private float _timeSinceLastFiring = 0f;

    private float _sqrRange = 1f;

    private Collider2D _currentlyTargetedCollider = null;

    private void Start()
    {
        _sqrRange = _range * _range; // sqrMagnitude is faster than magnitude, so set up comparison float
    }

    private void Update()
    {
        _timeSinceLastFiring += Time.deltaTime;
        TryFireWeapon();
    }

    private void TryFireWeapon()
    {
        if (_currentlyTargetedCollider)
        {
            Vector3 direction = _currentlyTargetedCollider.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            //Debug.Log(_currentlyTargetedCollider.transform.position);
            if (_timeSinceLastFiring >= _fireRate)
            {
                if ((_currentlyTargetedCollider.transform.position - transform.position).sqrMagnitude <= _sqrRange)
                {
                    _timeSinceLastFiring = 0f;
                    Debug.Log("Boom");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 3f, _enemyLayer);
        if (hitColliders.Length > 0)
        {
            // Aim Weapon
            int closestIndex = 0;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < hitColliders.Length; i++)
            {
                float currentDistanceToCollider = (hitColliders[i].transform.position - transform.position).sqrMagnitude;
                if (currentDistanceToCollider < closestDistance)
                {
                    closestIndex = i;
                }
            }

            _currentlyTargetedCollider = hitColliders[closestIndex];
        }
        else
        {
            transform.rotation = Quaternion.identity; // Reset rotation if missed? maybe change late to not change at all
            _currentlyTargetedCollider = null;
        }
        //TryFireWeapon();
    }
}
