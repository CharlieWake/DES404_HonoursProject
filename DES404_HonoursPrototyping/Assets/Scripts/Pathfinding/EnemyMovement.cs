using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Transform playerCharacter;


    private float movementSpeed = 1f;
    [SerializeField] private float pauseBetweenTiles = 0.5f;
    [SerializeField] private int actionsPerTurn = 1;


    private Vector3Int enemyPosition;
    private List<Vector3Int> path = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.instance.FindAllEnemies(this);
        enemyPosition = floorTilemap.WorldToCell(transform.position);
        gridManager.SetTileAsOccupied(enemyPosition, true);
    }

    public IEnumerator TakeTurn()
    {
        int actionsRemaining = actionsPerTurn;

        while (actionsRemaining > 0)
        {
            // Check if the enemy is adjacent to the player and can attack
            if (gridManager.getAdjacentTiles(enemyPosition).Contains(floorTilemap.WorldToCell(playerCharacter.position)))
            {
                AttackPlayer();
                actionsRemaining--;
            }
            else
            {
                // If no path, recalculate path to player
                if (path == null || path.Count == 0)
                {
                    Vector3Int playerPosition = floorTilemap.WorldToCell(playerCharacter.position);
                    path = pathfinding.FindPath(enemyPosition, playerPosition);
                }

                // If there's a path to follow, check the first tile in the path
                if (path.Count > 0)
                {
                    // If the next tile is not blocked, move towards it
                    if (!gridManager.IsTileOccupied(path[0]) || path[0] == floorTilemap.WorldToCell(playerCharacter.position))
                    {
                        gridManager.SetTileAsOccupied(enemyPosition, false);
                        yield return StartCoroutine(MoveToNextTile(path[0]));
                        enemyPosition = path[0];
                        gridManager.SetTileAsOccupied(enemyPosition, true);
                        path.RemoveAt(0);
                    }
                    else
                    {
                        // If the next tile is occupied, print "waiting" and stop movement
                        Debug.Log("Next tile is occupied, waiting");
                        break; // This makes the enemy skip any further movement for this turn
                    }
                }
                else
                {
                    Debug.Log("No valid path, waiting");
                    break; // No valid path found, enemy waits
                }
            }

            actionsRemaining--;
            yield return new WaitForSeconds(pauseBetweenTiles);
        }

        // At the start of its turn, an enemy uses the getAdjacentTiles method from the GridManager script
        // If its next to the player, it will use its action to attack
        // If not, it will calculate a new path to the player using the FindPath method in the Pathfinding script
    }

    IEnumerator MoveToNextTile(Vector3Int nextTile)
    {
        Vector3 targetPosition = floorTilemap.CellToWorld(nextTile) + floorTilemap.cellSize / 2;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / movementSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime * movementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // This function works similar to the PlayerController function to lerp the enemy sprite smoothly to the targetPosition
    }

    void AttackPlayer()
    {
        Debug.Log("Next to Player, I now Attack!");
    }
}