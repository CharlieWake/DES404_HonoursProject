using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Transform playerCharacter;
    [SerializeField] private float movementSpeed = 1f;

    private Vector3Int enemyPosition;
    private List<Vector3Int> path = new List<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        enemyPosition = floorTilemap.WorldToCell(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (path == null || path.Count == 0)
            {
                Vector3Int playerPosition = floorTilemap.WorldToCell(playerCharacter.position);
                path = pathfinding.FindPath(enemyPosition, playerPosition);
            }

            MoveAlongPath();
        } 
    }

    IEnumerator MoveOneStep(Vector3Int nextTile)
    {
        Vector3 targetPosition = floorTilemap.CellToWorld(nextTile) + floorTilemap.cellSize / 2;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / movementSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime * movementSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    void MoveAlongPath()
    {
        if (path != null && path.Count > 0)
        {
            StartCoroutine(MoveOneStep(path[0]));
            enemyPosition = path[0];
            path.RemoveAt(0);
        }
    }
}
