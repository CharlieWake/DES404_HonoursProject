using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyData enemyData;
    SpriteRenderer spriteRenderer;    
    Transform enemyCanvas;
    GameObject floatingTextPrefab;
    FloatingHealthBar healthBar;

    [Header("Stats")]
    [SerializeField] private float currentHealth;
    [SerializeField] private int actionsPerTurn;

    
    


    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
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
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyData.maxHealth;
        healthBar.UpdateHealthBar(currentHealth, enemyData.maxHealth);
        actionsPerTurn = enemyData.actionsPerTurn;        
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
        GetComponent<EnemyBehaviour>().RemoveEnemy();
        GameManager.instance.playerStats.AddExperience(enemyData.experienceToGive);
        Destroy(gameObject);
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
}
