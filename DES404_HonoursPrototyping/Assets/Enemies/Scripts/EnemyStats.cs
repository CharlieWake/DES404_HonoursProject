using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] FloatingHealthBar healthBar;
    private float health = 10;
    public int actionsPerTurn = 1;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
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
        Destroy(gameObject);
    }
}
