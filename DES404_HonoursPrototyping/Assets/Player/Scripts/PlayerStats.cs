using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    public int actionsPerTurn;
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
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
        Destroy(gameObject);
    }

}
