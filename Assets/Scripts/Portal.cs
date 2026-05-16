using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    public int targetLevelIndex = 1;

    [Header("Visual Effects")]
    public float rotationSpeed = 50f;
    public float pulseSpeed = 3f;
    public float pulseAmount = 0.2f;

    private SpriteRenderer _spriteRenderer;
    private Color _baseColor;
    private Vector3 _originalScale;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _baseColor = _spriteRenderer.color;
        }
        _originalScale = transform.localScale;
    }

    void Update()
    {
        // Rotate portal for visual effect
        transform.rotation = Quaternion.Euler(0, 0, Time.time * rotationSpeed);

        // Pulse effect
        if (_spriteRenderer != null)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            _spriteRenderer.color = new Color(
                _baseColor.r + pulse * 0.1f,
                _baseColor.g + pulse * 0.1f,
                _baseColor.b + pulse * 0.1f,
                _baseColor.a
            );
        }

        // Scale pulse
        float scalePulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.1f;
        transform.localScale = _originalScale * scalePulse;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered portal! Going to level {targetLevelIndex}");
            LevelManager.Instance?.GoToNextLevel();
        }
    }
}
