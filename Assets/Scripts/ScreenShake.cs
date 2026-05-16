using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.3f;
    public float dampingSpeed = 1f;
    
    private Vector3 _initialPosition;
    private float _shakeTimer = 0f;
    private float _currentMagnitude = 0f;
    
    public static ScreenShake Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        _initialPosition = transform.position;
    }
    
    void Update()
    {
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;
            
            float shakeAmount = _currentMagnitude * (_shakeTimer / shakeDuration);
            
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;
            
            transform.position = _initialPosition + new Vector3(x, y, 0f);
        }
        else
        {
            transform.position = _initialPosition;
            _shakeTimer = 0f;
            _currentMagnitude = 0f;
        }
    }
    
    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        if (duration < 0f) duration = shakeDuration;
        if (magnitude < 0f) magnitude = shakeMagnitude;
        
        _shakeTimer = duration;
        _currentMagnitude = magnitude;
        _initialPosition = transform.position;
    }
}
