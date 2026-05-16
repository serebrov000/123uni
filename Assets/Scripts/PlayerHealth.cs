using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("❤️ Здоровье")]
    public int maxHealth = 3;
    [HideInInspector] public int currentHealth;
    
    [Header("Эффекты")]
    public float invincibilityTime = 1f;
    private bool _isInvincible = false;
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement movement;
    private PlayerAttack attack;
    
    public static PlayerHealth Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        
        GameManager.Instance?.UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (_isInvincible || currentHealth <= 0) return;
        
        currentHealth -= amount;
        Debug.Log($"👤 Урон! Здоровье: {currentHealth}/{maxHealth}");
        
        // Show damage number
        DamageNumber.ShowDamage(transform.position, amount, Color.red);
        
        StartCoroutine(InvincibilityCoroutine());
        
        if (currentHealth <= 0)
        {
            Die();
        }
        GameManager.Instance?.UpdateHealthUI();
    }
    
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        Debug.Log($"💚 Лечение! Здоровье: {currentHealth}/{maxHealth}");
        
        // Show heal effect
        GameObject healEffect = new GameObject("HealEffect");
        healEffect.transform.position = transform.position;
        
        ParticleSystem particles = healEffect.AddComponent<ParticleSystem>();
        var main = particles.main;
        main.startColor = Color.green;
        main.startSize = 0.3f;
        main.startSpeed = 1f;
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
        Destroy(healEffect, 1f);
        
        GameManager.Instance?.UpdateHealthUI();
    }

    System.Collections.IEnumerator InvincibilityCoroutine()
    {
        _isInvincible = true;
        float elapsed = 0f;
        
        while (elapsed < invincibilityTime)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
        _spriteRenderer.enabled = true;
        _isInvincible = false;
    }

    void Die()
    {
        Debug.Log("💀 Игрок умер!");
        if (movement != null)
        {
            movement.enabled = false;
        }
        if (attack != null)
        {
            attack.enabled = false;
        }
        enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        _spriteRenderer.color = Color.gray;
        Invoke(nameof(ShowGameOver), 0.5f);
    }

    void ShowGameOver()
    {
        GameManager.Instance?.ShowGameOver();
    }
}
