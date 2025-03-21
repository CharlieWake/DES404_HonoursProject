using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPressAction;

    [SerializeField] private PlayerController playerControllerScript;

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
        // Debug.Log("button tapped");
        playerControllerScript.TestInput();
        playerControllerScript.InputCheck();
    }
}
