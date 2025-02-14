using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject playerCharacter;

    [SerializeField] private PlayerController playerControllerScript;

    [SerializeField] private List<EnemyMovement> enemies = new List<EnemyMovement>();

    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void FindAllEnemies(EnemyMovement enemy)
    {
        enemies.Add(enemy);
    }

    public void StartEnemyTurn()
    {
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        isPlayerTurn = false;

        foreach (EnemyMovement enemy in enemies)
        {
            virtualCamera.Follow = enemy.transform;
            yield return enemy.TakeTurn();
        }

        isPlayerTurn= true;
        playerControllerScript.isMoving = false;
        virtualCamera.Follow = playerCharacter.transform;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
