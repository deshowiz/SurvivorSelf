using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private float _baseSpeed = 20f;
    [SerializeField]
    private float _baseDamage = 1f;
    public float BaseDamage {get{ return _baseDamage; }}
    [SerializeField]
    private float _baseKnockback = 75f;
    public float BaseKnockback {get{ return _baseKnockback; }}
    [SerializeField]
    private float _lifetime = 1f;

    private Vector3 _currentDirection = Vector3.zero;

    private float _currentLifetime = 0f;

    private Vector2 _lastPosition = Vector2.zero;


    public void Initialize(Vector3 newDirection)
    {
        _currentDirection = transform.right;
        _lastPosition = transform.position;
    }


    private void FixedUpdate()
    {
        if (_currentLifetime >= _lifetime)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }
        float fixedDelta = Time.fixedDeltaTime;
        _currentLifetime += fixedDelta;
        float distanceThisFrame = _baseSpeed * fixedDelta;
        transform.position += _currentDirection * distanceThisFrame;

        RaycastHit2D hit = Physics2D.Raycast(_lastPosition, _currentDirection, distanceThisFrame, _enemyLayer);

        if (hit.collider != null)
        {
            Enemy hitEnemy = hit.collider.GetComponent<Enemy>();
            if (hitEnemy != null)
            {
                hitEnemy.Damage(_baseDamage, _currentDirection * _baseKnockback);
            }
            else
            {
                Debug.LogError(hit.collider.gameObject.name + " is on the enemy layer but is not an enemy?");
            }
            Destroy(gameObject);
        }
        _lastPosition = transform.position;
    }
    

}
