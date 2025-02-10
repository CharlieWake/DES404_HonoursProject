using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    public Dictionary<Vector3Int, bool> walkableTiles = new Dictionary<Vector3Int, bool>();

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

    }

    public bool IsTileWalkable(Vector3Int tilePosition)
    {
        return walkableTiles.ContainsKey(tilePosition) && walkableTiles[tilePosition];
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
    }
}
