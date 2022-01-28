using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    #region Singleton
    private static InputSystem instance;

    public static InputSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputSystem>();
            }
            return instance;
        }
    }

    #endregion
    public event Action<Vector2> OnButtonPressed;
    private Vector2 movement;
    private bool isInputEnabled;
    private Joystick floatingJoystick;
    public bool IsInputEnabled
    {
        get
        {
            return isInputEnabled;
        }
        set
        {
            isInputEnabled = value;

            if (!isInputEnabled)
            {
                OnButtonPressed?.Invoke(Vector2.zero);
            }
        }
    }

    private void Awake()
    {
        floatingJoystick = FindObjectOfType<Joystick>();
    }
    public void Movement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            movement = Vector2.zero;
        }

        if (IsInputEnabled)
        {
            OnButtonPressed?.Invoke(movement);
        }
    }
    private void Update()
    {
        if (floatingJoystick != null)
        {
            movement = new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);

            OnButtonPressed?.Invoke(movement);
        }
    }
}
