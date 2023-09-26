using UnityEngine;

public class Player : MonoBehaviour {

  public static Player Instance;

  [Header("Move Information")]
  [SerializeField] private float moveSpeed;

  [SerializeField] private float jumpForce;

  [Header("Collition Info")]
  [SerializeField] private float groundCheckDistance;

  [SerializeField] private LayerMask WhatIsGround;

  public Rigidbody2D Rb { get; private set; }
  private GameInput GameInput;

  public bool IsGrounded { get; private set; }

  private void Awake() {
    Instance = this;
  }

  void Start() {
    Rb = GetComponent<Rigidbody2D>();

    GameInput = GameInput.Instance;

    GameInput.OnJumpAction += GameInput_OnJumpAction;

    moveSpeed = 5;
    jumpForce = 15;
  }

  void Update() {
    HandleMovement();
  }
  private void HandleMovement() {
    IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, WhatIsGround);
      
    Vector2 moveDirection = GameInput.GetMovementVector();
    if (moveDirection.x > 0) {
      Rb.velocity = new Vector2(moveSpeed, Rb.velocity.y);
    }

    if (moveDirection.x < 0) {
      Rb.velocity = new Vector2(moveSpeed * -1, Rb.velocity.y);
    }
  }

  private void GameInput_OnJumpAction(object sender, System.EventArgs e) {
    if (!IsGrounded) {
      return;
    }

    Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
  }

  private void OnDrawGizmos() {
    Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    Gizmos.color = Color.black;
  }
}