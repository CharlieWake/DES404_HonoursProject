using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : MonoBehaviour
{        
    public static TurnManager instance;

    private CinemachineVirtualCamera virtualCamera;
    private GameObject playerCharacter;
    private PlayerController playerControllerScript;

    // Creates a new List of type EnemyMovement script and creates it when the game starts.
    private List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    public bool isPlayerTurn = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // This function checks if there is already an instance active in the game
        // If there is, it destroys this game object
        // Otherwise, this object becomes the instance
        // This is called a Singleton, used for things like GameManagers where you would only want one instance of the object active in your game (like this TurnManager)

        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        playerCharacter = GameManager.instance.playerCharacter;
        playerControllerScript = playerCharacter.GetComponent<PlayerController>();

        virtualCamera.Follow = playerCharacter.transform;
    }

    public void FindAllEnemies(EnemyBehaviour enemy)
    {
        enemies.Add(enemy);

        // Finds all enemies in the scene and adds them to the previously created 'enemies' list.
        // This allows us to cycle through a list of enemies when it is time for the enemies to take their individual turns
        // This is called from the enemy's 'EnemyMovement' script
    }

    public void RemoveEnemy(EnemyBehaviour enemy)
    {
        enemies.Remove(enemy);
    }


    public void StartEnemyTurn()
    {
        SortEnemiesByDistance();
        StartCoroutine(EnemyTurn());
    }

    private void SortEnemiesByDistance()
    {
        enemies.Sort((enemy1, enemy2) =>
            GetDistance(enemy1.transform.position, playerCharacter.transform.position)
            .CompareTo(GetDistance(enemy2.transform.position, playerCharacter.transform.position)));
            
    }

    private int GetDistance(Vector3 enemyPosition, Vector3 playerPosition)
    {
        return Mathf.RoundToInt(Mathf.Abs(enemyPosition.x - playerPosition.x) + Mathf.Abs(enemyPosition.y - playerPosition.y));
    }

    private IEnumerator EnemyTurn()
    {
        isPlayerTurn = false;

        foreach (EnemyBehaviour enemy in enemies)
        {
            virtualCamera.Follow = enemy.transform;
            yield return enemy.TakeTurn();
        }

        isPlayerTurn= true;
        playerControllerScript.ResetActions();
        playerControllerScript.takingAction = false;
        virtualCamera.Follow = playerCharacter.transform;

        // This is a coroutine which is a function that can be paused and resumed at runtime
        // First, it sets the playerTurn to false then loops through each enemy in the enemy list
        // For each enemy, it gets the enemies to take their turn one-by-one
        // As the enemy takes its turn, the cinemachine camera follows the enemy's position
        // After all of the enemies have taken their turn, the isPlayerTurn bool is set back to true
        // Now the player can take their turn again and the camera pans back to them
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
