using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
  private const string IS_GROUNDED = "IsGrounded";
  private const string Y_Velocity = "YVelocity";
  private const string X_Velocity = "XVelocity";
  private const string CAN_DOUBLE_JUMP = "CanDoubleJump";
  private const string IS_SLIDING = "IsSliding";

  private Animator animator;
  private Player player;

  void Start() {
    animator = GetComponent<Animator>();
    player = GetComponentInParent<Player>();
  }

  void Update() {
    animator.SetBool(IS_GROUNDED, player.IsGrounded);
    animator.SetBool(CAN_DOUBLE_JUMP, player.CanDoubleJump);

    animator.SetBool(IS_SLIDING, player.IsSliding);

    animator.SetFloat(X_Velocity, player.Rb.velocity.x);

    //float yVelocity = Mathf.Clamp(player.Rb.velocity.y, -1f, 1f);
    animator.SetFloat(Y_Velocity, player.Rb.velocity.y);
  }

}