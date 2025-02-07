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

    // Update is called once per frame
    void Update()
    {
        InputCheck();
        RotateSpinner();
        HighlightCheck(movementSpinner.transform.position);
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
            transform.position = floorTilemap.GetCellCenterWorld(gridPosition);
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
        Vector2 rotatePoint;
        Vector3 rotateAxis = new Vector3(0, 0, 1);
        
        rotatePoint = transform.position;
        movementSpinner.transform.RotateAround(rotatePoint, rotateAxis, Time.deltaTime * spinSpeed);
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