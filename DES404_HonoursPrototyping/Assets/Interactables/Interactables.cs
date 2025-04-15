using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interactables : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Tilemap floorTilemap;
    private Vector3Int interactablesPosition; 

    // Start is called before the first frame update
    void Start()
    {
        interactablesPosition = floorTilemap.WorldToCell(transform.position);
        gridManager.SetTileAsOccupied(interactablesPosition, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
