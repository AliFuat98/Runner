using UnityEngine;

public class Player : MonoBehaviour {

  [Header("Move Information")]
  [SerializeField] private float moveSpeed;

  [SerializeField] private float jumpForce;

  [Header("Collition Info")]
  [SerializeField] private float groundCheckDistance;

  [SerializeField] private LayerMask WhatIsGround;

  private Rigidbody2D rb;
  private GameInput GameInput;

  private bool isGrounded;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    if (rb == null) {
      Debug.LogError("player needs a rigidbody");
    }
    GameInput = GameInput.Instance;

    GameInput.OnJumpAction += GameInput_OnJumpAction;

    moveSpeed = 2;
    jumpForce = 5;
  }

  void Update() {
    HandleMovement();
  }

  private void HandleMovement() {
    Vector2 moveDirection = GameInput.GetMovementVector();
    if (moveDirection.x > 0) {
      rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    if (moveDirection.x < 0) {
      rb.velocity = new Vector2(moveSpeed * -1, rb.velocity.y);
    }
  }

  private void GameInput_OnJumpAction(object sender, System.EventArgs e) {
    isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, WhatIsGround);

    if (!isGrounded) {
      return;
    }

    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
  }

  private void OnDrawGizmos() {
    Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    Gizmos.color = Color.black;
  }
}