using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Soul Knight Game Settings - Complete Editor Configuration Tool
/// Provides a unified interface for configuring all game parameters
/// </summary>
public class SoulKnightGameSettings : EditorWindow
{
    // Tab selection
    private int _currentTab = 0;
    private Vector2 _scrollPosition;
    
    // Player Settings
    private float playerSpeed = 5f;
    private float playerJumpForce = 10f;
    private float playerCrouchSpeed = 2f;
    private int playerMaxHealth = 3;
    private float playerInvincibilityTime = 1f;
    private Vector3 playerScale = new Vector3(0.3f, 0.3f, 0.3f);
    
    // Player Attack Settings
    private float playerFireRate = 0.5f;
    private int playerMeleeDamage = 2;
    private float playerMeleeCooldown = 0.35f;
    private float playerMeleeRange = 1.2f;
    
    // Bullet Settings
    private float bulletSpeed = 15f;
    private int bulletDamage = 1;
    private float bulletLifetime = 3f;
    
    // Camera Settings
    private float cameraSmoothTime = 0.18f;
    private Vector3 cameraOffset = new Vector3(0f, 1.2f, -10f);
    private float cameraMinX = -4f;
    private float cameraMaxX = 105f;
    private float cameraMinY = -2f;
    private float cameraMaxY = 8f;
    
    // Enemy Settings
    private int enemyMaxHealth = 3;
    private float enemyDetectionRange = 8f;
    private float enemyPatrolSpeed = 1.2f;
    private float enemyChaseSpeed = 2.4f;
    private float enemyAttackRange = 1f;
    private float enemyRangedAttackRange = 6f;
    private int enemyAttackDamage = 1;
    private float enemyAttackCooldown = 1.5f;
    private float enemyRangedAttackCooldown = 2.2f;
    private float enemyProjectileSpeed = 7f;
    private int enemyScoreReward = 100;
    
    // Enemy Spawner Settings
    private int spawnerMaxAliveEnemies = 5;
    private float spawnerIntervalMin = 2.5f;
    private float spawnerIntervalMax = 4.5f;
    private float spawnerAheadMin = 10f;
    private float spawnerAheadMax = 24f;
    private float spawnerMinSpawnX = 8f;
    private float spawnerMaxSpawnX = 105f;
    private float spawnerGroundY = -3f;
    
    // Combo System Settings
    private float comboTimeWindow = 3f;
    private int comboKillThreshold = 5;
    private float comboDamageMultiplierPerLevel = 0.1f;
    private int comboMaxLevel = 10;
    
    // Screen Shake Settings
    private float shakeDuration = 0.5f;
    private float shakeMagnitude = 0.3f;
    private float shakeDampingSpeed = 1f;
    
    // Level Settings
    private float portalXPosition = 100f;
    private float portalYPosition = -2f;
    private int totalLevels = 3;
    private int targetScore = 1000;
    
    // Drop Rates
    private float coinDropRate = 0.5f;
    private float healthDropRate = 0.2f;
    private float weaponDropRate = 0.1f;
    
    // UI Settings
    private bool showComboUI = true;
    private bool showDamageNumbers = true;
    private bool showAchievementNotifications = true;

    [MenuItem("Tools/Soul Knight/Game Settings", false, 0)]
    public static void ShowWindow()
    {
        var window = GetWindow<SoulKnightGameSettings>("Soul Knight Settings");
        window.minSize = new Vector2(500, 600);
    }

    [MenuItem("Tools/Soul Knight/Quick Setup Scene", false, 1)]
    public static void QuickSetupScene()
    {
        SoulKnightSceneSetup.SetupScene();
    }

    void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        
        // Header
        DrawHeader();
        
        // Tabs
        DrawTabs();
        
        // Content based on tab
        switch (_currentTab)
        {
            case 0: DrawPlayerSettings(); break;
            case 1: DrawCombatSettings(); break;
            case 2: DrawEnemySettings(); break;
            case 3: DrawSpawnerSettings(); break;
            case 4: DrawCameraSettings(); break;
            case 5: DrawComboSettings(); break;
            case 6: DrawLevelSettings(); break;
            case 7: DrawVisualSettings(); break;
            case 8: DrawApplySettings(); break;
        }
        
        EditorGUILayout.EndScrollView();
    }

    private void DrawHeader()
    {
        EditorGUILayout.Space(10);
        
        var headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 18,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = new Color(1f, 0.8f, 0.2f) }
        };
        
        EditorGUILayout.LabelField("⚔️ SOUL KNIGHT - GAME SETTINGS ⚔️", headerStyle);
        
        var subStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 11,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.gray }
        };
        
        EditorGUILayout.LabelField("Complete configuration tool for all game parameters", subStyle);
        
        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox("Configure your game settings below and click 'Apply All Settings' to update prefabs and scene objects.", MessageType.Info);
        EditorGUILayout.Space(10);
    }

    private void DrawTabs()
    {
        string[] tabs = new string[] 
        { 
            "👤 Player", 
            "⚔️ Combat", 
            "👹 Enemy", 
            "📍 Spawner",
            "📷 Camera",
            "🔥 Combo",
            "🗺️ Level",
            "✨ Visual",
            "💾 Apply"
        };
        
        _currentTab = GUILayout.Toolbar(_currentTab, tabs, GUILayout.Height(30));
        EditorGUILayout.Space(10);
    }

    private void DrawPlayerSettings()
    {
        EditorGUILayout.LabelField("Player Movement & Health", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        playerSpeed = DrawFloatField("Movement Speed", playerSpeed, 1f, 15f, "How fast the player moves");
        playerJumpForce = DrawFloatField("Jump Force", playerJumpForce, 5f, 20f, "Height of player jump");
        playerCrouchSpeed = DrawFloatField("Crouch Speed", playerCrouchSpeed, 1f, 5f, "Speed while crouching");
        playerMaxHealth = DrawIntField("Max Health", playerMaxHealth, 1, 10, "Maximum player health points");
        playerInvincibilityTime = DrawFloatField("Invincibility Time", playerInvincibilityTime, 0.5f, 3f, "Seconds of invincibility after hit");
        playerScale = DrawVector3Field("Player Scale", playerScale, "Visual size of player");
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawCombatSettings()
    {
        EditorGUILayout.LabelField("Player Attack", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        playerFireRate = DrawFloatField("Fire Rate", playerFireRate, 0.1f, 2f, "Time between shots (seconds)");
        playerMeleeDamage = DrawIntField("Melee Damage", playerMeleeDamage, 1, 10, "Damage dealt by melee attack");
        playerMeleeCooldown = DrawFloatField("Melee Cooldown", playerMeleeCooldown, 0.1f, 2f, "Time between melee attacks");
        playerMeleeRange = DrawFloatField("Melee Range", playerMeleeRange, 0.5f, 3f, "Radius of melee attack");
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Bullet Properties", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        bulletSpeed = DrawFloatField("Bullet Speed", bulletSpeed, 5f, 30f, "Speed of projectile");
        bulletDamage = DrawIntField("Bullet Damage", bulletDamage, 1, 10, "Damage per bullet");
        bulletLifetime = DrawFloatField("Bullet Lifetime", bulletLifetime, 1f, 10f, "How long bullets exist before disappearing");
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawEnemySettings()
    {
        EditorGUILayout.LabelField("Enemy Base Stats", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        enemyMaxHealth = DrawIntField("Max Health", enemyMaxHealth, 1, 20, "Enemy health points");
        enemyDetectionRange = DrawFloatField("Detection Range", enemyDetectionRange, 3f, 15f, "Distance at which enemy spots player");
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
        enemyPatrolSpeed = DrawFloatField("Patrol Speed", enemyPatrolSpeed, 0.5f, 5f, "Speed while patrolling");
        enemyChaseSpeed = DrawFloatField("Chase Speed", enemyChaseSpeed, 1f, 8f, "Speed while chasing player");
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Combat", EditorStyles.boldLabel);
        enemyAttackRange = DrawFloatField("Melee Attack Range", enemyAttackRange, 0.5f, 3f, "Range for close combat");
        enemyAttackDamage = DrawIntField("Attack Damage", enemyAttackDamage, 1, 5, "Damage dealt to player");
        enemyAttackCooldown = DrawFloatField("Attack Cooldown", enemyAttackCooldown, 0.5f, 3f, "Time between attacks");
        
        enemyRangedAttackRange = DrawFloatField("Ranged Attack Range", enemyRangedAttackRange, 3f, 15f, "Range for projectile attack");
        enemyRangedAttackCooldown = DrawFloatField("Ranged Cooldown", enemyRangedAttackCooldown, 1f, 5f, "Time between ranged attacks");
        enemyProjectileSpeed = DrawFloatField("Projectile Speed", enemyProjectileSpeed, 3f, 15f, "Speed of enemy projectiles");
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Rewards", EditorStyles.boldLabel);
        enemyScoreReward = DrawIntField("Score Reward", enemyScoreReward, 10, 500, "Points awarded on kill");
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawSpawnerSettings()
    {
        EditorGUILayout.LabelField("Enemy Spawner Configuration", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        spawnerMaxAliveEnemies = DrawIntField("Max Alive Enemies", spawnerMaxAliveEnemies, 1, 20, "Maximum enemies active at once");
        spawnerIntervalMin = DrawFloatField("Spawn Interval Min", spawnerIntervalMin, 0.5f, 5f, "Minimum time between spawns");
        spawnerIntervalMax = DrawFloatField("Spawn Interval Max", spawnerIntervalMax, 1f, 10f, "Maximum time between spawns");
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Spawn Position", EditorStyles.boldLabel);
        spawnerAheadMin = DrawFloatField("Spawn Ahead Min", spawnerAheadMin, 5f, 20f, "Minimum distance ahead of player");
        spawnerAheadMax = DrawFloatField("Spawn Ahead Max", spawnerAheadMax, 10f, 50f, "Maximum distance ahead of player");
        spawnerMinSpawnX = DrawFloatField("Min Spawn X", spawnerMinSpawnX, 0f, 20f, "Left boundary of spawn area");
        spawnerMaxSpawnX = DrawFloatField("Max Spawn X", spawnerMaxSpawnX, 50f, 150f, "Right boundary of spawn area");
        spawnerGroundY = DrawFloatField("Ground Y Position", spawnerGroundY, -10f, 5f, "Y position where enemies spawn");
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawCameraSettings()
    {
        EditorGUILayout.LabelField("Camera Follow Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        cameraSmoothTime = DrawFloatField("Smooth Time", cameraSmoothTime, 0.05f, 1f, "How smoothly camera follows player");
        cameraOffset = DrawVector3Field("Camera Offset", cameraOffset, "Position offset from player");
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Camera Boundaries", EditorStyles.boldLabel);
        cameraMinX = DrawFloatField("Min X", cameraMinX, -50f, 0f, "Left camera limit");
        cameraMaxX = DrawFloatField("Max X", cameraMaxX, 50f, 200f, "Right camera limit");
        cameraMinY = DrawFloatField("Min Y", cameraMinY, -20f, 0f, "Bottom camera limit");
        cameraMaxY = DrawFloatField("Max Y", cameraMaxY, 0f, 20f, "Top camera limit");
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawComboSettings()
    {
        EditorGUILayout.LabelField("Combo System", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        comboTimeWindow = DrawFloatField("Combo Time Window", comboTimeWindow, 1f, 10f, "Time to maintain combo (seconds)");
        comboKillThreshold = DrawIntField("Kill Threshold", comboKillThreshold, 3, 20, "Kills needed to start combo");
        comboDamageMultiplierPerLevel = DrawFloatField("Damage Multiplier / Level", comboDamageMultiplierPerLevel, 0.05f, 0.3f, "Bonus damage per combo level");
        comboMaxLevel = DrawIntField("Max Combo Level", comboMaxLevel, 5, 50, "Maximum combo multiplier");
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Screen Shake", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        shakeDuration = DrawFloatField("Shake Duration", shakeDuration, 0.1f, 2f, "How long shake lasts");
        shakeMagnitude = DrawFloatField("Shake Magnitude", shakeMagnitude, 0.1f, 1f, "Intensity of shake");
        shakeDampingSpeed = DrawFloatField("Damping Speed", shakeDampingSpeed, 0.5f, 5f, "How quickly shake fades");
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawLevelSettings()
    {
        EditorGUILayout.LabelField("Level Configuration", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        portalXPosition = DrawFloatField("Portal X Position", portalXPosition, 50f, 200f, "Where portal appears (end of level)");
        portalYPosition = DrawFloatField("Portal Y Position", portalYPosition, -10f, 10f, "Portal vertical position");
        totalLevels = DrawIntField("Total Levels", totalLevels, 1, 10, "Number of levels in game");
        targetScore = DrawIntField("Target Score for Win", targetScore, 100, 10000, "Score needed to win game");
        
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Item Drop Rates", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        coinDropRate = DrawFloatField("Coin Drop Rate", coinDropRate, 0f, 1f, "Chance to drop coin (0-1)");
        healthDropRate = DrawFloatField("Health Drop Rate", healthDropRate, 0f, 1f, "Chance to drop health pickup");
        weaponDropRate = DrawFloatField("Weapon Drop Rate", weaponDropRate, 0f, 1f, "Chance to drop weapon pickup");
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawVisualSettings()
    {
        EditorGUILayout.LabelField("UI & Visual Options", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        showComboUI = EditorGUILayout.Toggle("Show Combo UI", showComboUI);
        showDamageNumbers = EditorGUILayout.Toggle("Show Damage Numbers", showDamageNumbers);
        showAchievementNotifications = EditorGUILayout.Toggle("Show Achievement Notifications", showAchievementNotifications);
        
        EditorGUILayout.Space(15);
        EditorGUILayout.HelpBox("These settings control visual feedback elements. Disable any that don't fit your game style.", MessageType.None);
        
        EditorGUILayout.Space(10);
        DrawSectionDivider();
    }

    private void DrawApplySettings()
    {
        EditorGUILayout.LabelField("Apply Settings to Project", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox("Click the buttons below to apply your configured settings to the game scripts and prefabs.", MessageType.Info);
        
        EditorGUILayout.Space(15);
        
        var buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 14,
            fontWeight = FontWeights.Bold,
            fixedHeight = 40
        };
        
        if (GUILayout.Button("🎮 APPLY ALL SETTINGS TO SCRIPTS", buttonStyle, GUILayout.Height(50)))
        {
            ApplyAllSettings();
        }
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("🏗️ SETUP COMPLETE SCENE", buttonStyle, GUILayout.Height(50)))
        {
            SoulKnightSceneSetup.SetupScene();
        }
        
        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox("After applying settings, you may need to re-enter play mode for changes to take effect.", MessageType.Warning);
        
        EditorGUILayout.Space(20);
        DrawSectionDivider();
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Export / Import Presets", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Current Settings", GUILayout.Height(30)))
        {
            SaveSettingsPreset();
        }
        if (GUILayout.Button("Load Saved Settings", GUILayout.Height(30)))
        {
            LoadSettingsPreset();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ApplyAllSettings()
    {
        int updatedCount = 0;
        
        // Update PlayerMovement
        if (UpdateScriptValue("Assets/Scripts/PlayerMovement.cs", "speed =", $"speed = {playerSpeed}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/PlayerMovement.cs", "jumpForce =", $"jumpForce = {playerJumpForce}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/PlayerMovement.cs", "crouchSpeed =", $"crouchSpeed = {playerCrouchSpeed}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/PlayerMovement.cs", "playerScale =", $"playerScale = new Vector3({playerScale.x}f, {playerScale.y}f, {playerScale.z}f)"))
            updatedCount++;
        
        // Update PlayerHealth
        if (UpdateScriptValue("Assets/Scripts/PlayerHealth.cs", "maxHealth =", $"maxHealth = {playerMaxHealth}"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/PlayerHealth.cs", "invincibilityTime =", $"invincibilityTime = {playerInvincibilityTime}f"))
            updatedCount++;
        
        // Update PlayerAttack
        if (UpdateScriptValue("Assets/Scripts/PlayerAttack.cs", "fireRate =", $"fireRate = {playerFireRate}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/PlayerAttack.cs", "meleeDamage =", $"meleeDamage = {playerMeleeDamage}"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/PlayerAttack.cs", "meleeCooldown =", $"meleeCooldown = {playerMeleeCooldown}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/PlayerAttack.cs", "meleeRange =", $"meleeRange = {playerMeleeRange}f"))
            updatedCount++;
        
        // Update Bullet
        if (UpdateScriptValue("Assets/Scripts/Bullet.cs", "speed =", $"speed = {bulletSpeed}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Bullet.cs", "damage =", $"damage = {bulletDamage}"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Bullet.cs", "lifetime =", $"lifetime = {bulletLifetime}f"))
            updatedCount++;
        
        // Update CameraFollow
        if (UpdateScriptValue("Assets/Scripts/CameraFollow.cs", "smoothTime =", $"smoothTime = {cameraSmoothTime}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/CameraFollow.cs", "offset =", $"offset = new Vector3({cameraOffset.x}f, {cameraOffset.y}f, {cameraOffset.z}f)"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/CameraFollow.cs", "minX =", $"minX = {cameraMinX}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/CameraFollow.cs", "maxX =", $"maxX = {cameraMaxX}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/CameraFollow.cs", "minY =", $"minY = {cameraMinY}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/CameraFollow.cs", "maxY =", $"maxY = {cameraMaxY}f"))
            updatedCount++;
        
        // Update Enemy
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "maxHealth =", $"maxHealth = {enemyMaxHealth}"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "detectionRange =", $"detectionRange = {enemyDetectionRange}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "patrolSpeed =", $"patrolSpeed = {enemyPatrolSpeed}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "chaseSpeed =", $"chaseSpeed = {enemyChaseSpeed}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "attackRange =", $"attackRange = {enemyAttackRange}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "attackDamage =", $"attackDamage = {enemyAttackDamage}"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "attackCooldown =", $"attackCooldown = {enemyAttackCooldown}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "rangedAttackRange =", $"rangedAttackRange = {enemyRangedAttackRange}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "rangedAttackCooldown =", $"rangedAttackCooldown = {enemyRangedAttackCooldown}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "projectileSpeed =", $"projectileSpeed = {enemyProjectileSpeed}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "scoreReward =", $"scoreReward = {enemyScoreReward}"))
            updatedCount++;
        
        // Update EnemySpawner
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "maxAliveEnemies =", $"maxAliveEnemies = {spawnerMaxAliveEnemies}"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "spawnIntervalMin =", $"spawnIntervalMin = {spawnerIntervalMin}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "spawnIntervalMax =", $"spawnIntervalMax = {spawnerIntervalMax}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "spawnAheadMin =", $"spawnAheadMin = {spawnerAheadMin}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "spawnAheadMax =", $"spawnAheadMax = {spawnerAheadMax}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "minSpawnX =", $"minSpawnX = {spawnerMinSpawnX}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "maxSpawnX =", $"maxSpawnX = {spawnerMaxSpawnX}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/EnemySpawner.cs", "groundY =", $"groundY = {spawnerGroundY}f"))
            updatedCount++;
        
        // Update ComboSystem
        if (UpdateScriptValue("Assets/Scripts/ComboSystem.cs", "comboTimeWindow =", $"comboTimeWindow = {comboTimeWindow}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/ComboSystem.cs", "comboKillThreshold =", $"comboKillThreshold = {comboKillThreshold}"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/ComboSystem.cs", "damageMultiplierPerCombo =", $"damageMultiplierPerCombo = {comboDamageMultiplierPerLevel}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/ComboSystem.cs", "maxComboLevel =", $"maxComboLevel = {comboMaxLevel}"))
            updatedCount++;
        
        // Update ScreenShake
        if (UpdateScriptValue("Assets/Scripts/ScreenShake.cs", "shakeDuration =", $"shakeDuration = {shakeDuration}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/ScreenShake.cs", "shakeMagnitude =", $"shakeMagnitude = {shakeMagnitude}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/ScreenShake.cs", "dampingSpeed =", $"dampingSpeed = {shakeDampingSpeed}f"))
            updatedCount++;
        
        // Update LevelManager
        if (UpdateScriptValue("Assets/Scripts/LevelManager.cs", "portalXPosition =", $"portalXPosition = {portalXPosition}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/LevelManager.cs", "portalYPosition =", $"portalYPosition = {portalYPosition}f"))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/LevelManager.cs", "totalLevels =", $"totalLevels = {totalLevels}"))
            updatedCount++;
        
        // Update GameManager
        if (UpdateScriptValue("Assets/Scripts/GameManager.cs", "targetScore =", $"targetScore = {targetScore}"))
            updatedCount++;
        
        // Update Enemy drop rates
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "Random.value <", $"Random.value < {coinDropRate}f", 0))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "Random.value <", $"Random.value < {healthDropRate}f", 1))
            updatedCount++;
        if (UpdateScriptValue("Assets/Scripts/Enemy.cs", "Random.value <", $"Random.value < {weaponDropRate}f", 2))
            updatedCount++;
        
        EditorUtility.DisplayDialog(
            "Settings Applied!", 
            $"Successfully updated {updatedCount} configuration values across all scripts!\n\nYour game is now configured with the new settings.", 
            "OK"
        );
        
        Debug.Log($"✅ Applied {updatedCount} settings to game scripts");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private bool UpdateScriptValue(string scriptPath, string searchPattern, string replacement, int occurrence = 0)
    {
        string fullPath = Path.Combine(Application.dataPath, "..", scriptPath);
        
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"Script not found: {fullPath}");
            return false;
        }
        
        try
        {
            string content = File.ReadAllText(fullPath);
            
            // Find the line containing the pattern
            string[] lines = content.Split('\n');
            int foundCount = 0;
            
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(searchPattern))
                {
                    if (foundCount == occurrence)
                    {
                        // Replace the value part
                        int equalsIndex = lines[i].IndexOf('=');
                        if (equalsIndex > 0)
                        {
                            string beforeEquals = lines[i].Substring(0, equalsIndex + 1);
                            lines[i] = beforeEquals + " " + replacement.Substring(replacement.IndexOf('=') + 1).Trim();
                            break;
                        }
                    }
                    foundCount++;
                }
            }
            
            string newContent = string.Join("\n", lines);
            
            if (content != newContent)
            {
                File.WriteAllText(fullPath, newContent);
                Debug.Log($"✓ Updated: {scriptPath} - {searchPattern.Trim()}");
                return true;
            }
            
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error updating {scriptPath}: {e.Message}");
            return false;
        }
    }

    private void SaveSettingsPreset()
    {
        string json = JsonUtility.ToJson(new SettingsData
        {
            playerSpeed = playerSpeed,
            playerJumpForce = playerJumpForce,
            playerMaxHealth = playerMaxHealth,
            enemyMaxHealth = enemyMaxHealth,
            enemyDetectionRange = enemyDetectionRange,
            bulletSpeed = bulletSpeed,
            bulletDamage = bulletDamage
        }, true);
        
        string path = EditorUtility.SaveFilePanel("Save Settings Preset", "", "soul_knight_settings", "json");
        
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            EditorUtility.DisplayDialog("Settings Saved", $"Settings saved to:\n{path}", "OK");
        }
    }

    private void LoadSettingsPreset()
    {
        string path = EditorUtility.OpenFilePanel("Load Settings Preset", "", "json");
        
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<SettingsData>(json);
                
                playerSpeed = data.playerSpeed;
                playerJumpForce = data.playerJumpForce;
                playerMaxHealth = data.playerMaxHealth;
                enemyMaxHealth = data.enemyMaxHealth;
                enemyDetectionRange = data.enemyDetectionRange;
                bulletSpeed = data.bulletSpeed;
                bulletDamage = data.bulletDamage;
                
                EditorUtility.DisplayDialog("Settings Loaded", "Settings preset loaded successfully!", "OK");
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", $"Failed to load settings: {e.Message}", "OK");
            }
        }
    }

    // Helper drawing methods
    private float DrawFloatField(string label, float value, float min, float max, string tooltip = "")
    {
        EditorGUILayout.BeginHorizontal();
        GUIContent content = new GUIContent(label, tooltip);
        value = EditorGUILayout.FloatField(content, value);
        value = Mathf.Clamp(value, min, max);
        EditorGUILayout.EndHorizontal();
        return value;
    }

    private int DrawIntField(string label, int value, int min, int max, string tooltip = "")
    {
        EditorGUILayout.BeginHorizontal();
        GUIContent content = new GUIContent(label, tooltip);
        value = EditorGUILayout.IntField(content, value);
        value = Mathf.Clamp(value, min, max);
        EditorGUILayout.EndHorizontal();
        return value;
    }

    private Vector3 DrawVector3Field(string label, Vector3 value, string tooltip = "")
    {
        GUIContent content = new GUIContent(label, tooltip);
        return EditorGUILayout.Vector3Field(content, value);
    }

    private void DrawSectionDivider()
    {
        EditorGUILayout.Space(5);
        Rect rect = EditorGUILayout.GetControlRect(false, 2);
        EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f, 0.5f));
        EditorGUILayout.Space(5);
    }

    [System.Serializable]
    private class SettingsData
    {
        public float playerSpeed;
        public float playerJumpForce;
        public int playerMaxHealth;
        public int enemyMaxHealth;
        public float enemyDetectionRange;
        public float bulletSpeed;
        public int bulletDamage;
    }
}
