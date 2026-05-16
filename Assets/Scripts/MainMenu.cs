using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    
    [Header("Audio")]
    public AudioClip buttonClickSound;
    public AudioClip backgroundMusic;
    
    private AudioSource _audioSource;
    
    void Start()
    {
        Time.timeScale = 1f;
        
        // Create audio source if not exists
        if (GetComponent<AudioSource>() == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
        }
        else
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        // Play background music
        if (backgroundMusic != null)
        {
            _audioSource.clip = backgroundMusic;
            _audioSource.Play();
        }
        
        EnsureMainMenuUI();
    }
    
    public void StartGame()
    {
        PlaySound();
        SceneManager.LoadScene(1); // Load game scene
    }
    
    public void OpenSettings()
    {
        PlaySound();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
    
    public void CloseSettings()
    {
        PlaySound();
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
    
    public void ExitGame()
    {
        PlaySound();
        Debug.Log("Exiting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    private void PlaySound()
    {
        if (_audioSource != null && buttonClickSound != null)
        {
            _audioSource.PlayOneShot(buttonClickSound);
        }
        else if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }
    
    private void EnsureMainMenuUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }
        
        // Create main menu panel if not exists
        if (mainMenuPanel == null)
        {
            mainMenuPanel = CreatePanel("MainMenuPanel", canvas.transform);
        }
        
        // Create settings panel if not exists
        if (settingsPanel == null)
        {
            settingsPanel = CreatePanel("SettingsPanel", canvas.transform);
            settingsPanel.SetActive(false);
        }
        
        // Create title
        CreateText("Title", mainMenuPanel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
            new Vector2(0f, 180f), new Vector2(600f, 120f), "SOUL KNIGHT", 72, 
            new Color(1f, 0.8f, 0.2f), true);
        
        // Create Start button
        CreateButton("StartButton", mainMenuPanel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0f, 60f), new Vector2(280f, 80f), "START", new Color(0.2f, 0.8f, 0.3f), StartGame);
        
        // Create Settings button
        CreateButton("SettingsButton", mainMenuPanel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0f, -30f), new Vector2(280f, 80f), "SETTINGS", new Color(1f, 0.7f, 0.2f), OpenSettings);
        
        // Create Exit button
        CreateButton("ExitButton", mainMenuPanel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0f, -120f), new Vector2(280f, 80f), "EXIT", new Color(0.9f, 0.2f, 0.2f), ExitGame);
        
        // Settings panel content
        CreateText("SettingsTitle", settingsPanel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
            new Vector2(0f, 150f), new Vector2(500f, 100f), "SETTINGS", 56, Color.white, true);
        
        CreateText("VolumeText", settingsPanel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0f, 30f), new Vector2(400f, 60f), "Volume: 100%", 36, Color.white, false);
        
        CreateButton("BackButton", settingsPanel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
            new Vector2(0f, -80f), new Vector2(200f, 70f), "BACK", new Color(0.3f, 0.6f, 0.9f), CloseSettings);
    }
    
    private GameObject CreatePanel(string name, Transform parent)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.1f, 0.1f, 0.15f, 0.95f);
        
        return panel;
    }
    
    private void CreateButton(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 anchoredPosition, Vector2 size, string label, Color color, UnityEngine.Events.UnityAction onClick)
    {
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent, false);
        
        RectTransform rect = buttonObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;
        
        Image image = buttonObject.AddComponent<Image>();
        image.color = color;
        
        Button button = buttonObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = Color.Lerp(color, Color.white, 0.2f);
        colors.pressedColor = Color.Lerp(color, Color.black, 0.3f);
        colors.selectedColor = colors.highlightedColor;
        colors.colorMultiplier = 1f;
        colors.fadeDuration = 0.1f;
        button.colors = colors;
        
        button.onClick.AddListener(onClick);
        
        // Add juicy effect
        buttonObject.AddComponent<UIJuicyButton>();
        
        TextMeshProUGUI text = CreateText("Text", buttonObject.transform, Vector2.zero, Vector2.one,
            Vector2.zero, Vector2.zero, label, 42, new Color(0.05f, 0.05f, 0.05f), true);
    }
    
    private TextMeshProUGUI CreateText(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 anchoredPosition, Vector2 size, string text, float fontSize, Color color, bool bold)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent, false);
        
        RectTransform rect = textObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = size;
        
        TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        
        // Add outline for better visibility
        TMP_Outline outline = textObject.AddComponent<TMP_Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.5f);
        outline.effectDistance = new Vector2(2f, -2f);
        
        return tmp;
    }
}
