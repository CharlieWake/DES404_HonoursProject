using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Grid References")]
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap decorTilemap;
    [SerializeField] private GameObject highlighter;

    public bool isGamePaused = false;

    private void Awake()
    {
        // Singleton Setup
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;             

        InitialiseReferences();
    }

    private void InitialiseReferences()
    {
        if (playerCharacter == null)
        {
            playerCharacter = GameObject.FindWithTag("Player");
        }

        if (playerCharacter != null)
        {
            playerController ??= playerCharacter.GetComponent<PlayerController>();
            playerStats ??= playerCharacter.GetComponent<PlayerStats>();
        }

        if (gridManager == null)
            Debug.LogError("GameManager is missing a GridManager ref");

        if (turnManager == null)
            Debug.LogError("GameManager is missing a TurnManager ref");

        if (playerCharacter == null || playerController == null || playerStats == null)
            Debug.LogError("GameManager is missing Player refs");
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }           

        isGamePaused = true;
    }

    public void UnpauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }          

        isGamePaused = false;
    }

    public GridManager GridManager => gridManager;
    public TurnManager TurnManager => turnManager;
    public PlayerController PlayerController => playerController;
    public PlayerStats PlayerStats => playerStats;
    public GameObject Highlighter => highlighter;
    public Tilemap FloorTilemap => floorTilemap;
    public Tilemap DecorTilemap => decorTilemap;
}
