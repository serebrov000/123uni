using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponName = "New Weapon";
    public int damageBonus = 5;
    public float attackSpeedBonus = 0.2f;
    public Color weaponColor = Color.yellow;
    
    [Header("Visual")]
    public float rotationSpeed = 100f;
    public float bobbingSpeed = 3f;
    public float bobbingAmount = 0.5f;
    
    private SpriteRenderer _spriteRenderer;
    private Vector3 _originalPosition;
    private bool _collected = false;
    
    void Start()
    {
        _originalPosition = transform.position;
        
        // Create visual if no sprite renderer
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                _spriteRenderer.color = weaponColor;
            }
        }
    }
    
    void Update()
    {
        if (_collected) return;
        
        // Rotate weapon
        transform.rotation = Quaternion.Euler(0, 0, Time.time * rotationSpeed);
        
        // Bobbing effect
        float bob = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        transform.position = _originalPosition + new Vector3(0, bob, 0);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (_collected) return;
        
        if (other.CompareTag("Player"))
        {
            CollectWeapon(other.GetComponent<PlayerAttack>());
        }
    }
    
    void CollectWeapon(PlayerAttack playerAttack)
    {
        if (playerAttack == null) return;
        
        _collected = true;
        
        // Apply bonuses
        playerAttack.damage += damageBonus;
        playerAttack.attackCooldown = Mathf.Max(0.1f, playerAttack.attackCooldown - attackSpeedBonus);
        
        // Add to achievements
        Achievements.Instance?.CollectItem();
        
        Debug.Log($"Collected {weaponName}! Damage +{damageBonus}, Attack Speed +{attackSpeedBonus}");
        
        // Show pickup effect
        ShowPickupEffect();
        
        // Screen shake
        if (ScreenShake.Instance != null)
        {
            ScreenShake.Instance.Shake(0.2f, 0.15f);
        }
        
        // Destroy after effect
        Destroy(gameObject, 0.5f);
    }
    
    void ShowPickupEffect()
    {
        // Simple particle-like effect
        GameObject effect = new GameObject("PickupEffect");
        effect.transform.position = transform.position;
        
        SpriteRenderer effectRenderer = effect.AddComponent<SpriteRenderer>();
        effectRenderer.color = weaponColor;
        effectRenderer.sortingOrder = 10;
        
        // Scale up and fade out
        LeanTweenEffect(effect.transform, weaponColor);
        
        Destroy(effect, 0.5f);
    }
    
    void LeanTweenEffect(Transform effectTransform, Color color)
    {
        // Simple coroutine-like effect without external dependencies
        StartCoroutine(ScaleAndFade(effectTransform, color));
    }
    
    System.Collections.IEnumerator ScaleAndFade(Transform effectTransform, Color color)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = new Vector3(3f, 3f, 3f);
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            effectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            
            if (GetComponent<SpriteRenderer>() != null)
            {
                Color fadedColor = new Color(color.r, color.g, color.b, 1f - t);
                GetComponent<SpriteRenderer>().color = fadedColor;
            }
            
            yield return null;
        }
    }
}
