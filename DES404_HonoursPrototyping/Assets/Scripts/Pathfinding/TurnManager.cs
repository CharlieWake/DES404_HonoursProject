using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

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
            yield return enemy.TakeTurn();
        }

        isPlayerTurn= true;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
