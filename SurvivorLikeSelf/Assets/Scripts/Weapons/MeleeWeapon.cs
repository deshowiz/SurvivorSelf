using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Weapon")]
    [SerializeField]
    protected Collider2D _weaponCollider = null;
    [SerializeField]
    protected float _baseAttackDistance = 0.8f;
    [SerializeField]
    protected float _baseDamage = 1f;
    public float BaseDamage { get { return _baseDamage; } }
    [SerializeField]
    protected float _basePushback = 1f;
    protected float _animationTotalTime = Mathf.Infinity;

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();
        _animationTotalTime = _fireRate * _animationPercentage;
    }

    // protected override void RotateWeapon()
    // {
    //     base.RotateWeapon();
    //     if (_onlyWeapon)
    //     {
    //         if (_weaponSprite.flipX)
    //         {
    //             _weaponSprite.flipX = false;
    //             _weaponSprite.flipY = true;
    //         }
    //         float transformLocalX = transform.localPosition.x;
    //         if (transformLocalX == _startingWeaponPosition.x && transform.position.x - 0.1f > _currentTargetPosition.x)
    //         {
    //             _weaponSprite.flipY = true;
    //             transform.localPosition = new Vector3(-_startingWeaponPosition.x, _startingWeaponPosition.y, 0f);
    //         }
    //         else if (transformLocalX != _startingWeaponPosition.x && _currentTargetPosition.x - 0.1f > transform.position.x)
    //         {
    //             _weaponSprite.flipY = false;
    //             transform.localPosition = _startingWeaponPosition;
    //         }
    //     }
    //     else
    //     {
    //         _weaponSprite.flipY = transform.position.x >= _currentTargetPosition.x;
    //         _weaponSprite.flipX = false;
    //     }
    //     _lastPosX = transform.position.x;
    // }
    

    protected override void ResetRotation()
    {
        base.ResetRotation();
        _weaponSprite.flipY = false;

        if (transform.position.x != _lastPosX)
        {
            if (transform.position.x < _lastPosX)
            {
                _weaponSprite.flipX = true;
                if (_onlyWeapon)
                {
                    transform.localPosition = new Vector3(-_startingWeaponPosition.x, _startingWeaponPosition.y, 0f);
                }
            }
            else
            {
                _weaponSprite.flipX = false;
                if (_onlyWeapon)
                {
                    transform.localPosition = _startingWeaponPosition;
                }
            }
        }
        _lastPosX = transform.position.x;
    }

    protected override void FireWeapon()
    {
        if (_firingRoutine != null)
        {
            StopCoroutine(_firingRoutine);
            _firingRoutine = null;
            _hitEnemies.Clear();
        }
        _firingRoutine = StartCoroutine(Firing());
    }

    protected virtual IEnumerator Firing()
    {
        _weaponCollider.enabled = true;
        _targeting = false;
        float startTime = Time.time;
        float halfTime = _animationTotalTime * 0.5f;

        Vector3 destination = new Vector3(_baseAttackDistance, 0f, 0f);
        while (halfTime > Time.time - startTime)
        {
            _visualTransform.localPosition = Vector3.Lerp(Vector3.zero, destination, (Time.time - startTime) / halfTime);
            yield return new WaitForFixedUpdate();
        }
        _visualTransform.localPosition = destination;

        _weaponCollider.enabled = false;
        startTime = Time.time;

        while (halfTime > Time.time - startTime)
        {
            _visualTransform.localPosition = Vector3.Lerp(destination, Vector3.zero, (Time.time - startTime) / halfTime);
            yield return new WaitForFixedUpdate();
        }
        _visualTransform.localPosition = Vector3.zero;
        _targeting = true;
        _hitEnemies.Clear();
    }

    private List<uint> _hitEnemies = new List<uint>();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy hitEnemy = collision.GetComponent<Enemy>();
        _currentAttackDirection = _visualTransform.right;
        if (hitEnemy)
        {
            for (int i = 0; i < _hitEnemies.Count; i++)
            {
                if (hitEnemy._enemyId == _hitEnemies[i])
                {
                    return;
                }
            }

            hitEnemy.Damage(_baseDamage, _currentAttackDirection * _basePushback);
            if (hitEnemy != null)
            {
                _hitEnemies.Add(hitEnemy._enemyId);
            }
        }
        else
        {
            Debug.LogError(collision.gameObject.name + " is on the enemy layer for some reason?");
        }
    }
}
