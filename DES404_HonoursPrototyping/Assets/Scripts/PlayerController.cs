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

    [SerializeField] private float spinSpeed;
    private Vector3 spinnerStartPosition;
    private Quaternion spinnerStartRotation;
    private bool hasResetSpinner = false;

    private void Start()
    {
        spinnerStartPosition = movementSpinner.transform.localPosition;
        spinnerStartRotation = movementSpinner.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.instance.IsPlayerTurn())
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
            HighlightCheck(movementSpinner.transform.position);
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
            MoveCharacter(movementSpinner.transform.position);
        }
    }

    private void MoveCharacter(Vector2 spinnerPosition)
    {
        if (CanMoveToCell(spinnerPosition))
        {
            transform.position = floorTilemap.GetCellCenterWorld(gridPosition);
            TurnManager.instance.StartEnemyTurn();
        }
            

    }

    private bool CanMoveToCell(Vector2 spinnerPosition)
    {
        gridPosition = floorTilemap.WorldToCell(spinnerPosition);
        if (!floorTilemap.HasTile(gridPosition) || decorTilemap.HasTile(gridPosition))
            return false;
        return true;
    }

    private void RotateSpinner()
    {
        movementSpinner.transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * spinSpeed);
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
}