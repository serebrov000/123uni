using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [Header("Settings")]
    public int damageValue = 10;
    public Color damageColor = Color.red;
    public float displayDuration = 1f;
    
    [Header("Motion")]
    public float riseSpeed = 2f;
    public float fadeSpeed = 1f;
    
    private TextMeshProUGUI _textMesh;
    private float _elapsedTime = 0f;
    private Color _originalColor;
    
    void Start()
    {
        // Create text mesh if not exists
        if (_textMesh == null)
        {
            GameObject textObj = new GameObject("DamageText");
            textObj.transform.SetParent(transform, false);
            textObj.transform.localPosition = Vector3.zero;
            
            _textMesh = textObj.AddComponent<TextMeshProUGUI>();
            _textMesh.text = damageValue.ToString();
            _textMesh.fontSize = 48;
            _textMesh.color = damageColor;
            _textMesh.alignment = TextAlignmentOptions.Center;
            _textMesh.fontStyle = FontStyles.Bold;
            
            // Add outline
            TMP_Outline outline = textObj.AddComponent<TMP_Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(2f, -2f);
        }
        
        _originalColor = damageColor;
    }
    
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        
        // Rise up
        transform.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);
        
        // Rotate slightly
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(_elapsedTime * 10f) * 15f);
        
        // Fade out
        if (_elapsedTime > displayDuration - 1f / fadeSpeed)
        {
            float alpha = 1f - (_elapsedTime - (displayDuration - 1f / fadeSpeed)) * fadeSpeed;
            alpha = Mathf.Max(0f, alpha);
            _textMesh.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
        }
        
        // Destroy when done
        if (_elapsedTime >= displayDuration)
        {
            Destroy(gameObject);
        }
    }
    
    public static void ShowDamage(Vector3 position, int damage, Color color)
    {
        GameObject damageObj = new GameObject("DamageNumber");
        damageObj.transform.position = position;
        
        DamageNumber damageNumber = damageObj.AddComponent<DamageNumber>();
        damageNumber.damageValue = damage;
        damageNumber.damageColor = color;
    }
}
