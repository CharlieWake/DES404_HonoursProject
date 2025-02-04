using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap decorTilemap;

    [SerializeField] private GameObject movementSpinner;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Vector3Int gridPosition;

    private bool isFlipped;

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("space"))
        {
            Debug.Log("Space is Pressed!");
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

    private void SpriteFlip()
    {
        spriteRenderer.flipX = isFlipped;
    }

}
