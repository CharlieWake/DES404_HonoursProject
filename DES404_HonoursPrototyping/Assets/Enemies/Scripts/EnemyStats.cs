using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] FloatingHealthBar healthBar;
    private float health = 10;
    public int actionsPerTurn = 1;

    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform worldCanvas;

    [SerializeField] private PlayerStats playerStats;

    public float xpToGive;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        StartCoroutine(DamageFlash(spriteRenderer));
        FloatingDamageText(damageAmount.ToString(), transform.position);
        if (health <= 0)
        {
            Death();
        }
        else
        {
            Debug.Log("Ouch, my remaining health is " + health);
        }
    }

    private void Death()
    {
        Debug.Log("Now I am slain...");
        GetComponent<EnemyMovement>().RemoveEnemy();
        playerStats.AddXP(xpToGive);
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
        GameObject textObject = Instantiate(floatingTextPrefab, worldPosition, Quaternion.identity, worldCanvas);
        textObject.GetComponent<FloatingDamageText>().setText(damageAmount);
    }
}
