using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    protected SpriteRenderer _weaponSprite = null;
    [Header("Projectile")]
    [SerializeField]
    private Transform _exitTransform = null;
    [SerializeField]
    protected GameObject _projectile = null;
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
        Vector3 direction = _currentTargetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _swivelTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(_projectile, _exitTransform.position, Quaternion.Euler(0f, 0f, angle));
        //Time.timeScale = 0f;
    }

    protected override void RotateWeapon()
    {
        Vector3 direction = _currentTargetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _swivelTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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


}
