using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerData playerData;
    SpriteRenderer spriteRenderer;
    GameObject floatingTextPrefab;
    Transform playerCanvas;

    private float currentHealth;
    private float currentExperience;    
    private int playerLevel;

    [Header("UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image experienceBar;
    [SerializeField] private TextMeshProUGUI healthText;
     
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerCanvas = transform.Find("PlayerCanvas");

        if (playerCanvas == null)
        {
            Debug.LogError("Could not find PlayerCanvas");
        }

        floatingTextPrefab = Resources.Load<GameObject>("DamageText/FloatingDamageText");

        if (floatingTextPrefab == null)
        {
            Debug.LogError("FloatingDamageTextPrefab could not be loaded from Resources!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = playerData.maxHealth;
        UpdateHealthText(currentHealth, playerData.maxHealth);
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / playerData.maxHealth;
        experienceBar.fillAmount = currentExperience / playerData.experienceToLevelUp;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        StartCoroutine(DamageFlash(spriteRenderer));
        FloatingDamageText(damageAmount.ToString(), transform.position);
        UpdateHealthText(currentHealth, playerData.maxHealth);

        if (currentHealth <= 0)
        {
            Death();
        }        
    }

    private void Death()
    {
        // Destroy(gameObject);
        Debug.Log("HP is zero, dead.");
    }

    IEnumerator DamageFlash(SpriteRenderer spriteRenderer)
    {
        Color originalColor = spriteRenderer.color;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void FloatingDamageText(string damageAmount, Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition + Vector3.up * 0.5f);
        GameObject textObject = Instantiate(floatingTextPrefab, worldPosition, Quaternion.identity, playerCanvas);
        textObject.GetComponent<FloatingDamageText>().setText(damageAmount);
    }

    void UpdateHealthText(float currentHealth, float maxHealth)
    {
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public void AddExperience(float experienceValue)
    {
        currentExperience += experienceValue;
    }

}
