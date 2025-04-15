using JetBrains.Annotations;
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
    [Header("Components")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject movementSpinner;
    private GridManager gridManager;
    private Tilemap floorTilemap;
    private Tilemap decorTilemap;
    private GameObject highlighter;

    private float movementSpeed = 1.5f;
    private int actionsPerTurn;
    private int remainingActions;
       
    private Vector3Int gridPosition;
        
    public float movementSpinnerSpeed;

    private Vector3 spinnerStartPosition;
    private Quaternion spinnerStartRotation;
    private bool hasResetSpinner = false;

    public bool takingAction = false;

    private void Start()
    {
        actionsPerTurn = playerData.actionsPerTurn;
        gridManager = GameManager.instance.gridManager;
        floorTilemap = GameManager.instance.floorTilemap;
        decorTilemap = GameManager.instance.decorTilemap;
        highlighter = GameManager.instance.highlighter;

        ResetActions();

        spinnerStartPosition = movementSpinner.transform.localPosition;
        spinnerStartRotation = movementSpinner.transform.localRotation;        
        Collider2D spinnerCollider = movementSpinner.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.instance.IsPlayerTurn() && !takingAction)
        {
            movementSpinner.SetActive(true);
            highlighter.SetActive(true);

            if (hasResetSpinner == false)
            {
                movementSpinner.transform.localPosition = spinnerStartPosition;
                movementSpinner.transform.localRotation = spinnerStartRotation;
                hasResetSpinner = true;
            }
                        
            RotateSpinner();

        }
        else
        {
            movementSpinner.SetActive(false);
            highlighter.SetActive(false);
            hasResetSpinner = false;
        }

        // Inside Update the logic checks whether it is the player's turn AND if the player is not already moving
        // If the player is moving or it's not their turn, the movementSpinner and Highlighter are hidden
        // Also resets the movementSpinner's local position and rotation to where the player parent object is
    }
    
    public void PlayerAction()
    {
        if (remainingActions > 0)
        {
            CellInteractionInfo cellInfo = GetCellInfo(movementSpinner.transform.position);

            if (cellInfo.enemy != null)
            {
                AttackEnemy(cellInfo.enemy);
            }
            else if (cellInfo.interactable != null)
            {
                Debug.Log("I interact with the interactable object");
            }
            else if (cellInfo.isWalkable)
            {
                StartCoroutine(MoveToTargetPosition(gridPosition));
            }                          
        }    
    }
    private CellInteractionInfo GetCellInfo(Vector2 spinnerPosition)
    {
        gridPosition = floorTilemap.WorldToCell(spinnerPosition);

        bool isWalkable = floorTilemap.HasTile(gridPosition) &&
                          !decorTilemap.HasTile(gridPosition) &&
                          gridManager.getAdjacentTiles(floorTilemap.WorldToCell(transform.position)).Contains(gridPosition);

        GameObject enemy = null;
        GameObject interactable = null;

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

        return new CellInteractionInfo
        {
            isWalkable = isWalkable,
            enemy = enemy,
            interactable = interactable,
        };
    }

    IEnumerator MoveToTargetPosition(Vector3Int targetPosition)
    {
        Vector3 targetGridCentre = floorTilemap.GetCellCenterWorld(targetPosition);
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        takingAction = true;
        remainingActions--;

        while (elapsedTime < 1f / movementSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetGridCentre, elapsedTime * movementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetGridCentre;

        yield return new WaitForSeconds(0.5f);

        if (remainingActions > 0)
        {
            takingAction = false;
        }
        else
        {
            Invoke("EndPlayerTurn", 1f);
        }        

        // This is a coroutine which is a function that can pause and resume at runtime
        // This coroutine finds the target grid position and smoothly lerps the player sprite towards it
        // The speed the player moves towards the target position is based on the movementSpeed variable
        // The elapsedTime variable keeps track of how long the player has taken to move
        // After the player has moved to the new position, there is a pause of 1 second then the StartEnemyTurn function is called
    }

    private void RotateSpinner()
    {
        movementSpinner.transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * movementSpinnerSpeed);
        HighlightCheck(movementSpinner.transform.position);
    }

    private void HighlightCheck(Vector2 spinnerPosition)
    {
        CellInteractionInfo cellInfo = GetCellInfo(movementSpinner.transform.position);

        if (cellInfo.enemy != null)
        {
            highlighter.transform.position = floorTilemap.GetCellCenterWorld(gridPosition);
            highlighter.SetActive(true);
        }
        else if (cellInfo.interactable != null)
        {
            highlighter.transform.position = floorTilemap.GetCellCenterWorld(gridPosition);
            highlighter.SetActive(true);
        }
        else if (cellInfo.isWalkable)
        {
            highlighter.transform.position = floorTilemap.GetCellCenterWorld(gridPosition);
            highlighter.SetActive(true);
        }
        else
        {
            highlighter.SetActive(false);
        }
    }     

    private void EndPlayerTurn()
    {
        TurnManager.instance.StartEnemyTurn();

        // Invoked at the end of the 'MoveToTargetPosition' coroutine.
    }

    public void ResetActions()
    {
        remainingActions = actionsPerTurn;
    }

    private void AttackEnemy(GameObject enemy)
    {       

        takingAction = true;
        remainingActions--;

        StartCoroutine(AttackAnimation(enemy));
    }  
    
    IEnumerator AttackAnimation(GameObject enemy)
    {
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = enemy.transform.position;

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

        // Deal Damage
        float damageAmount = playerData.GetRandomDamage();
        enemy.GetComponent<EnemyStats>().TakeDamage(damageAmount);
        // Debug.Log("I did " + damageAmount + " damage to " + enemy.name);

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
                
        // Turn End Logic
        if (remainingActions > 0)
        {
            takingAction = false;
        }
        else
        {
            Invoke("EndPlayerTurn", 2f);
        }
    }    
}