using UnityEngine;

public class Achievements : MonoBehaviour
{
    [Header("Achievement Data")]
    public int totalKills = 0;
    public int totalCoinsCollected = 0;
    public int levelsCompleted = 0;
    public int maxCombo = 0;
    public int damageTaken = 0;
    public int itemsCollected = 0;
    
    [Header("Achievements Unlocked")]
    public bool firstBlood = false;
    public bool killer = false;
    public bool slayer = false;
    public bool godOfDeath = false;
    public bool coinLover = false;
    public bool rich = false;
    public bool explorer = false;
    public bool worldTraveler = false;
    public bool comboMaster = false;
    public bool comboLegend = false;
    public bool survivor = false;
    public bool collector = false;
    
    public static Achievements Instance { get; private set; }
    
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
        
        LoadAchievements();
    }
    
    public void AddKill()
    {
        totalKills++;
        
        if (!firstBlood && totalKills >= 1)
        {
            UnlockAchievement("firstBlood", "First Blood!", "Убить первого врага");
        }
        
        if (!killer && totalKills >= 50)
        {
            UnlockAchievement("killer", "Killer", "Убить 50 врагов");
        }
        
        if (!slayer && totalKills >= 200)
        {
            UnlockAchievement("slayer", "Slayer", "Убить 200 врагов");
        }
        
        if (!godOfDeath && totalKills >= 1000)
        {
            UnlockAchievement("godOfDeath", "God of Death", "Убить 1000 врагов");
        }
    }
    
    public void AddCoin(int amount = 1)
    {
        totalCoinsCollected += amount;
        
        if (!coinLover && totalCoinsCollected >= 100)
        {
            UnlockAchievement("coinLover", "Coin Lover", "Собрать 100 монет");
        }
        
        if (!rich && totalCoinsCollected >= 1000)
        {
            UnlockAchievement("rich", "Rich", "Собрать 1000 монет");
        }
    }
    
    public void CompleteLevel()
    {
        levelsCompleted++;
        
        if (!explorer && levelsCompleted >= 1)
        {
            UnlockAchievement("explorer", "Explorer", "Пройти первый уровень");
        }
        
        if (!worldTraveler && levelsCompleted >= 3)
        {
            UnlockAchievement("worldTraveler", "World Traveler", "Пройти все уровни");
        }
    }
    
    public void UpdateCombo(int combo)
    {
        if (combo > maxCombo)
        {
            maxCombo = combo;
            
            if (!comboMaster && maxCombo >= 15)
            {
                UnlockAchievement("comboMaster", "Combo Master", "Достичь комбо x15");
            }
            
            if (!comboLegend && maxCombo >= 30)
            {
                UnlockAchievement("comboLegend", "Combo Legend", "Достичь комбо x30");
            }
        }
    }
    
    public void TakeDamage(int amount)
    {
        damageTaken += amount;
        
        if (!survivor && damageTaken >= 500)
        {
            UnlockAchievement("survivor", "Survivor", "Получить 500 урона");
        }
    }
    
    public void CollectItem()
    {
        itemsCollected++;
        
        if (!collector && itemsCollected >= 50)
        {
            UnlockAchievement("collector", "Collector", "Собрать 50 предметов");
        }
    }
    
    private void UnlockAchievement(string achievementName, string title, string description)
    {
        Debug.Log($"🏆 ACHIEVEMENT UNLOCKED: {title} - {description}");
        
        // Здесь можно добавить UI уведомление
        ShowAchievementNotification(title, description);
        
        SaveAchievements();
    }
    
    private void ShowAchievementNotification(string title, string description)
    {
        // Простое уведомление в консоль
        // В полной игре здесь должно быть красивое UI окно
        Debug.Log($"===========================================");
        Debug.Log($"🏆 {title.ToUpper()}");
        Debug.Log($"   {description}");
        Debug.Log($"===========================================");
    }
    
    public void SaveAchievements()
    {
        PlayerPrefs.SetInt("TotalKills", totalKills);
        PlayerPrefs.SetInt("TotalCoins", totalCoinsCollected);
        PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
        PlayerPrefs.SetInt("MaxCombo", maxCombo);
        PlayerPrefs.SetInt("DamageTaken", damageTaken);
        PlayerPrefs.SetInt("ItemsCollected", itemsCollected);
        
        PlayerPrefs.SetInt("FirstBlood", firstBlood ? 1 : 0);
        PlayerPrefs.SetInt("Killer", killer ? 1 : 0);
        PlayerPrefs.SetInt("Slayer", slayer ? 1 : 0);
        PlayerPrefs.SetInt("GodOfDeath", godOfDeath ? 1 : 0);
        PlayerPrefs.SetInt("CoinLover", coinLover ? 1 : 0);
        PlayerPrefs.SetInt("Rich", rich ? 1 : 0);
        PlayerPrefs.SetInt("Explorer", explorer ? 1 : 0);
        PlayerPrefs.SetInt("WorldTraveler", worldTraveler ? 1 : 0);
        PlayerPrefs.SetInt("ComboMaster", comboMaster ? 1 : 0);
        PlayerPrefs.SetInt("ComboLegend", comboLegend ? 1 : 0);
        PlayerPrefs.SetInt("Survivor", survivor ? 1 : 0);
        PlayerPrefs.SetInt("Collector", collector ? 1 : 0);
        
        PlayerPrefs.Save();
    }
    
    public void LoadAchievements()
    {
        totalKills = PlayerPrefs.GetInt("TotalKills", 0);
        totalCoinsCollected = PlayerPrefs.GetInt("TotalCoins", 0);
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted", 0);
        maxCombo = PlayerPrefs.GetInt("MaxCombo", 0);
        damageTaken = PlayerPrefs.GetInt("DamageTaken", 0);
        itemsCollected = PlayerPrefs.GetInt("ItemsCollected", 0);
        
        firstBlood = PlayerPrefs.GetInt("FirstBlood", 0) == 1;
        killer = PlayerPrefs.GetInt("Killer", 0) == 1;
        slayer = PlayerPrefs.GetInt("Slayer", 0) == 1;
        godOfDeath = PlayerPrefs.GetInt("GodOfDeath", 0) == 1;
        coinLover = PlayerPrefs.GetInt("CoinLover", 0) == 1;
        rich = PlayerPrefs.GetInt("Rich", 0) == 1;
        explorer = PlayerPrefs.GetInt("Explorer", 0) == 1;
        worldTraveler = PlayerPrefs.GetInt("WorldTraveler", 0) == 1;
        comboMaster = PlayerPrefs.GetInt("ComboMaster", 0) == 1;
        comboLegend = PlayerPrefs.GetInt("ComboLegend", 0) == 1;
        survivor = PlayerPrefs.GetInt("Survivor", 0) == 1;
        collector = PlayerPrefs.GetInt("Collector", 0) == 1;
    }
    
    public void ResetAchievements()
    {
        PlayerPrefs.DeleteAll();
        totalKills = 0;
        totalCoinsCollected = 0;
        levelsCompleted = 0;
        maxCombo = 0;
        damageTaken = 0;
        itemsCollected = 0;
        
        firstBlood = false;
        killer = false;
        slayer = false;
        godOfDeath = false;
        coinLover = false;
        rich = false;
        explorer = false;
        worldTraveler = false;
        comboMaster = false;
        comboLegend = false;
        survivor = false;
        collector = false;
        
        Debug.Log("🔄 All achievements reset!");
    }
}
