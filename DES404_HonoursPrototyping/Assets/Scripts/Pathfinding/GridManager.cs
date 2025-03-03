using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    public Dictionary<Vector3Int, bool> walkableTiles = new Dictionary<Vector3Int, bool>();
    // Creates a new dictionary with two values to store a list of walkable tiles
    // The two values are a Vector3Int which will be the coordinates of the walkable tile
    // And the bool will say that it can be walked on or not

    public Dictionary<Vector3Int, bool> occupiedTiles = new Dictionary<Vector3Int, bool>();
    // Creates another dictionary with two values to store a list of tiles that are occupied by enemies
    // This dictionary takes in the tile position on the tilemap (Vector3Int) and whether it is occupied or not (bool)

    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        floorTilemap.CompressBounds();
        BoundsInt tilemapBounds = floorTilemap.cellBounds;

        foreach (Vector3Int tilePosition in tilemapBounds.allPositionsWithin)
        {
            if (floorTilemap.HasTile(tilePosition))
            {
                walkableTiles[tilePosition] = true;
            }
        }

        // This function first compresses the bounds of the tilemap to optimize the walkable loop
        // Then for each tile within the new tilemap bounds, it checks if there is a tile on that location
        // If yes, it adds its coordinates to the dictionary of walkable tiles

    }

    public bool IsTileWalkable(Vector3Int tilePosition)
    {
        return walkableTiles.ContainsKey(tilePosition) && walkableTiles[tilePosition];

        // This method takes in a tilePosition and returns a yes or not value (bool)
        // It checks if the walkable tile dictionary contains the inputted tileposition
        // It also checks if that tile position is marked as walkable
        // If both are yes, the enemy or player could move onto that tile
    }

    public List<Vector3Int> getAdjacentTiles(Vector3Int position)
    {
        List<Vector3Int> adjacentTiles = new List<Vector3Int>
        {
            position + Vector3Int.up,
            position + Vector3Int.down,
            position + Vector3Int.left,
            position + Vector3Int.right,
        };

        adjacentTiles.RemoveAll(n  => !IsTileWalkable(n));
        return adjacentTiles;

        // This method returns a list of Vector3Ints and takes in a position
        // A new list of adjacent tiles is created. 
        // The list is composed of 4 tiles north, east, south and west of the position input
        // Then the list is checked to see if any of the tiles in those 4 directions aren't walkable 
        // If they are, they're removed from the list
        // The method then returns the list of remaining adjacent tiles
    }

    public void SetTileAsOccupied(Vector3Int tilePosition, bool isOccupied)
    {
        if (isOccupied)
        {
            occupiedTiles[tilePosition] = true;
        }
        else if (occupiedTiles.ContainsKey(tilePosition))
        {
            occupiedTiles.Remove(tilePosition);
        }

        // This function takes in a tilePosition Vector3Int and a bool
        // Using the dictionary created above, it adds a tile that an enemy is on to the dictionary of occupied tiles
        // This function can also remove tiles by checking if they already exist in the dictionary and removing them
    }


    public bool IsTileOccupied(Vector3Int tilePosition)
    {
        return occupiedTiles.ContainsKey(tilePosition);

        // Gets the list of currently occupied tiles which can be checked when enemies or the player are moving to a new grid space
    }
}
