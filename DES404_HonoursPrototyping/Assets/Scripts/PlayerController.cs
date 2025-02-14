using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap decorTilemap;
    [SerializeField] private GameObject highlighter;

    [SerializeField] private GameObject movementSpinner;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3Int gridPosition;

    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float spinSpeed;
    private Vector3 spinnerStartPosition;
    private Quaternion spinnerStartRotation;
    private bool hasResetSpinner = false;

    public bool isMoving = false;

    private void Start()
    {
        spinnerStartPosition = movementSpinner.transform.localPosition;
        spinnerStartRotation = movementSpinner.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.instance.IsPlayerTurn() && !isMoving)
        {
            movementSpinner.SetActive(true);
            highlighter.SetActive(true);

            if (!hasResetSpinner)
            {
                movementSpinner.transform.localPosition = spinnerStartPosition;
                movementSpinner.transform.localRotation = spinnerStartRotation;
                hasResetSpinner = true;
            }
                        
            InputCheck();
            RotateSpinner();
            // HighlightCheck(movementSpinner.transform.position);
        }
        else
        {
            movementSpinner.SetActive(false);
            highlighter.SetActive(false);

            hasResetSpinner = false;
        }
    }
    
    private void InputCheck()
    {        
        if (Input.GetKeyDown("space"))
        {
            // Debug.Log("Button is Pressed!");
            if (CanMoveToCell(movementSpinner.transform.position))
            {
                // movementSpinner.SetActive(false);
                isMoving = true;
                StartCoroutine(MoveToTargetPosition(gridPosition));
            }
        }
    }

        private bool CanMoveToCell(Vector2 spinnerPosition)
    {
        gridPosition = floorTilemap.WorldToCell(spinnerPosition);
        if (!floorTilemap.HasTile(gridPosition) || decorTilemap.HasTile(gridPosition))
            return false;
        return true;
    }

    IEnumerator MoveToTargetPosition(Vector3Int targetPosition)
    {
        Vector3 targetGridCentre = floorTilemap.GetCellCenterWorld(targetPosition);
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / movementSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetGridCentre, elapsedTime * movementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetGridCentre;
        highlighter.SetActive(false);
        Invoke("StartEnemyTurn", 1f);
    }

    private void RotateSpinner()
    {
        movementSpinner.transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * spinSpeed);
        HighlightCheck(movementSpinner.transform.position);
    }

    private void HighlightCheck(Vector2 spinnerPosition)
    {
        if (CanMoveToCell(spinnerPosition))
        {
            highlighter.transform.position = floorTilemap.GetCellCenterWorld(gridPosition);
            highlighter.SetActive(true);            
        }
        else
        {
            highlighter.SetActive(false);
        }
    }

    private void StartEnemyTurn()
    {
        TurnManager.instance.StartEnemyTurn();
    }
}