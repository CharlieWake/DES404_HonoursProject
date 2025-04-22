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
    [SerializeField] protected GameObject alertIcon;
    [SerializeField] protected GameObject healthBar;
    protected bool inCombat = false;
    protected bool hasSeenPlayer = false;

    protected EnemyStats enemyStats;

    [SerializeField] protected int sightRange = 5;

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

    public bool InCombat => inCombat;
    public bool HasSeenPlayer => hasSeenPlayer;
}
