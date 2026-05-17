
// Separate class for scene setup functionality
public static class SoulKnightSceneSetup
{
    [MenuItem("Tools/Soul Knight/Quick Setup Scene", false, 2)]
    public static void SetupScene()
    {
        Debug.Log("🎮 Starting Soul Knight Scene Setup...");

        // Add required tags
        AddTags();

        // Create GameManager with all systems
        CreateGameManager();

        // Create Main Camera
        CreateMainCamera();

        // Create Player
        CreatePlayer();

        // Create Portal
        CreatePortal();

        // Create EnemySpawner
        CreateEnemySpawner();

        // Create UI
        CreateUI();

        // Create LevelManager
        CreateLevelManager();

        Debug.Log("✅ Scene setup complete! Ready to play.");
        EditorUtility.DisplayDialog("Soul Knight Setup", "Scene has been configured successfully!\n\nAll tags, game objects, and systems are ready.\n\nYou can now press Play to start the game.", "OK");
    }

    private static void AddTags()
    {
        string[] tags = { "Player", "Enemy", "Bullet", "Portal", "Pickup" };

        foreach (string tag in tags)
        {
            if (!TagExists(tag))
            {
                AddTag(tag);
            }
        }

        Debug.Log("✓ Tags added: Player, Enemy, Bullet, Portal, Pickup");
    }

    private static bool TagExists(string tag)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProperty = serializedObject.FindProperty("tags");

        for (int i = 0; i < tagsProperty.arraySize; i++)
        {
            SerializedProperty t = tagsProperty.GetArrayElementAtIndex(i);
            if (t.stringValue == tag)
            {
                return true;
            }
        }

        return false;
    }

    private static void AddTag(string tag)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProperty = serializedObject.FindProperty("tags");

        int insertIndex = tagsProperty.arraySize;
        tagsProperty.InsertArrayElementAtIndex(insertIndex);
        tagsProperty.GetArrayElementAtIndex(insertIndex).stringValue = tag;

        serializedObject.ApplyModifiedProperties();
        Debug.Log($"  + Added tag: {tag}");
    }

    private static void CreateGameManager()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager == null)
        {
            gameManager = new GameObject("GameManager");
        }

        if (gameManager.GetComponent<GameManager>() == null)
        {
            gameManager.AddComponent<GameManager>();
        }

        if (gameManager.GetComponent<ComboSystem>() == null)
        {
            gameManager.AddComponent<ComboSystem>();
        }

        if (gameManager.GetComponent<Achievements>() == null)
        {
            gameManager.AddComponent<Achievements>();
        }

        if (gameManager.GetComponent<ScreenShake>() == null)
        {
            gameManager.AddComponent<ScreenShake>();
        }

        Undo.RegisterCreatedObjectUndo(gameManager, "Create GameManager");
        Debug.Log("✓ GameManager created with Combo, Achievements, ScreenShake systems");
    }

    private static void CreateMainCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            mainCamera = camObj.AddComponent<Camera>();
            camObj.transform.position = new Vector3(0, 0, -10);
            camObj.tag = "MainCamera";
        }

        if (mainCamera.GetComponent<CameraFollow>() == null)
        {
            mainCamera.gameObject.AddComponent<CameraFollow>();
        }

        Undo.RegisterCreatedObjectUndo(mainCamera.gameObject, "Create Main Camera");
        Debug.Log("✓ Main Camera created with CameraFollow");
    }

    private static void CreatePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = new GameObject("Player");
            player.tag = "Player";
            player.transform.position = new Vector3(0, 0, 0);
        }

        if (player.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        if (player.GetComponent<BoxCollider2D>() == null)
        {
            player.AddComponent<BoxCollider2D>();
        }

        if (player.GetComponent<PlayerMovement>() == null)
        {
            player.AddComponent<PlayerMovement>();
        }

        if (player.GetComponent<PlayerAttack>() == null)
        {
            player.AddComponent<PlayerAttack>();
        }

        if (player.GetComponent<PlayerHealth>() == null)
        {
            player.AddComponent<PlayerHealth>();
        }

        if (player.GetComponent<PlayerVisualController>() == null)
        {
            player.AddComponent<PlayerVisualController>();
        }

        Undo.RegisterCreatedObjectUndo(player, "Create Player");
        Debug.Log("✓ Player created with Movement, Attack, Health components");
    }

    private static void CreatePortal()
    {
        GameObject portal = GameObject.FindGameObjectWithTag("Portal");
        if (portal == null)
        {
            portal = new GameObject("Portal");
            portal.tag = "Portal";
            portal.transform.position = new Vector3(50, 0, 0);
        }

        if (portal.GetComponent<SpriteRenderer>() == null)
        {
            SpriteRenderer sr = portal.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 5;
            sr.color = new Color(0.2f, 0.8f, 0.2f);
        }

        if (portal.GetComponent<CircleCollider2D>() == null)
        {
            CircleCollider2D collider = portal.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 1.5f;
        }

        if (portal.GetComponent<Portal>() == null)
        {
            portal.AddComponent<Portal>();
        }

        Undo.RegisterCreatedObjectUndo(portal, "Create Portal");
        Debug.Log("✓ Portal created");
    }

    private static void CreateEnemySpawner()
    {
        GameObject spawner = GameObject.Find("EnemySpawner");
        if (spawner == null)
        {
            spawner = new GameObject("EnemySpawner");
            spawner.transform.position = new Vector3(10, 0, 0);
        }

        if (spawner.GetComponent<EnemySpawner>() == null)
        {
            spawner.AddComponent<EnemySpawner>();
        }

        Undo.RegisterCreatedObjectUndo(spawner, "Create EnemySpawner");
        Debug.Log("✓ EnemySpawner created");
    }

    private static void CreateUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        CreateHealthPanel(canvas.transform);
        CreateCoinsText(canvas.transform);

        Undo.RegisterCreatedObjectUndo(canvas.gameObject, "Create UI");
        Debug.Log("✓ UI created with Health Panel and Coins display");
    }

    private static void CreateHealthPanel(Transform parent)
    {
        GameObject healthPanel = GameObject.Find("HealthPanel");
        if (healthPanel == null)
        {
            healthPanel = new GameObject("HealthPanel");
            healthPanel.transform.SetParent(parent, false);

            RectTransform rect = healthPanel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(100, -50);
            rect.sizeDelta = new Vector2(200, 50);

            Image image = healthPanel.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.5f);
        }

        GameObject heartsContainer = GameObject.Find("HeartsContainer");
        if (heartsContainer == null)
        {
            heartsContainer = new GameObject("HeartsContainer");
            heartsContainer.transform.SetParent(healthPanel.transform, false);
        }
    }

    private static void CreateCoinsText(Transform parent)
    {
        GameObject coinsText = GameObject.Find("CoinsText");
        if (coinsText == null)
        {
            coinsText = new GameObject("CoinsText");
            coinsText.transform.SetParent(parent, false);

            RectTransform rect = coinsText.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(-100, -50);
            rect.sizeDelta = new Vector2(200, 50);

            TextMeshProUGUI text = coinsText.AddComponent<TextMeshProUGUI>();
            text.text = "💰 0";
            text.fontSize = 36;
            text.alignment = TextAlignmentOptions.Right;
            text.color = Color.yellow;
        }
    }

    private static void CreateLevelManager()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager == null)
        {
            levelManager = new GameObject("LevelManager");
        }

        if (levelManager.GetComponent<LevelManager>() == null)
        {
            levelManager.AddComponent<LevelManager>();
        }

        Undo.RegisterCreatedObjectUndo(levelManager, "Create LevelManager");
        Debug.Log("✓ LevelManager created with auto-background generation");
    }
}
