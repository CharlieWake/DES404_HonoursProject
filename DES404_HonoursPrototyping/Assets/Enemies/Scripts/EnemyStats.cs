using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int health = 10;
    public int actionsPerTurn = 1;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        
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
