using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Stat Data")]    
    [SerializeField] private PlayerLevelUpData playerLevelUpData;

    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image experienceBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private LevelUpText levelUpText;

    [Header("FloatingDamageText")]
    [SerializeField] private GameObject floatingTextPrefab;

    [Header("Settings")]
    [SerializeField] private string playerCanvasName = "PlayerCanvas";
    [SerializeField] private string healthBarObjectName = "HealthBar";

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private Transform playerCanvas;

    // Player State
    private float currentHealth;
    private float maxHealth;
    private float currentExperience = 0f;
    private int playerLevel = 1;
    private int actionsPerTurn;

    // Read-Only Properties
    public int Level => playerLevel;
    public float Health => currentHealth;
    public int ActionsPerTurn => actionsPerTurn;

    private void Awake()
    {               
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerCanvas = transform.Find(playerCanvasName);

        if (playerCanvas == null)
        {
            Debug.LogError("Could not find PlayerCanvas");
        }

        if (floatingTextPrefab == null)
        {
            Debug.LogError("FloatingDamageTextPrefab not assigned in Inspector");
        }
    }

    private void Start()
    {
        InitialiseStats();
    }

    public void InitialiseStats()
    {
        var levelInfo = playerLevelUpData.GetLevelInfo(playerLevel);
        if (levelInfo == null)
        {
            Debug.LogError("Level info not found during stat set up!");
        }

        maxHealth = levelInfo.maxHealth;
        currentHealth = maxHealth;
        actionsPerTurn = levelInfo.actionsPerTurn;

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        StartCoroutine(DamageFlash());
        ShowFloatingDamageText(damageAmount.ToString());

        TriggerHealthBarShake();

        UpdateUI();

        if (currentHealth <= 0)
        {
            HandleDeath();
        }    
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    public void AddExperience(float expAmount)
    {
        currentExperience += expAmount;
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        while (playerLevel < playerLevelUpData.maxLevel && currentExperience >= playerLevelUpData.GetLevelInfo(playerLevel).experienceToLevelUp)
        {
            var currentLevelInfo = playerLevelUpData.GetLevelInfo(playerLevel);
            currentExperience -= currentLevelInfo.experienceToLevelUp;                      
            
            float oldMaxHealth = maxHealth;
            int oldActions = actionsPerTurn;
            int oldLevel = playerLevel;

            playerLevel++;
            var newLevelInfo = playerLevelUpData.GetLevelInfo(playerLevel);

            float healthGain = newLevelInfo.maxHealth - maxHealth;
            maxHealth = newLevelInfo.maxHealth;
            Heal(healthGain);

            actionsPerTurn = newLevelInfo.actionsPerTurn;

            playerController.UpdateActionDots();

            levelUpText.ShowLevelUpText(oldLevel, playerLevel, oldMaxHealth, maxHealth, oldActions, actionsPerTurn);            
        }
    }

    private void HandleDeath()
    {
        Debug.Log("Player Has Died.");
    }

    IEnumerator DamageFlash()
    {
        Color originalColour = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColour;
    }

    void ShowFloatingDamageText(string damageAmount)
    {
        if (floatingTextPrefab == null || playerCanvas == null) return;

        Vector3 position = transform.position + Vector3.up * 0.5f;
        GameObject textObject = Instantiate(floatingTextPrefab, position, Quaternion.identity, playerCanvas);
        textObject.GetComponent<FloatingDamageText>().setText(damageAmount);
    }

    private void TriggerHealthBarShake()
    {
        GameObject healthBarObject = GameObject.Find(healthBarObjectName);
        if (healthBarObject != null && healthBarObject.TryGetComponent(out UIShake shakerScript))
        {
            shakerScript.TriggerShake(0.2f);
        }
    }       

    private void UpdateUI()
    {
        float expToLevelUp = playerLevelUpData.GetLevelInfo(playerLevel)?.experienceToLevelUp ?? 1f;
        healthBar.fillAmount = currentHealth / maxHealth;
        experienceBar.fillAmount = currentExperience / expToLevelUp;
        healthText.text = $"{Mathf.Ceil(currentHealth)} / {maxHealth}";
    }
}
