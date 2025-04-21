using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct CellInteractionInfo
{
    public bool isWalkable;
    public GameObject enemy;
    public GameObject interactable;
}

public class PlayerController : MonoBehaviour
{
    [Header("Serialized Fields")]
    [SerializeField] private DamageData damageData;
    [SerializeField] private PlayerLevelUpData playerLevelUpData;
    [SerializeField] private GameObject movementSpinner;
    [SerializeField] private ActionDots actionDots;
    [SerializeField] private int remainingActions;
    [SerializeField] private float movementSpeed = 1.5f;
    [SerializeField] public float movementSpinnerSpeed = -50f;

    [Header("Cached References")]
    private PlayerStats playerStats;
    private GridManager gridManager;
    private Tilemap floorTilemap;
    private Tilemap decorTilemap;
    private GameObject highlighter;

    [Header("Runtime State")]
    public bool takingAction = false;
    private bool hasResetSpinner = false;
    private Vector3Int gridPosition;
    private Vector3 spinnerStartPosition;
    private Quaternion spinnerStartRotation;

    private void Start()
    {
        GetReferences();
        InitialisePlayer();
        SetupSpinnerPositions();                                 
    }

    private void GetReferences()
    {
        gridManager = GameManager.instance.GridManager;
        floorTilemap = GameManager.instance.FloorTilemap;
        decorTilemap = GameManager.instance.DecorTilemap;
        highlighter = GameManager.instance.Highlighter;
        playerStats = GetComponent<PlayerStats>();
    }

    private void InitialisePlayer()
    {
        playerStats.InitialiseStats();
        ResetActions();
        UpdateActionDots();
    }

    private void SetupSpinnerPositions()
    {
        spinnerStartPosition = movementSpinner.transform.localPosition;
        spinnerStartRotation = movementSpinner.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlayerTurnAndReady())
        {
            DeactivateSpinnerAndHighlighter();
            return;
        }

        ActivateSpinnerAndHighlighter();
        ResetSpinner();
        RotateSpinner();        
    }
    
    private bool IsPlayerTurnAndReady()
    {
        return TurnManager.instance.IsPlayerTurn() && !takingAction && !GameManager.instance.isGamePaused;
    }

    private void ActivateSpinnerAndHighlighter()
    {
        movementSpinner.SetActive(true);
        highlighter.SetActive(true);
    }

    private void DeactivateSpinnerAndHighlighter()
    {
        movementSpinner.SetActive(false);
        highlighter.SetActive(false);
        hasResetSpinner = false;
    }

    private void ResetSpinner()
    {
        if (hasResetSpinner) return;

        movementSpinner.transform.localPosition = spinnerStartPosition;
        movementSpinner.transform.localRotation = spinnerStartRotation;
        hasResetSpinner = true;
    }

    public void PlayerAction()
    {
        if (!CanTakeAction()) return;

        CellInteractionInfo cellInfo = GetCellInfo(movementSpinner.transform.position);

        if (cellInfo.enemy != null)
        {
            HandleEnemyInteraction(cellInfo.enemy);
        }
        else if (cellInfo.interactable != null)
        {
            HandleInteractable(cellInfo.interactable);
        }
        else if (cellInfo.isWalkable)
        {
            StartCoroutine(MoveToTargetPosition(gridPosition));
        }          
    }

    private bool CanTakeAction()
    {
        return remainingActions > 0;
    }

    private void HandleEnemyInteraction(GameObject enemy)
    {
        AttackEnemy(enemy);
    }

    private void HandleInteractable(GameObject interactable)
    {
        //Interact with Interactable Here
    }

    IEnumerator MoveToTargetPosition(Vector3Int targetPosition)
    {
        StartPlayerMovement();

        yield return MoveToPositionRoutine(floorTilemap.GetCellCenterWorld(targetPosition));

        yield return new WaitForSeconds(0.5f);

        FinishPlayerMovement();
    }

    private void StartPlayerMovement()
    {
        takingAction = true;
        UseAction();
    }

    private void FinishPlayerMovement()
    {
        if (remainingActions > 0)
        {
            takingAction = false;
        }
        else
        {
            Invoke(nameof(EndPlayerTurn), 1f);
        }
    }

    private IEnumerator MoveToPositionRoutine(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float duration = 1f / movementSpeed;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void AttackEnemy(GameObject enemy)
    {
        StartPlayerAttack();
        StartCoroutine(AttackRoutine(enemy));
    }

    private IEnumerator AttackRoutine(GameObject enemy)
    {
        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = GetAttackLungePosition(enemy.transform.position);

        yield return LerpPosition(originalPosition, attackPosition, 0.1f);

        DealDamageToEnemy(enemy);

        yield return new WaitForSeconds(0.05f);

        yield return LerpPosition(attackPosition, originalPosition, 0.1f);

        FinishPlayerAttack();
    }

    private Vector3 GetAttackLungePosition(Vector3 enemyPosition)
    {
        return Vector3.Lerp(transform.position, enemyPosition, 0.2f);
    }

    private void DealDamageToEnemy(GameObject enemy)
    {
        float damage = damageData.GetRandomDamage();
        enemy.GetComponent<EnemyStats>().TakeDamage(damage);
    }

    private void StartPlayerAttack()
    {
        takingAction = true;
        UseAction();
    }

    private void FinishPlayerAttack()
    {
        if (remainingActions > 0)
        {
            takingAction = false;
        }
        else
        {
            Invoke(nameof(EndPlayerTurn), 2f);
        }
    }

    private IEnumerator LerpPosition(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }

    private void HighlightCheck(Vector3 spinnerWorldPosition)
    {
        CellInteractionInfo cellInfo = GetCellInfo(spinnerWorldPosition);

        if (cellInfo.enemy != null || cellInfo.interactable != null || cellInfo.isWalkable)
        {
            HighlightCellAt(gridPosition);            
        }
        else
        {        
            highlighter.SetActive(false);        
        }
    }

    private void HighlightCellAt(Vector3Int cellPosition)
    {
        highlighter.transform.position = floorTilemap.GetCellCenterWorld(cellPosition);
        highlighter.SetActive(true);
    }

    private void RotateSpinner()
    {
        movementSpinner.transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * movementSpinnerSpeed);
        HighlightCheck(movementSpinner.transform.position);
    }

    private void EndPlayerTurn()
    {
        TurnManager.instance.StartEnemyTurn();
    }

    public void ResetActions()
    {
        remainingActions = playerStats.ActionsPerTurn;
        actionDots.ResetActionDots();
    }          

    public void HideMovementSpinner()
    {
        movementSpinner.GetComponent<SpriteRenderer>().enabled = false;
        foreach (Transform child in movementSpinner.transform)
        {
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = false;
                break;
            }
        }
    }

    public void ShowMovementSpinner()
    {
        movementSpinner.GetComponent<SpriteRenderer>().enabled = true;
        foreach (Transform child in movementSpinner.transform)
        {
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = true;
                break;
            }
        }
    }

    public void UseAction()
    {
        remainingActions--;
        actionDots.UseAction();
    }

    public void UpdateActionDots()
    {
        actionDots.SetActionDotCount(playerStats.ActionsPerTurn);
    }

    // ---CellInfo---

    private CellInteractionInfo GetCellInfo(Vector2 spinnerPosition)
    {
        gridPosition = floorTilemap.WorldToCell(spinnerPosition);
        Vector3Int playerPosition = floorTilemap.WorldToCell(transform.position);

        List<Vector3Int> adjacentTiles = gridManager.getAdjacentTiles(playerPosition);
        bool isAdjacent = adjacentTiles.Contains(gridPosition);

        bool isWalkable = floorTilemap.HasTile(gridPosition) &&
                          !decorTilemap.HasTile(gridPosition) &&
                          isAdjacent;

        GameObject enemy = null;
        GameObject interactable = null;

        if (isAdjacent)
        {
            Collider2D enemyCollider = Physics2D.OverlapPoint(floorTilemap.GetCellCenterWorld(gridPosition), LayerMask.GetMask("Enemy"));
            if (enemyCollider != null)
            {
                enemy = enemyCollider.gameObject;
            }

            Collider2D interactableCollider = Physics2D.OverlapPoint(floorTilemap.GetCellCenterWorld(gridPosition), LayerMask.GetMask("Interactable"));
            if (interactableCollider != null)
            {
                interactable = interactableCollider.gameObject;
            }
        }

        return new CellInteractionInfo
        {
            isWalkable = isWalkable,
            enemy = enemy,
            interactable = interactable,
        };                     
    }
}