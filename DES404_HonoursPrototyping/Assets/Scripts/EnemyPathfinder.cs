using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;

public class EnemyPathfinder : MonoBehaviour
{

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Transform playerCharacter;
    [SerializeField] private float movementSpeed;

    [SerializeField] private Vector3Int currentGridPos;
    [SerializeField] private Vector3Int targetGridPos;
    [SerializeField] private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        currentGridPos = floorTilemap.WorldToCell(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            MoveEnemy();
        }
    }

    void MoveEnemy()
    {
        Vector3Int playerGridPosition = floorTilemap.WorldToCell(playerCharacter.position);

        Vector3Int moveDirection = playerGridPosition - currentGridPos;
        moveDirection = new Vector3Int((int)Mathf.Sign(moveDirection.x), (int)Mathf.Sign(moveDirection.y), 0);

        targetGridPos = (currentGridPos + moveDirection);

        StartCoroutine(MoveToTarget(targetGridPos));

    }

    System.Collections.IEnumerator MoveToTarget(Vector3Int target)
    {
        isMoving = true;


        Vector3 targetWorldPosition = floorTilemap.CellToWorld(target) + floorTilemap.cellSize / 2;

        while (Vector3.Distance(transform.position, targetWorldPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetWorldPosition;
        currentGridPos = target;
        // isMoving = false;
    }
}
