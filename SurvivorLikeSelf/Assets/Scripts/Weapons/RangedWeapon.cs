using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Projectile")]
    [SerializeField]
    private Transform _exitTransform = null;
    [SerializeField]
    protected Projectile _projectile = null;
    public Projectile Projectile { get { return _projectile; } }

    protected override void FireWeapon()
    {
        AudioManager.Instance.PlaySound(_weaponAudio).Forget();
        
        Vector3 direction = _swivelTransform.right; // This is more efficient and is automatically normalized foe the projectile
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _swivelTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Projectile projectile = Instantiate(_projectile, _exitTransform.position, Quaternion.Euler(0f, 0f, angle));
        projectile.Initialize(direction);
    }

    protected override void ResetRotation()
    {
        base.ResetRotation();
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
            }
            else
            {
                _weaponSprite.flipX = true;
            }
        }
        _lastPosX = transform.position.x;
    }


}
