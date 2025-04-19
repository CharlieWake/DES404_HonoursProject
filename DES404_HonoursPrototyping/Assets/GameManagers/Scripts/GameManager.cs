using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game References")]
    public GridManager gridManager;
    public TurnManager turnManager;
    public GameObject playerCharacter;
    public PlayerController playerController;
    public PlayerStats playerStats;
    public GameObject pauseMenuPanel;

    [Header("Grid References")]
    public Tilemap floorTilemap;
    public Tilemap decorTilemap;
    public GameObject highlighter;

    public bool isGamePaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (playerCharacter == null)
        {
            playerCharacter = GameObject.FindWithTag("Player");
        }

        if (playerController == null)
        {
            playerController = playerCharacter.GetComponent<PlayerController>();
        }

        if (playerStats == null)
        {
            playerStats = playerCharacter.GetComponent<PlayerStats>();
        }

        if (gridManager == null || turnManager == null)
        {
            Debug.LogError("GameManager is missing references.");
        }
    }

    public void PauseGame()
    {
        // playerController.HideMovementSpinner();
        pauseMenuPanel.SetActive(true);    
        isGamePaused = true;
    }

    public void UnpauseGame()
    {
        pauseMenuPanel.SetActive(false);
        // playerController.ShowMovementSpinner();
        isGamePaused=false;
    }
}
