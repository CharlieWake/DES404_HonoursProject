using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private float maxHealth = 10;
    private float currentHealth;

    public int currentExperience;
    private int experienceToLevelUp;

    private int playerLevel;

    public int actionsPerTurn;

    public Image healthBar;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;        
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

}
