using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputActions inputActions;

    public Vector2 MoveInput { get; private set; }
    public bool JumpHold { get; private set; }

    public event Action JumpPressed;
    public event Action AttackPressed;
    public event Action SpecialPressed;
    public event Action BlockPressed;
    public event Action RollPressed;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        // Move
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;

        //Jump
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Jump.canceled += OnJumpCanceled;

        // Buttons
        inputActions.Player.Attack.performed += OnAttackPerformed;
        inputActions.Player.Special.performed += OnSpecialPerformed;
        inputActions.Player.Block.performed += OnBlockPerformed;
        inputActions.Player.Roll.performed += OnRollPerformed;
    }

    private void OnDisable()
    {
        // Move
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;

        // Jump
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;
        
        // Buttons 
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Player.Special.performed -= OnSpecialPerformed;
        inputActions.Player.Block.performed -= OnBlockPerformed;
        inputActions.Player.Roll.performed -= OnRollPerformed;

        inputActions.Player.Disable();
    }

    // Move Input
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        // Debug.Log($"Move Input: {MoveInput}");
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveInput = Vector2.zero;
        // Debug.Log("Move Input Canceled");
    }

    // Button Input
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpHold = true;
        JumpPressed?.Invoke();
        // Debug.Log("Jump Pressed");
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        JumpHold = false;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        AttackPressed?.Invoke();
        // Debug.Log("Attack Pressed");
    }

    private void OnSpecialPerformed(InputAction.CallbackContext context)
    {
        SpecialPressed?.Invoke();
        // Debug.Log("Special Pressed");
    }

    private void OnBlockPerformed(InputAction.CallbackContext context)
    {
        BlockPressed?.Invoke();
        // Debug.Log("Block Pressed");
    }

    private void OnRollPerformed(InputAction.CallbackContext context)
    {
        RollPressed?.Invoke();
        // Debug.Log("Roll Pressed");
    }
}