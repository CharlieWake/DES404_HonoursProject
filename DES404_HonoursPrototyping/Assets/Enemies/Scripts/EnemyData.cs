using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHealth;
    public int actionsPerTurn;
    public int experienceToGive;

    [Header("Damage Range")]
    public int minDamage;
    public int maxDamage;

    public float GetRandomDamage()
    {
        return Random.Range(minDamage, maxDamage);
    }
}
