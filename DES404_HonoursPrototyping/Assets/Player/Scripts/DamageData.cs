using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Data", menuName = "Player/Player Data")]
public class DamageData : ScriptableObject
{   
    [Header("Damage")]
    public int minDamage;
    public int maxDamage;

    public float GetRandomDamage()
    {
        return Random.Range(minDamage, maxDamage);
    }
}
