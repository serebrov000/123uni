using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    public LevelType currentLevelType = LevelType.Forest;
    public int currentLevelIndex = 0;
    public int totalLevels = 3;

    [Header("Portal Settings")]
    public float portalXPosition = 100f;
    public float portalYPosition = -2f;

    [Header("Background References")]
    public SpriteRenderer backgroundRenderer;
    
    // Level backgrounds - assign in Inspector or use defaults
    public Sprite forestBackground;
    public Sprite snowBackground;
    public Sprite waterBackground;

    // Default colors for each level
    private Color[] levelColors = new Color[]
    {
        new Color(0.2f, 0.5f, 0.2f),    // Forest - green tint
        new Color(0.8f, 0.9f, 1.0f),    // Snow - light blue/white
        new Color(0.2f, 0.4f, 0.6f)     // Water - blue
    };

    private GameObject _portal;
    private bool _levelTransitioning = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetupLevel(currentLevelIndex);
    }

    public void SetupLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= totalLevels)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        currentLevelIndex = levelIndex;
        currentLevelType = (LevelType)levelIndex;
        _levelTransitioning = false;

        // Setup background
        SetupBackground();

        // Create portal at end of level
        CreatePortal();

        // Update enemy spawner bounds for this level
        UpdateEnemySpawnerBounds();

        Debug.Log($"Level {levelIndex + 1} ({currentLevelType}) started!");
    }

    void SetupBackground()
    {
        if (backgroundRenderer == null)
        {
            // Find or create background object
            GameObject bgObject = GameObject.Find("Background");
            if (bgObject == null)
            {
                bgObject = new GameObject("Background");
                bgObject.transform.position = new Vector3(0, 0, 10);
                backgroundRenderer = bgObject.AddComponent<SpriteRenderer>();
                backgroundRenderer.sortingOrder = -10;
            }
            else
            {
                backgroundRenderer = bgObject.GetComponent<SpriteRenderer>();
                if (backgroundRenderer == null)
                {
                    backgroundRenderer = bgObject.AddComponent<SpriteRenderer>();
                }
            }
        }

        // Set background based on level type
        Sprite levelSprite = GetBackgroundSpriteForLevel(currentLevelType);
        if (levelSprite != null)
        {
            backgroundRenderer.sprite = levelSprite;
            backgroundRenderer.color = Color.white;
        }
        else
        {
            // Use solid color if no sprite available
            backgroundRenderer.sprite = null;
            backgroundRenderer.color = levelColors[currentLevelIndex];
        }

        // Also set camera background color
        Camera.main.backgroundColor = levelColors[currentLevelIndex];
    }

    Sprite GetBackgroundSpriteForLevel(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.Forest:
                return forestBackground;
            case LevelType.Snow:
                return snowBackground;
            case LevelType.Water:
                return waterBackground;
            default:
                return null;
        }
    }

    void CreatePortal()
    {
        // Destroy existing portal if any
        if (_portal != null)
        {
            Destroy(_portal);
        }

        // Create portal object
        _portal = new GameObject("LevelPortal");
        _portal.transform.position = new Vector3(portalXPosition, portalYPosition, 0);

        // Add visual representation
        SpriteRenderer portalRenderer = _portal.AddComponent<SpriteRenderer>();
        portalRenderer.sortingOrder = 5;
        
        // Create a simple portal visual (colored circle)
        Color portalColor = GetPortalColorForLevel(currentLevelType);
        portalRenderer.color = portalColor;

        // Add collider for trigger detection
        CircleCollider2D portalCollider = _portal.AddComponent<CircleCollider2D>();
        portalCollider.isTrigger = true;
        portalCollider.radius = 1.5f;

        // Add portal script
        Portal portalScript = _portal.AddComponent<Portal>();
        portalScript.targetLevelIndex = currentLevelIndex + 1;

        Debug.Log($"Portal created at x={portalXPosition} for level {currentLevelIndex + 1}");
    }

    Color GetPortalColorForLevel(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.Forest:
                return new Color(0.2f, 0.8f, 0.2f);   // Green portal
            case LevelType.Snow:
                return new Color(0.8f, 0.9f, 1.0f);   // White/light blue portal
            case LevelType.Water:
                return new Color(0.2f, 0.6f, 1.0f);   // Blue portal
            default:
                return Color.white;
        }
    }

    void UpdateEnemySpawnerBounds()
    {
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            // Adjust spawn bounds based on level
            // Portal is at portalXPosition, so enemies should spawn before it
            spawner.maxSpawnX = portalXPosition - 10f;
        }
    }

    public void GoToNextLevel()
    {
        if (_levelTransitioning) return;

        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex >= totalLevels)
        {
            // All levels completed!
            _levelTransitioning = true;
            
            // Add to achievements
            Achievements.Instance?.CompleteLevel();
            
            GameManager.Instance?.ShowGameWin();
            return;
        }

        _levelTransitioning = true;
        
        // Add to achievements
        Achievements.Instance?.CompleteLevel();
        
        StartCoroutine(TransitionToLevel(nextLevelIndex));
    }

    System.Collections.IEnumerator TransitionToLevel(int levelIndex)
    {
        // Fade out effect could be added here
        yield return new WaitForSeconds(0.5f);

        SetupLevel(levelIndex);

        // Reset player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, 0, 0);
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        // Fade in effect could be added here
        yield return new WaitForSeconds(0.5f);

        _levelTransitioning = false;
    }

    public void LoadSpecificLevel(int levelIndex)
    {
        SetupLevel(levelIndex);
    }
}
