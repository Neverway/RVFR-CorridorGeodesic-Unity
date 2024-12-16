using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWallJumpState : PlayerState
{
    private float jumpTime;
    private float enterYPos;
    private Vector3 moveVelocity;

    private Vector3 playerVelocity;
    public PWallJumpState(PlayerContext ctx) : base(ctx)
    {
    }
    public override void EnterState()
    {
        jumpTime = 0;
        enterYPos = ctx.transform.position.y;

        Vector3 wallNormal = ctx.lastWallNormal;
        ctx.lastWallNormal = Vector3.zero;

        //Vector3 wallNormal = Vector3.zero;
        //Collider[] cols = Physics.OverlapSphere(ctx.transform.position, ctx.playerController.radius + 0.2f, ctx.ignoreMask);

        //foreach (Collider col in cols)
        //{
        //    if (col.CompareTag("Wall"))
        //    {
        //        for (int i = 0; i < 8; i++)
        //        {
        //            Vector3 checkDir = Quaternion.AngleAxis(i * 45, Vector3.up) * Vector3.forward;
        //            if (Physics.Raycast(ctx.transform.position, checkDir, out RaycastHit hit, 1, ctx.ignoreMask))
        //            {
        //                if (hit.collider == col)
        //                {
        //                    wallNormal = hit.normal;
        //                    break;
        //                }
        //            }
        //        }
        //        ctx.SwitchState(PlayerStateEnum.Airborne);
        //    }
        //}

        float jumpForce = Mathf.Sqrt(-2 * Physics.gravity.y * ctx.jumpHeight);

        //Vector3 wishDir = ctx.transform.right * Input.GetAxisRaw("Horizontal");

        //if (Vector3.Dot(wishDir, wallNormal) < 0.5f)
        //    wishDir = Vector3.zero;

        //Vector3 forwardVelocity = ctx.playerController.velocity + wishDir * jumpForce;

        //moveVelocity = 1.5f * jumpForce * wallNormal + forwardVelocity;
        Vector3 forwardVelocity = ctx.playerController.velocity.magnitude < ctx.walkSpeed ? 
            ctx.playerController.velocity.normalized * ctx.walkSpeed : ctx.playerController.velocity;

        moveVelocity = 1.5f * jumpForce * wallNormal + forwardVelocity;

        moveVelocity.y = 0;

        ctx.gravityVector.y = jumpForce;
    }
    public override void ExitState()
    {
        ctx.playerController.Move(moveVelocity * Time.deltaTime + ctx.gravityVector * Time.deltaTime);
    }
    public override void UpdateState()
    {
        jumpTime += Time.deltaTime;

        if (ctx.transform.position.y < enterYPos)
            ctx.SwitchState(PlayerStateEnum.Airborne);
        if (ctx.grounded)
            ctx.SwitchState(PlayerStateEnum.Idle);

        ctx.AirMovement(ref moveVelocity, jumpTime);

        ctx.playerController.Move(moveVelocity * Time.deltaTime);

        ctx.Gravity();
        ctx.RotatePlayer();

        //if (jumpTime > 0.2f)
        //    ctx.CheckForWall();
    }
}