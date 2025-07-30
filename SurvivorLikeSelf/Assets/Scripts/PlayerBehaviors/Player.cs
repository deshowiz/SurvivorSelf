using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _speedMultiplier = 1f;
    private Vector2 _currentMovementVector = Vector2.zero;

    void Update()
    {
        transform.Translate(_currentMovementVector * _speedMultiplier * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        _currentMovementVector = value.Get<Vector2>();
    }
}
