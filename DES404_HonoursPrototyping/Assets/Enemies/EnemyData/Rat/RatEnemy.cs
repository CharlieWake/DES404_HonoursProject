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

    private IEnumerator MoveTowardsPlayer()
    {
        Vector3Int startPosition = currentGridPosition;
        Vector3Int targetPosition = gridManager.grid.WorldToCell(playerController.transform.position);

        List<Vector3Int> path = pathfinding.FindPath(startPosition, targetPosition);

        if (path != null && path.Count > 0)
        {
            Vector3Int nextStep = path[0];

            gridManager.SetTileAsOccupied(currentGridPosition, false);

            if (gridManager.IsTileWalkable(nextStep) && !gridManager.IsTileOccupied(nextStep))
            {
                gridManager.SetTileAsOccupied(nextStep, true);
                
                yield return MoveToNextTile(nextStep);
            }
            else
            {
                gridManager.SetTileAsOccupied(currentGridPosition, true);
                Debug.Log("Next step is blocked!");
            }
        }
        else
        {
            Debug.Log("No path to player!");
        }
    }
}