using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    protected SpriteRenderer _weaponSprite = null;
    [Header("Projectile")]
    [SerializeField]
    private Transform _exitTransform = null;
    [SerializeField]
    protected Projectile _projectile = null;
    [SerializeField]
    protected Vector3 _localProjectilePosition = Vector3.zero;
    [SerializeField]
    [Tooltip("Base speed of projectiles fired from this weapon")]
    protected float _baseProjVelMagnitude = 0f;

    private Vector2 _startingWeaponPosition = Vector2.zero;

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();
        _startingWeaponPosition = transform.position;
    }

    protected override void FireWeapon()
    {
        Vector3 direction = _swivelTransform.right; // This is more efficient and is automatically normalized foe the projectile
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _swivelTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Projectile projectile = Instantiate(_projectile, _exitTransform.position, Quaternion.Euler(0f, 0f, angle));
        projectile.Initialize(direction);
        //Time.timeScale = 0f;
    }

    protected override void RotateWeapon()
    {
        Vector3 direction = _currentTargetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _swivelTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _weaponSprite.flipX = false;
        if (!_weaponSprite.flipY && transform.position.x - _currentTargetPosition.x > 0.25f)
        {
            _weaponSprite.flipY = true;
            transform.localPosition = -_startingWeaponPosition;
        }
        else if (_weaponSprite.flipY && _currentTargetPosition.x - transform.position.x > 0.25f)
        {
            _weaponSprite.flipY = false;
            transform.localPosition = _startingWeaponPosition;
        }
    }

    protected override void ResetRotation()
    {
        base.ResetRotation();
        //_weaponSprite.flipY = false;
        if (transform.localPosition.x != _startingWeaponPosition.x)
        {
             _weaponSprite.flipY = false;
            _weaponSprite.flipX = true;
        }
    }


}
