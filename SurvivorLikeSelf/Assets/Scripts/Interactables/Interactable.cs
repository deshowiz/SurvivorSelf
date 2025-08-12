using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private float _distanceToPickup = 2f;
    private float _sqrDistancePickup = 0f;
    [SerializeField]
    private float _distanceToAbsorb = 2f;
    private float _sqrDistanceAbsorb = 0f;
    Player _player = null;

    private bool _pickedUp = false;
    Coroutine _flyingRoutine = null;

    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _sqrDistancePickup = _distanceToPickup * _distanceToPickup;
        _sqrDistanceAbsorb = _distanceToAbsorb * _distanceToAbsorb;
    }
    private void Update()
    {
        if (!_pickedUp)
        {
            if (_sqrDistancePickup > (_player.transform.position - transform.position).sqrMagnitude)
            {
                _pickedUp = true;
                if (_flyingRoutine != null)
                {
                    StopCoroutine(_flyingRoutine);
                    _flyingRoutine = null;
                }
                _flyingRoutine = StartCoroutine(FlyTowardsPlayer());
            }
        }
        
    }

    private IEnumerator FlyTowardsPlayer()
    {
        float startTime = 0f;
        while (_sqrDistanceAbsorb < (_player.transform.position - transform.position).sqrMagnitude)
        {
            startTime += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, _player.transform.position, startTime);
            yield return null;
        }
        EventManager.OnPickedUpInteractable?.Invoke(this);
    }
}
