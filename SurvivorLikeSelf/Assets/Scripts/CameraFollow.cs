using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Vector3 _cameraOffset = new Vector3(0f, 0f, -10f);
    private Player _player = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.transform.position + _cameraOffset;
    }
}
