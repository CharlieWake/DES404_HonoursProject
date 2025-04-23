using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderEnemy : EnemyBehaviour
{
    private bool shouldShoot = false;

    public override IEnumerator TakeTurn()
    {
        if (shouldShoot && HasLineOfSightToPlayer(currentGridPosition, gridManager.grid.WorldToCell(playerController.transform.position)))
        {
            yield return RangedAttack(playerController.transform.position);
        }
        else
        {
            yield return MoveAwayFromPlayer();
        }

        shouldShoot = !shouldShoot;
        yield return new WaitForSeconds(1f);
    }
}
