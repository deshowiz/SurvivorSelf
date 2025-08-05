using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    private bool _onlyWeapon = true;
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

    private float _lastPosX = 0f;

    protected override void InitializeWeapon()
    {
        _lastPosX = transform.position.x;
        _startingWeaponPosition = transform.localPosition;
        base.InitializeWeapon();
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
        if (_onlyWeapon)
        {
            if (_weaponSprite.flipX)
            {
                _weaponSprite.flipX = false;
                _weaponSprite.flipY = true;
            }
            float transformLocalX = transform.localPosition.x;
            if (transformLocalX == _startingWeaponPosition.x && transform.position.x - 0.25f > _currentTargetPosition.x)
            {
                _weaponSprite.flipY = true;
                transform.localPosition = new Vector3(-_startingWeaponPosition.x, _startingWeaponPosition.y, 0f);
            }
            else if (transformLocalX != _startingWeaponPosition.x && _currentTargetPosition.x - 0.25f > transform.position.x)
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

    protected override void ResetRotation()
    {
        base.ResetRotation();
        //_weaponSprite.flipY = false;
        if (_onlyWeapon)
        {
            if (transform.localPosition.x != _startingWeaponPosition.x)
            {
                _weaponSprite.flipY = false;
                _weaponSprite.flipX = true;
            }
        }
        else
        {
            _weaponSprite.flipY = false;
            if (_lastPosX <= transform.position.x)
            {
                _weaponSprite.flipX = false;
                // transform.localPosition = _startingWeaponPosition;
            }
            else
            {
                _weaponSprite.flipX = true;
                // transform.localPosition = new Vector3(-_startingWeaponPosition.x, _startingWeaponPosition.y, 0f);
            }
        }
        _lastPosX = transform.position.x;
    }


}
