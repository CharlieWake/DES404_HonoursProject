using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpinnerController : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] GameObject playerCharacter;

    [SerializeField] Vector2 rotatePoint;
    [SerializeField] Vector3 rotateAxis = new Vector3(0, 0, 1);

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap decorTilemap;
    [SerializeField] private GameObject highlighter;

    [SerializeField] private Vector3Int gridPosition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateSpinner();
        ShowHighlight(transform.position);
    }

    void RotateSpinner()
    {
        rotatePoint = playerCharacter.transform.position;
        transform.RotateAround(rotatePoint, rotateAxis, Time.deltaTime * spinSpeed);
    }

    private void ShowHighlight(Vector2 spinnerPosition)
    {
        if (ShouldShowHighlight(spinnerPosition))
        {
            highlighter.SetActive(true);
            highlighter.transform.position = floorTilemap.GetCellCenterWorld(gridPosition);
        }
        else
        {
            highlighter.SetActive(false);
        }
    }

    private bool ShouldShowHighlight(Vector2 spinnerPosition)
    {
        gridPosition = floorTilemap.WorldToCell(spinnerPosition);
        if (!floorTilemap.HasTile(gridPosition) || decorTilemap.HasTile(gridPosition))
            return false;
        return true;
    }
}
