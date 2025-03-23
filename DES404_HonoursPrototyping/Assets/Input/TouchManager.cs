using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPressAction;

    private PlayerController playerControllerScript;
    [SerializeField] TurnManager turnManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerControllerScript = GetComponent<PlayerController>();
        touchPressAction = playerInput.actions["TouchPress"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;      
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        if (turnManager.isPlayerTurn == true)
        {
            playerControllerScript.PlayerAction();
        }        
    }
}
