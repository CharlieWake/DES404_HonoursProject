using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Basic Stats")]
    public string playerClass;
    public float maxHealth;
    public int actionsPerTurn;
    public float experienceToLevelUp;
    public int playerLevel;

    [Header("Damage")]
    public int minDamage;
    public int maxDamage;

    public float GetRandomDamage()
    {
        return Random.Range(minDamage, maxDamage);
    }
}
