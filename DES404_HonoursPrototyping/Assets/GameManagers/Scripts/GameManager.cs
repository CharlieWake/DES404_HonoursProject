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
    public PlayerStats playerStats;

    [Header("Grid References")]
    public Tilemap floorTilemap;
    public Tilemap decorTilemap;

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

        if (playerStats == null)
        {
            playerStats = playerCharacter.GetComponent<PlayerStats>();
        }

        if (gridManager == null || turnManager == null)
        {
            Debug.LogError("GameManager is missing references.");
        }
    }
}
