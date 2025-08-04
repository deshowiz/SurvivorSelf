using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Weapon")]
    [SerializeField]
    protected Collider2D _weaponCollider = null;
    [SerializeField]
    protected float _attackDistance = 0.8f;
    protected float _animationTotalTime = Mathf.Infinity;

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();
        _animationTotalTime = _fireRate * _animationPercentage;
    }

    protected override void FireWeapon()
    {
        if (_firingRoutine != null)
        {
            StopCoroutine(_firingRoutine);
            _firingRoutine = null;
        }
        _firingRoutine = StartCoroutine(Firing());
    }

    protected virtual IEnumerator Firing()
    {
        _weaponCollider.enabled = true;
        _targeting = false;
        float startTime = Time.time;
        float halfTime = _animationTotalTime * 0.5f;

        Vector3 destination = new Vector3(0f, _attackDistance, 0f);
        while (halfTime > Time.time - startTime)
        {
            _visualTransform.localPosition = Vector3.Lerp(Vector3.zero, destination, (Time.time - startTime) / halfTime);
            yield return null;
        }

        _weaponCollider.enabled = false;
        startTime = Time.time;

        while (halfTime > Time.time - startTime)
        {
            _visualTransform.localPosition = Vector3.Lerp(destination, Vector3.zero, (Time.time - startTime) / halfTime);
            yield return null;
        }
        _targeting = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy hitEnemy = collision.GetComponent<Enemy>();
        _currentAttackDirection = (_currentTargetPosition - transform.position).normalized;
        if (hitEnemy)
        {
            hitEnemy.Damage(_baseDamage, _currentAttackDirection * _basePushback);
        }
        else
        {
            Debug.LogError(collision.gameObject.name + " is on the enemy layer for some reason?");
        }
    }
}
