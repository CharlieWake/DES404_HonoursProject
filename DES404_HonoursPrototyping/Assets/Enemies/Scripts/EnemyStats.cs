using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyStats : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyData enemyData;
    SpriteRenderer spriteRenderer;    
    Transform enemyCanvas;
    GameObject floatingTextPrefab;
    FloatingHealthBar healthBar;
    GameObject expGemPrefab;
    [SerializeField] private Light2D selfLight;

    [Header("Stats")]
    [SerializeField] private float currentHealth;
    [SerializeField] private int actionsPerTurn;    

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>(true);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyCanvas = transform.Find("EnemyCanvas");

        if (enemyCanvas == null)
        {
            Debug.LogError("Could not find EnemyCanvas");
        }

        floatingTextPrefab = Resources.Load<GameObject>("DamageText/FloatingDamageText");

        if (floatingTextPrefab == null)
        {
            Debug.LogError("FloatingDamageTextPrefab could not be loaded from Resources!");
        }

        expGemPrefab = Resources.Load<GameObject>("ExpGems/ExperienceGem");

        if (expGemPrefab == null)
        {
            Debug.LogError("ExperienceGemPrefab could not be loaded from Resources!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyData.maxHealth;
        healthBar.UpdateHealthBar(currentHealth, enemyData.maxHealth);
        actionsPerTurn = enemyData.actionsPerTurn;
        // selfLight = GetComponentInChildren<Light>();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.UpdateHealthBar(currentHealth, enemyData.maxHealth);
        StartCoroutine(DamageFlash(spriteRenderer));
        FloatingDamageText(damageAmount.ToString(), transform.position);
        if (currentHealth <= 0)
        {
            Death();
        }        
    }

    private void Death()
    {
        healthBar.gameObject.SetActive(false);
        StartCoroutine(DeathEvent());
        //GetComponent<EnemyBehaviour>().RemoveEnemy();
        // GameManager.instance.playerStats.AddExperience(enemyData.experienceToGive);
        //Destroy(gameObject);
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
        GameObject textObject = Instantiate(floatingTextPrefab, worldPosition, Quaternion.identity, enemyCanvas);
        textObject.GetComponent<FloatingDamageText>().setText(damageAmount);
    }

    IEnumerator DeathEvent()
    {               
        yield return StartCoroutine(FadeOutEnemy(1f));

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(transform.position);

        for (int i = 0; i < enemyData.experienceToGive; i++)
        {
            GameObject expGem = Instantiate(expGemPrefab, spawnPosition, Quaternion.identity, GameObject.Find("HUD").transform);
            expGem.GetComponent<ExpGemScript>().Initialize(spawnPosition);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }

        GetComponent<EnemyBehaviour>().UnregisterEnemy();
        Destroy(gameObject);
    }

    IEnumerator FadeOutEnemy(float fadeDuration)
    {
        float fadeElapsed = 0f;
        Color originalColor = spriteRenderer.color;
        float initialLightIntensity = selfLight != null ? selfLight.intensity : 0f;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float t = fadeElapsed / fadeDuration;

            float alpha = Mathf.Lerp(1f, 0f, t);
            Color newColor = originalColor;
            newColor.a = alpha;
            spriteRenderer.color = newColor;

            if (selfLight != null)
            {
                selfLight.intensity = Mathf.Lerp(initialLightIntensity, 0f, t);
            }

            yield return null;
        }

        
        Color finalColor = originalColor;
        finalColor.a = 0f;
        spriteRenderer.color = finalColor;
        if (selfLight != null)
        {
            selfLight.intensity = 0f;
        }
    }

    public float GetDamageAmount()
    {
        return enemyData.GetRandomDamage();
    }
}
