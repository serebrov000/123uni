using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int coinValue = 10;
    
    [Header("Visual")]
    public float rotationSpeed = 200f;
    public float bobbingSpeed = 4f;
    public float bobbingAmount = 0.3f;
    public Color coinColor = Color.yellow;
    
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
                
                // Create a simple coin sprite (circle)
                Texture2D texture = new Texture2D(32, 32);
                Color[] pixels = new Color[32 * 32];
                
                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        float dist = Vector2.Distance(new Vector2(x, y), new Vector2(16, 16));
                        if (dist <= 14)
                        {
                            pixels[y * 32 + x] = coinColor;
                        }
                        else if (dist <= 16)
                        {
                            pixels[y * 32 + x] = new Color(1f, 0.85f, 0f);
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
        
        // Rotate coin (flipping effect)
        float rotation = Mathf.Sin(Time.time * rotationSpeed * 0.01f) * 90f;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        
        // Bobbing effect
        float bob = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        transform.position = _originalPosition + new Vector3(0, bob, 0);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_collected) return;
        
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }
    
    void CollectCoin()
    {
        _collected = true;
        
        // Add score
        GameManager.Instance?.AddScore(coinValue);
        
        Debug.Log($"Collected coin! +{coinValue} points");
        
        // Show collection effect
        ShowCollectionEffect();
        
        Destroy(gameObject, 0.3f);
    }
    
    void ShowCollectionEffect()
    {
        GameObject effect = new GameObject("CoinEffect");
        effect.transform.position = transform.position;
        
        ParticleSystem particles = effect.AddComponent<ParticleSystem>();
        
        var main = particles.main;
        main.startColor = coinColor;
        main.startSize = 0.3f;
        main.startSpeed = 2f;
        main.emissionRate = 20;
        main.gravityModifier = 0f;
        main.maxParticles = 30;
        
        var emission = particles.emission;
        emission.enabled = true;
        emission.rateOverTime = 20;
        
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
        
        particles.Play();
        
        Destroy(effect, 1f);
    }
}
