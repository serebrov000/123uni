using UnityEngine;
using System;

public class ComboSystem : MonoBehaviour
{
    [Header("Combo Settings")]
    public float comboTimeWindow = 3f; // Время для поддержания комбо
    public int comboKillThreshold = 5; // Убийств для начала комбо
    
    [Header("Combo Rewards")]
    public float damageMultiplierPerCombo = 0.1f; // +10% урона за каждое комбо
    public int maxComboLevel = 10;
    
    [Header("UI")]
    public GameObject comboTextObject;
    
    private int _currentKills = 0;
    private int _comboCount = 0;
    private float _comboTimer = 0f;
    private bool _inCombo = false;
    
    public static ComboSystem Instance { get; private set; }
    
    public int CurrentCombo => _comboCount;
    public float DamageMultiplier => 1f + (_comboCount * damageMultiplierPerCombo);
    public bool InCombo => _inCombo;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        CreateComboUI();
    }
    
    void Update()
    {
        if (_inCombo)
        {
            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0f)
            {
                ResetCombo();
            }
            UpdateComboUI();
        }
    }
    
    public void AddKill()
    {
        _currentKills++;
        
        if (!_inCombo && _currentKills >= comboKillThreshold)
        {
            StartCombo();
        }
        
        if (_inCombo)
        {
            _comboCount++;
            _comboTimer = comboTimeWindow;
            
            if (_comboCount > maxComboLevel)
            {
                _comboCount = maxComboLevel;
            }
            
            ShowComboFeedback();
            UpdateComboUI();
        }
    }
    
    private void StartCombo()
    {
        _inCombo = true;
        _comboCount = comboKillThreshold;
        _comboTimer = comboTimeWindow;
        
        Debug.Log($"🔥 COMBO STARTED! {_comboCount} kills!");
        ShowComboFeedback();
    }
    
    private void ResetCombo()
    {
        if (_comboCount > 0)
        {
            Debug.Log($"❌ Combo broken at {_comboCount} kills!");
        }
        
        _inCombo = false;
        _comboCount = 0;
        _currentKills = 0;
        _comboTimer = 0f;
        
        UpdateComboUI();
    }
    
    private void ShowComboFeedback()
    {
        // Визуальный эффект
        Camera.main?.GetComponent<AudioSource>()?.Play();
        
        // Тряска камеры при высоком комбо
        if (_comboCount >= 10)
        {
            // Можно добавить тряску камеры
            Debug.Log("🎬 SCREEN SHAKE!");
        }
    }
    
    private void CreateComboUI()
    {
        if (comboTextObject != null) return;
        
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }
        
        comboTextObject = new GameObject("ComboText");
        comboTextObject.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = comboTextObject.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -100f);
        rect.sizeDelta = new Vector2(400f, 80f);
        
        TextMeshProUGUI text = comboTextObject.AddComponent<TextMeshProUGUI>();
        text.fontSize = 48;
        text.alignment = TextAlignmentOptions.Center;
        text.fontStyle = FontStyles.Bold;
        text.text = "";
        
        TMP_Outline outline = comboTextObject.AddComponent<TMP_Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(3f, -3f);
    }
    
    private void UpdateComboUI()
    {
        if (comboTextObject == null) return;
        
        TextMeshProUGUI text = comboTextObject.GetComponent<TextMeshProUGUI>();
        if (text == null) return;
        
        if (_inCombo && _comboCount > 0)
        {
            text.text = $"🔥 COMBO x{_comboCount}!";
            text.color = GetComboColor(_comboCount);
            
            // Анимация пульсации
            float scale = 1f + Mathf.Sin(Time.time * 10f) * 0.1f;
            comboTextObject.transform.localScale = Vector3.one * scale;
        }
        else
        {
            text.text = "";
            comboTextObject.transform.localScale = Vector3.one;
        }
    }
    
    private Color GetComboColor(int combo)
    {
        if (combo >= 20) return new Color(1f, 0f, 1f); // Фиолетовый
        if (combo >= 15) return new Color(1f, 0.5f, 0f); // Оранжевый
        if (combo >= 10) return new Color(1f, 0f, 0f); // Красный
        if (combo >= 5) return new Color(1f, 1f, 0f); // Желтый
        return new Color(0f, 1f, 0f); // Зеленый
    }
}
