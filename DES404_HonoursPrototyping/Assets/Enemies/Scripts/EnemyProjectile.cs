using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float projectileLifetime = 3f;
    private float damage = 1;
    private GameObject projectileOwner;
        
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, projectileLifetime);
    }

    public void SetProjectileDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    public void SetOwner(GameObject sender)
    {
        projectileOwner = sender;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == projectileOwner) return;

        if (other.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}
