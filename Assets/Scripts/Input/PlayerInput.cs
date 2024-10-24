using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
{
    public event UnityAction<Vector2> ONMove = delegate {  };
    public event UnityAction ONStopMove = delegate {  }; 
    public event UnityAction ONFire = delegate {  }; 
    public event UnityAction ONStopFire = delegate {  };
    
    private InputActions _inputActions;

    private void OnEnable()
    {
        _inputActions = new InputActions();
        
        _inputActions.Gameplay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }

    public void DisableAllInputs()
    {
        _inputActions.Gameplay.Disable();
    }

    public void EnableGameplayInput()
    {
        _inputActions.Gameplay.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ONMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            ONStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ONFire.Invoke();
        }

        if (context.canceled)
        {
            ONStopFire.Invoke();
        }
    }
}
