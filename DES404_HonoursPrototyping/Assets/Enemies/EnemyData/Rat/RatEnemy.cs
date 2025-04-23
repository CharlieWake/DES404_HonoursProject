using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemy : EnemyBehaviour
{
    public override IEnumerator TakeTurn()
    {
        if (IsPlayerAdjacent())
        {
            yield return MeleeAttack();
        }
        else
        {
            yield return MoveTowardsPlayer();
        }

        yield return new WaitForSeconds(1f);
    }
}