using UnityEngine;

public class Player : MonoBehaviour {
  public static Player Instance;

  [Header("Move Information")]
  [SerializeField] private float moveSpeed;

  [SerializeField] private float jumpForce;
  [SerializeField] private float doubleJumpForce;

  [Header("Collition Info")]
  [SerializeField] private float groundCheckDistance;

  [SerializeField] private LayerMask WhatIsGround;
  [SerializeField] private Transform WallCheckTransform;
  [SerializeField] private Vector2 WallCheckSize;

  public Rigidbody2D Rb { get; private set; }
  private GameInput GameInput;

  public bool IsGrounded { get; private set; }
  public bool CanDoubleJump { get; private set; }
  public bool WallDetected { get; private set; }

  private void Awake() {
    Instance = this;
  }

  void Start() {
    Rb = GetComponent<Rigidbody2D>();

    GameInput = GameInput.Instance;

    GameInput.OnJumpAction += GameInput_OnJumpAction;
  }

  void Update() {
    HandleMovement();
    if (IsGrounded) {
      CanDoubleJump = true;
    }
  }

  private void HandleMovement() {
    IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, WhatIsGround);
    WallDetected = Physics2D.BoxCast(WallCheckTransform.position, WallCheckSize, 0, Vector2.zero, 0, WhatIsGround);

    Vector2 moveDirection = GameInput.GetMovementVector();
    if (moveDirection.x > 0) {
      Rb.velocity = new Vector2(moveSpeed, Rb.velocity.y);
    }

    if (moveDirection.x < 0) {
      Rb.velocity = new Vector2(moveSpeed * -1, Rb.velocity.y);
    }

    if (WallDetected) {
      Rb.velocity = new Vector2(0, Rb.velocity.y);
    }
  }

  private void GameInput_OnJumpAction(object sender, System.EventArgs e) {
    if (IsGrounded) {
      Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
    } else if (CanDoubleJump) {
      CanDoubleJump = false;
      Rb.velocity = new Vector2(Rb.velocity.x, doubleJumpForce);
    }
  }

  private void OnDrawGizmos() {
    Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    Gizmos.DrawWireCube(WallCheckTransform.position, WallCheckSize);
  }
}