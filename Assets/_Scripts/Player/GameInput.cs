using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;
    public event EventHandler OnOpenMenuAction;
    public event EventHandler OnOpenInventoryAction;

    private bool isInputActionEnabled;
    
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        isInputActionEnabled = true;
        playerInputActions.Player.ToggleOpenMenu.performed += ToggleOpenMenu_performed;
        playerInputActions.Player.ToggleOpenInventory.performed += ToggleOpenInventory_performed;
    }

    public void DisablePlayerInput()
    {
     playerInputActions.Player.Disable();
    }
    private void ToggleOpenMenu_performed(InputAction.CallbackContext obj)
    {
        OnOpenMenuAction?.Invoke(this, EventArgs.Empty);
    }
    private void ToggleOpenInventory_performed(InputAction.CallbackContext obj)
    {
        OnOpenInventoryAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        playerInputActions.Player.ToggleOpenMenu.performed -= ToggleOpenMenu_performed;
        playerInputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public bool IsTeleportKeyPressed()
    {
        return playerInputActions.Player.Teleport.IsPressed();
    }
    
    public bool IsAttackPressed()
    {
        return playerInputActions.Player.Attack.IsPressed();
    }
    
    
}
