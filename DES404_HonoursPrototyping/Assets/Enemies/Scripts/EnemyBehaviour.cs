using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBehaviour : MonoBehaviour
{
    private GridManager gridManager;
    private Pathfinding pathfinding;
    private TurnManager turnManager;
    private Transform playerCharacter;
    private Tilemap floorTilemap;

    [SerializeField] private EnemyData enemyData;
    private EnemyStats enemyStats;

    
    private float gridMovementSpeed = 1.5f;
    private float pauseBetweenActions = 1f;


    private Vector3Int enemyPosition;
    private List<Vector3Int> path = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameManager.instance.gridManager;
        turnManager = GameManager.instance.turnManager;
        playerCharacter = GameManager.instance.playerCharacter.transform;
        floorTilemap = GameManager.instance.floorTilemap;

        enemyStats = GetComponent<EnemyStats>();
        pathfinding = GetComponent<Pathfinding>();

        TurnManager.instance.FindAllEnemies(this);
        enemyPosition = floorTilemap.WorldToCell(transform.position);
        gridManager.SetTileAsOccupied(enemyPosition, true);
    }

    public IEnumerator TakeTurn()
    {
        int actionsRemaining = enemyData.actionsPerTurn;
        path = null;

        while (actionsRemaining > 0)
        {
            // Check if the enemy is adjacent to the player and can attack
            if (gridManager.getAdjacentTiles(enemyPosition).Contains(floorTilemap.WorldToCell(playerCharacter.position)))
            {
                AttackPlayer();
                actionsRemaining--;
                yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(pauseBetweenActions);
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

        while (elapsedTime < 1f / gridMovementSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime * gridMovementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // This function works similar to the PlayerController function to lerp the enemy sprite smoothly to the targetPosition
    }

    void AttackPlayer()
    {
        StartCoroutine(AttackAnimation());
    }

    public void RemoveEnemy()
    {
        gridManager.SetTileAsOccupied(enemyPosition, false);
        TurnManager.instance.RemoveEnemy(this);
    }

    IEnumerator AttackAnimation()
    {
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = playerCharacter.transform.position;

        Vector3 attackPosition = Vector3.Lerp(originalPosition, targetPosition, 0.2f);

        float animSpeed = 10f;
        float elapsedTime = 0f;

        // Lunge Forward
        while (elapsedTime < 0.1f)
        {
            transform.position = Vector3.Lerp(originalPosition, attackPosition, elapsedTime * animSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = attackPosition;

        // Debug.Log("Next to Player, I now Attack!");
        float damageAmount = Random.Range(1, 4);
        playerCharacter.GetComponent<PlayerStats>().TakeDamage(damageAmount);
        // Debug.Log("I dealt " + damageAmount + " to the player");

        yield return new WaitForSeconds(0.05f);

        // Move Back
        elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            transform.position = Vector3.Lerp(attackPosition, originalPosition, elapsedTime * animSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

}