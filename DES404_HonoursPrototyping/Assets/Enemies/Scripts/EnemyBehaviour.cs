using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(EnemyStats))]
public abstract class EnemyBehaviour : MonoBehaviour
{
    [Header("References")]
    protected GridManager gridManager;
    protected Pathfinding pathfinding;
    protected GameManager gameManager;
    protected PlayerController playerController;
    protected PlayerStats playerStats;

    protected Vector3Int currentGridPosition;
    protected bool inCombat = false;
    protected bool hasSeenPlayer = false;

    [SerializeField] protected GameObject alertIcon;
    [SerializeField] protected GameObject healthBar;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed = 5f;

    protected EnemyStats enemyStats;

    [SerializeField] protected int sightRange = 5;

    [SerializeField] protected AudioClip alertSFX;
    [SerializeField] protected AudioClip attackSFX;
    [SerializeField] protected AudioClip moveSFX;

    protected virtual void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    public virtual void Initialise()
    {
        gameManager = GameManager.instance;
        gridManager = gameManager.GridManager;
        playerController = gameManager.PlayerController;
        playerStats = gameManager.PlayerStats;
        pathfinding = GetComponent<Pathfinding>();

        currentGridPosition = gridManager.grid.WorldToCell(transform.position);
        gridManager.SetTileAsOccupied(currentGridPosition, true);
    }

    protected virtual void Start()
    {
        Initialise();
        RegisterEnemy();
    }

    public virtual IEnumerator TakeTurn()
    {             
        yield break;
    }

    public virtual void CheckForPlayer()
    {
        Vector3Int enemyPosition = currentGridPosition;
        Vector3Int playerPosition = gridManager.grid.WorldToCell(playerController.transform.position);

        int distance = Mathf.Abs(enemyPosition.x - playerPosition.x) + Mathf.Abs(enemyPosition.y - playerPosition.y);

        if (distance <= sightRange && HasLineOfSightToPlayer(enemyPosition, playerPosition))
        {
            Debug.Log("I see the player, now entering combat.");
            inCombat = true;
        }
    }

    public IEnumerator HandleAlertAndEnterCombat()
    {
        hasSeenPlayer = true;

        if (alertIcon != null)
        {
            alertIcon.SetActive(true);
            AudioManager.instance.PlaySFX(alertSFX);
        }

        yield return new WaitForSeconds(0.75f);

        if (alertIcon != null)
        {
            alertIcon.SetActive(false);            
        }

        yield return new WaitForSeconds(0.25f);

        if (healthBar != null)
        {
            healthBar.SetActive(true);
        }

        yield return TakeTurn();
    }
    
    protected bool IsPlayerAdjacent()
    {
        Vector3Int playerPos = gridManager.grid.WorldToCell(playerController.transform.position);
        return Vector3Int.Distance(currentGridPosition, playerPos) == 1;
    }

    protected IEnumerator MoveTowardsPlayer()
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
                if (moveSFX != null)
                {
                    AudioManager.instance.PlaySFX(moveSFX);
                }
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

    protected IEnumerator MoveAwayFromPlayer()
    {
        Vector3Int playerPosition = gridManager.grid.WorldToCell(playerController.transform.position);
        Vector3Int bestEscapeTile = currentGridPosition;
        int bestDistance = GetDistance(currentGridPosition, playerPosition);

        foreach (Vector3Int dir in new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, })
        {
            Vector3Int tileCandidate = currentGridPosition + dir;
            if (!gridManager.IsTileWalkable(tileCandidate) || gridManager.IsTileOccupied(tileCandidate))
                continue;

            int distance = GetDistance(tileCandidate, playerPosition);
            if (distance > bestDistance)
            {
                bestEscapeTile = tileCandidate;
                bestDistance = distance;
            }
        }

        if (bestEscapeTile != currentGridPosition)
        {
            gridManager.SetTileAsOccupied(currentGridPosition, false);
            gridManager.SetTileAsOccupied(bestEscapeTile, true);
            if (moveSFX != null)
            {
                AudioManager.instance.PlaySFX(moveSFX);
            }
            yield return MoveToNextTile(bestEscapeTile);
        }
    }

    protected IEnumerator MoveToNextTile(Vector3Int targetGridPosition)
    {
        Vector3 start = transform.position;
        Vector3 end = gridManager.grid.GetCellCenterWorld(targetGridPosition);
        float elapsed = 0f;
        float duration = 0.66f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        currentGridPosition = targetGridPosition;
    }

    protected IEnumerator MeleeAttack()
    {
        Vector3 start = transform.position;
        Vector3 target = playerController.transform.position;
        Vector3 lungePosition = Vector3.Lerp(start, target, 0.2f);

        float elapsed = 0f;
        float duration = 0.1f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, lungePosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        DealDamage();
        if (attackSFX != null)
        {
            AudioManager.instance.PlaySFX(attackSFX);
        }
        transform.position = lungePosition;
        elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(lungePosition, start, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = start;
    }

    protected IEnumerator RangedAttack (Vector3 targetWorldPosition)
    {
        if (projectilePrefab == null) yield break;

        Vector3 spawnPosition = transform.position;
        Vector3 projectileDirection = (targetWorldPosition - spawnPosition).normalized;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        if (attackSFX != null)
        {
            AudioManager.instance.PlaySFX(attackSFX);
        }
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.velocity = projectileDirection * projectileSpeed;
        }

        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetProjectileDamage(enemyStats.GetDamageAmount());
            projectileScript.SetOwner(gameObject);
        }

        yield return null;
    }

        protected void DealDamage()
    {
        playerStats.TakeDamage(enemyStats.GetDamageAmount());
    }

    protected void RegisterEnemy()
    {
        TurnManager.instance.RegisterEnemy(this);
    }

    public void UnregisterEnemy()
    {
        TurnManager.instance.UnregisterEnemy(this);
    }

    protected bool HasLineOfSightToPlayer(Vector3Int from, Vector3Int to)
    {
        Vector3Int direction = to - from;
        int steps = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        Vector2 step = new Vector2(direction.x, direction.y) / steps;

        Vector3 current = (Vector3)from + new Vector3(0.5f, 0.5f, 0); // center of tile
        for (int i = 0; i <= steps; i++)
        {
            Vector3Int tilePos = gridManager.grid.WorldToCell(current);
            if (!gridManager.IsTileWalkable(tilePos))
                return false; // blocked

            if (tilePos == to)
                return true;

            current += (Vector3)step;
        }

        return false;
    }

    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public bool InCombat => inCombat;
    public bool HasSeenPlayer => hasSeenPlayer;
}
