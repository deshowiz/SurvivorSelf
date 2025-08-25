using UnityEngine;

public class SpawningVisual : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float _indicationSpeed = 3f;
    void Update()
    {
        Color prevColor = _spriteRenderer.color;
        _spriteRenderer.color = new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.Abs(Mathf.Cos(Time.time *_indicationSpeed)));
    }
}
