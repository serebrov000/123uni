using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Health Settings")]
    public int healAmount = 1;
    
    [Header("Visual")]
    public float bobbingSpeed = 2f;
    public float bobbingAmount = 0.4f;
    public Color heartColor = new Color(1f, 0.3f, 0.3f);
    
    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalPosition;
    private bool _collected = false;
    
    void Start()
    {
        _originalPosition = transform.position;
        
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                
                // Create a heart sprite
                Texture2D texture = new Texture2D(32, 32);
                Color[] pixels = new Color[32 * 32];
                
                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        // Simple heart shape
                        float u = (x - 16f) / 16f;
                        float v = (y - 16f) / 16f;
                        
                        float heart = Mathf.Pow(u * u + v * v - 1f, 3f) - u * u * v * v * v;
                        
                        if (heart <= 0f && v <= 0.5f)
                        {
                            pixels[y * 32 + x] = heartColor;
                        }
                        else
                        {
                            pixels[y * 32 + x] = Color.clear;
                        }
                    }
                }
                
                texture.SetPixels(pixels);
                texture.Apply();
                
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
                _spriteRenderer.sprite = sprite;
            }
        }
    }
    
    void Update()
    {
        if (_collected) return;
        
        // Bobbing effect
        float bob = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        transform.position = _originalPosition + new Vector3(0, bob, 0);
        
        // Gentle rotation
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 2f) * 10f);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_collected) return;
        
        if (other.CompareTag("Player"))
        {
            CollectHealth(other.GetComponent<PlayerHealth>());
        }
    }
    
    void CollectHealth(PlayerHealth playerHealth)
    {
        if (playerHealth == null) return;
        
        _collected = true;
        
        // Heal player
        playerHealth.Heal(healAmount);
        
        // Add to achievements
        Achievements.Instance?.CollectItem();
        
        Debug.Log($"Collected health! +{healAmount} HP");
        
        // Show collection effect
        ShowCollectionEffect();
        
        Destroy(gameObject, 0.3f);
    }
    
    void ShowCollectionEffect()
    {
        GameObject effect = new GameObject("HealthEffect");
        effect.transform.position = transform.position;
        
        ParticleSystem particles = effect.AddComponent<ParticleSystem>();
        
        var main = particles.main;
        main.startColor = heartColor;
        main.startSize = 0.4f;
        main.startSpeed = 1.5f;
        main.emissionRate = 15;
        main.gravityModifier = 0f;
        main.maxParticles = 20;
        
        var emission = particles.emission;
        emission.enabled = true;
        emission.rateOverTime = 15;
        
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
        
        particles.Play();
        
        Destroy(effect, 1f);
    }
}
