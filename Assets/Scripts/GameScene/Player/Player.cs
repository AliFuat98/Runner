using UnityEngine;

public class Player : MonoBehaviour {
  public static Player Instance;

  [Header("Move Information")]
  [SerializeField] private float moveSpeed;

  [SerializeField] private float jumpForce;
  [SerializeField] private float doubleJumpForce;

  [Header("Slide Information")]
  [SerializeField] private float maxSlideTime;

  [SerializeField] private float slideSpeed;

  [Header("Collition Info")]
  [SerializeField] private float groundCheckDistance;

  [SerializeField] private float ceillingCheckDistance;

  [SerializeField] private LayerMask WhatIsGround;
  [SerializeField] private Transform WallCheckTransform;
  [SerializeField] private Vector2 WallCheckSize;

  public Rigidbody2D Rb { get; private set; }
  private GameInput GameInput;
  private float slideTimer;

  public bool IsGrounded { get; private set; }

  private bool xIsSliding;
  public bool IsSliding {
    get {
      return xIsSliding;
    }
    private set {
      if (CeillingWallDetected) {
        xIsSliding = true;
      } else {
        xIsSliding = value;
      }
    }
  }

  public bool CanDoubleJump { get; private set; }
  public bool WallDetected { get; private set; }
  public bool CeillingWallDetected { get; private set; }

  private void Awake() {
    Instance = this;
  }

  void Start() {
    Rb = GetComponent<Rigidbody2D>();
    GameInput = GameInput.Instance;
    GameInput.OnJumpAction += GameInput_OnJumpAction;
    GameInput.OnSlideAction += GameInput_OnSlideAction;
  }

  void Update() {
    HandleCast();
    HandleMovement();
    HandleJump();
    HandleSliding();
  }

  private void HandleJump() {
    if (IsGrounded) {
      CanDoubleJump = true;
    }
  }

  private void HandleCast() {
    IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, WhatIsGround);
    CeillingWallDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, WhatIsGround);
    WallDetected = Physics2D.BoxCast(WallCheckTransform.position, WallCheckSize, 0, Vector2.zero, 0, WhatIsGround);
    if (IsSliding) {
      WallDetected = false;
    }
  }

  private void HandleMovement() {
    Vector2 moveDirection = GameInput.GetMovementVector();
    if (moveDirection.x > 0) {
      Rb.velocity = new Vector2(moveSpeed, Rb.velocity.y);
    } else {
      Rb.velocity = new Vector2(0, Rb.velocity.y);
    }

    if (WallDetected) {
      Rb.velocity = new Vector2(0, Rb.velocity.y);
    }
  }

  private void HandleSliding() {
    if (!IsSliding) {
      return;
    }

    slideTimer -= Time.deltaTime;
    if (slideTimer < 0) {
      IsSliding = false;
    }

    if (IsGrounded) {
      Rb.velocity = new Vector2(slideSpeed, Rb.velocity.y);
    }
  }

  private void GameInput_OnJumpAction(object sender, System.EventArgs e) {
    if (CeillingWallDetected) {
      return;
    }
    IsSliding = false;

    if (IsGrounded) {
      Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
    } else if (CanDoubleJump) {
      CanDoubleJump = false;
      Rb.velocity = new Vector2(Rb.velocity.x, doubleJumpForce);
    }
  }

  private void GameInput_OnSlideAction(object sender, System.EventArgs e) {
    if (Rb.velocity.x == 0) {
      return;
    }

    if (!IsSliding) {
      IsSliding = true;
      slideTimer = maxSlideTime;
    }
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.black;
    Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
    Gizmos.DrawWireCube(WallCheckTransform.position, WallCheckSize);
  }
}