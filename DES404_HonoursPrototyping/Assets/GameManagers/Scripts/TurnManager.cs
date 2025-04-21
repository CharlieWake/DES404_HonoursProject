using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance { get; private set; }

    [Header("Turn Settings")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private GameObject playerCharacter;
    private PlayerController playerController;

    private readonly List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();
    public bool isPlayerTurn = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (virtualCamera == null)
        {
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
    }

    private void Start()
    {
        playerCharacter = GameManager.instance.PlayerController.gameObject;
        playerController = GameManager.instance.PlayerController;

        if (virtualCamera != null && playerCharacter != null)
        {
            virtualCamera.Follow = playerCharacter.transform;
        }
    }

    public bool IsPlayerTurn() => isPlayerTurn;

    public void RegisterEnemy(EnemyBehaviour enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(EnemyBehaviour enemy)
    {
        enemies.Remove(enemy);
    }

    public void StartEnemyTurn()
    {
        SortEnemiesByDistanceToPlayer();
        StartCoroutine(EnemyTurnSequence());
    }

    private IEnumerator EnemyTurnSequence()
    {
        isPlayerTurn = false;

        yield return TriggerStartOfEnemyTurn();

        foreach (EnemyBehaviour enemy in enemies)
        {
            if (enemy == null) continue;

            virtualCamera.Follow = enemy.transform;
            yield return enemy.TakeTurn();
        }

        yield return TriggerEndOfEnemyTurn();

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        isPlayerTurn = true;
        playerController.ResetActions();
        playerController.takingAction = false;

        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerCharacter.transform;
        }
    }

    private void SortEnemiesByDistanceToPlayer()
    {
        enemies.Sort((a, b) =>
            GetDistance(a.transform.position, playerCharacter.transform.position)
            .CompareTo(GetDistance(b.transform.position, playerCharacter.transform.position)));
    }

    private int GetDistance(Vector3 a, Vector3 b)
    {
        return Mathf.RoundToInt(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
    }


    // ---Enemy Turn Phases---

    private IEnumerator TriggerStartOfEnemyTurn()
    {
        yield return null;
    }

    private IEnumerator TriggerEndOfEnemyTurn()
    {
        yield return null;
    }
}
