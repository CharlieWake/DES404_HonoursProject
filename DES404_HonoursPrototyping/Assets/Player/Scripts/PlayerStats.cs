using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{

    private SpriteRenderer spriteRender;
    [SerializeField] private float maxHealth = 10;
    private float currentHealth;

    public float currentExperience;
    private float experienceToLevelUp = 100;

    private int playerLevel;

    public int actionsPerTurn;

    public Image healthBar;
    public Image XPBar;

    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform worldCanvas;


    private void Awake()
    {
        spriteRender = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText(currentHealth, maxHealth);
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        XPBar.fillAmount = currentExperience / experienceToLevelUp;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        StartCoroutine(DamageFlash(spriteRender));
        FloatingDamageText(damageAmount.ToString(), transform.position);
        UpdateHealthText(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
        else
        {
            Debug.Log("Ouch, my remaining health is " + currentHealth);
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
        GameObject textObject = Instantiate(floatingTextPrefab, worldPosition, Quaternion.identity, worldCanvas);
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
