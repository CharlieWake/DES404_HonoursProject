using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap decorTilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("w"))
        {
           //  Move(0, 1);
            Debug.Log("W key was pressed");
        }
       if (Input.GetKeyDown("a"))
        {
            Debug.Log("A key was pressed");
        }
       if (Input.GetKeyDown("s"))
        {
            Debug.Log("S key was pressed");
        }
       if (Input.GetKeyDown("d"))
        {
            Debug.Log("D key was pressed");
        }
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
            transform.position += (Vector3)direction;
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = floorTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!floorTilemap.HasTile(gridPosition) || decorTilemap.HasTile(gridPosition))
            return false;
        return true;
    }

}
