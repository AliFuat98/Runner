using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
  public static GameInput Instance { get; private set; }

  public enum Binding {
    Move_Up,
    Move_Down,
    Move_Left,
    Move_Right,
    Pause,
  }

  private PlayerInputActions playerInputActions;

  public event EventHandler OnJumpAction;

  public event EventHandler OnSlideAction;

  private void OnDestroy() {
    playerInputActions.Player.Jump.performed -= Jump_performed;
    playerInputActions.Player.Slide.performed -= Slide_performed;

    playerInputActions.Dispose();
  }

  private void Awake() {
    Instance = this;

    /// open new input system
    playerInputActions = new PlayerInputActions();

    /// sistemi aç
    playerInputActions.Player.Enable();

    playerInputActions.Player.Jump.performed += Jump_performed;
    playerInputActions.Player.Slide.performed += Slide_performed;
  }

  private void Jump_performed(InputAction.CallbackContext obj) {
    OnJumpAction?.Invoke(this, EventArgs.Empty);
  }

  private void Slide_performed(InputAction.CallbackContext obj) {
    OnSlideAction?.Invoke(this, EventArgs.Empty);
  }

  public Vector2 GetMovementVector(bool normalized = false) {
    Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

    if (normalized) {
      inputVector = inputVector.normalized;
    }

    return inputVector;
  }
}