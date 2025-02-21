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
        // This method returns a list of Vector3Ints, a 'Path' for the enemies to follow
        // It takes in two values, a Vector3Int Start Position and a Vector3Int targetPosition

        List<Vector3Int> tilesToCheck = new List<Vector3Int> { startPosition };
        // A list of tiles to check as part of the pathfinding

        Dictionary<Vector3Int, Vector3Int> tileMovedFrom = new Dictionary<Vector3Int, Vector3Int>();
        // A dictionary that keeps track of where each tile was moved from
        // Each value has two key values. Both of these are Vector3Ints

        Dictionary<Vector3Int, int> gCost = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, int> fCost = new Dictionary<Vector3Int, int>();
        // gCost is the cost of getting to each tile from the previous tile
        // fCost is the total cost from the starting position to that tile on the grid.
        // Both of these variables are dictionaries
        // Dictionaries store a value with two keys (two more values that can be of different types)
        // Each of these values for gCost and fCost stores the tile position and its value in its corresponding dictionary

        gCost[startPosition] = 0;
        // The startPosition gCost is 0 as it's the starting point (costs nothing to move here)

        fCost[startPosition] = GetDistance(startPosition, targetPosition);

        while (tilesToCheck.Count > 0) // This loop keeps running while there are tiles to check in the tilesToCheck list
        {
            Vector3Int currentTile = tilesToCheck[0];

            foreach (var position in tilesToCheck)
            {
                if (fCost[position] < fCost[currentTile])
                    currentTile = position;

                // The first time we loop over tilesToCheck there is only the startPosition to check
                // Once there are more tiles added to tilesToCheck this function then loops over each tile
                // For each tile, we check who has the lowest fCost as this implies it will lead to the shortest path for the enemy to follow
            }

            if (gridManager.getAdjacentTiles(targetPosition).Contains(currentTile))
            {
                if (currentTile == startPosition)
                    return new List<Vector3Int>(); // Edge case to return a null list if we're already at an adjacent tile to the player

                return RetracePath(tileMovedFrom, startPosition, currentTile);

                // This function stops searching tiles if any of the adjacent tiles to the target position is the current tile being checked
                // Why? This is because the enemy only needs to be adjacent to the targetPosition (the player) to attack them
                // Then the path is reversed with the RetracePath method to create a path of coordinates for the enemy to follow
            }


            tilesToCheck.Remove(currentTile); // Removes the currentTile to avoid looping over it again in the next loop

            foreach (Vector3Int adjacentTiles in gridManager.getAdjacentTiles(currentTile)) // Gets all the neighbouring walkable tiles to the currentTile from the GridManager
            {
                int newGCost = gCost[currentTile] + 1;
                if (!gCost.ContainsKey(adjacentTiles) || newGCost < gCost[adjacentTiles])
                {
                    gCost[adjacentTiles] = newGCost;
                    fCost[adjacentTiles] = newGCost + GetDistance(adjacentTiles, targetPosition);
                    tileMovedFrom[adjacentTiles] = currentTile;

                    if (!tilesToCheck.Contains(adjacentTiles))
                        tilesToCheck.Add(adjacentTiles);

                    // Updates the gCost and fCost for the new tiles to be checked in adjacent tiles
                    // Then we add these to the tilesToCheck list
                    // Also adds the currentTile's coordinates to the dictionary of tileMovedFrom adjacent tiles
                    // This means each new adjacent tile has a record of which tile it has been checked from
                }
            }
        }

        return new List<Vector3Int>(); // Returns no path if player not found
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

        // Retraces the path back from the targetPosition (player) to the currentPosition of the enemy
        // Each tile in the tileMovedFrom dictionary knows what its current coordinates are and where it was checked from
        // Using this info, we can construct a path backwards from the player to the enemy
        // At the end of this method, we reverse that path to create a chronological path of nodes/tiles from the enemy to the player
    }

    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
