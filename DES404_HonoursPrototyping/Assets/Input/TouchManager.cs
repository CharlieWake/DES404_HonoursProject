using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPressAction;

    private PlayerController playerControllerScript;
    [SerializeField] private PauseCharging pauseCharging;
    TurnManager turnManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerControllerScript = GetComponent<PlayerController>();
        touchPressAction = playerInput.actions["TouchPress"];
    }

    private void Start()
    {
        turnManager = GameManager.instance.TurnManager;
    }

    private void OnEnable()
    {
        touchPressAction.started += TouchStarted;
        touchPressAction.canceled += TouchCancelled;
        touchPressAction.performed += TouchPressed;      
    }

    private void OnDisable()
    {
        touchPressAction.started -= TouchStarted;
        touchPressAction.canceled -= TouchCancelled;
        touchPressAction.performed -= TouchPressed;
    }

    private void TouchStarted(InputAction.CallbackContext context)
    {
        if (context.interaction is UnityEngine.InputSystem.Interactions.HoldInteraction)
        {
            if (turnManager.isPlayerTurn == true && !playerControllerScript.takingAction)
            {
                pauseCharging.StartHold();
            }            
        }
    }

    private void TouchCancelled(InputAction.CallbackContext context)
    {
        pauseCharging.CancelHold();
    }


    private void TouchPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {            
            if(context.interaction is UnityEngine.InputSystem.Interactions.TapInteraction)
            {
                if (turnManager.isPlayerTurn == true && !GameManager.instance.isGamePaused)
                {
                    playerControllerScript.PlayerAction();
                }
            }
        }             
    }  
}
