using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{

    [SerializeField] private GridManager gridManager;
    [SerializeField] private Tilemap floorTilemap;

    public List<Vector3Int> FindPath(Vector3Int startPosition, Vector3Int targetPosition)
    {
        List<Vector3Int> tilesToCheck = new List<Vector3Int> { startPosition };
        Dictionary<Vector3Int, Vector3Int> tileMovedFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, int> gCost = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, int> fCost = new Dictionary<Vector3Int, int>();

        gCost[startPosition] = 0;
        fCost[startPosition] = GetDistance(startPosition, targetPosition);

        while (tilesToCheck.Count > 0)
        {
            Vector3Int currentTile = tilesToCheck[0];

            foreach (var position in tilesToCheck)
            {
                if (fCost[position] < fCost[currentTile])
                    currentTile = position;
            }

            if (gridManager.getAdjacentTiles(targetPosition).Contains(currentTile))
            {
                if (currentTile == startPosition)
                    return new List<Vector3Int>();

                return RetracePath(tileMovedFrom, startPosition, currentTile);
            }
                

            tilesToCheck.Remove(currentTile);

            foreach (Vector3Int adjacentTiles in gridManager.getAdjacentTiles(currentTile))
            {
                int newGCost = gCost[currentTile] + 1;
                if (!gCost.ContainsKey(adjacentTiles) || newGCost < gCost[adjacentTiles])
                {
                    gCost[adjacentTiles] = newGCost;
                    fCost[adjacentTiles] = newGCost + GetDistance(adjacentTiles, targetPosition);
                    tileMovedFrom[adjacentTiles] = currentTile;

                    if (!tilesToCheck.Contains(adjacentTiles))
                        tilesToCheck.Add(adjacentTiles);
                }
            }
        }

        return new List<Vector3Int>();
    }

    private List<Vector3Int> RetracePath(Dictionary<Vector3Int, Vector3Int> tileMovedFrom, Vector3Int startPosition, Vector3Int targetPosition)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int currentPosition = targetPosition;

        while (currentPosition != startPosition)
        {
            path.Add(currentPosition);
            currentPosition = tileMovedFrom[currentPosition];
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
