using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTouchManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPressAction;

    public MenuSpinnerController menuSpinner;   


    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found on this GameObject!");
            return;
        }

        touchPressAction = playerInput.actions["TouchPress"];

        if (touchPressAction == null)
        {
            Debug.LogError("TouchPress action not found in input actions!");
            return;
        }

        // menuSpinner = GetComponent<MenuSpinnerController>();

        if (menuSpinner == null)
        {
            Debug.LogError("MenuSpinner is not assigned in the Inspector!");
            return;
        }

        touchPressAction.performed += TouchPressed;
        touchPressAction.Enable();
    }

    private void OnDisable()
    {
        if (touchPressAction != null)
        {
            touchPressAction.performed -= TouchPressed;
            touchPressAction.Disable();
        }
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
              
        if (menuSpinner != null)
        {
            menuSpinner.ButtonPress();
        }
        else
        {
            Debug.LogError("menuSpinner is null during touch press!");
        }
    }
}
