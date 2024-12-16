using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PWallRunState : PlayerState
{
    private Vector3 wallNormal;
    private Vector3 runVelocity;
    private Vector3 upMove;

    private float runLength = 0.25f;
    private float runTimer;
    public PWallRunState(PlayerContext ctx) : base(ctx)
    {
    }
    public override void EnterState()
    {
        Vector3 velocity = ctx.playerController.velocity;
        velocity.y = 0;

        upMove = Vector3.up * 4;

        runTimer = 0;
        ctx.gravityVector.y = 0;
        Collider[] cols = Physics.OverlapSphere(ctx.transform.position, ctx.playerController.radius + 0.2f, ctx.ignoreMask);

        foreach (Collider col in cols)
        {
            if (col.CompareTag("Wall"))
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector3 checkDir = Quaternion.AngleAxis(i * 45, Vector3.up) * Vector3.forward;
                    if(Physics.Raycast(ctx.transform.position, checkDir, out RaycastHit hit, 2, ctx.ignoreMask))
                    {
                        if(hit.collider == col)
                        {
                            wallNormal = hit.normal;
                            break;
                        }
                    }
                }
                ctx.SwitchState(PlayerStateEnum.Airborne);
            }
        }

        runVelocity = Vector3.ProjectOnPlane(velocity, wallNormal);
        ctx.lastWallNormal = wallNormal;
    }
    public override void ExitState()
    {
        ctx.playerController.Move((runVelocity + upMove) * Time.deltaTime);
        ctx.gravityVector.y = -1;
        ctx.SetCamWallRun(0, 0);
    }
    public override void UpdateState()
    {
        if (runTimer < runLength)
            runTimer += Time.deltaTime;
        else
            runTimer = runLength;

        if (runTimer > 0.2f && ctx.grounded)
        {
            ctx.SwitchState(PlayerStateEnum.Idle);
            return;
        }
        if (Physics.Raycast(ctx.transform.position, -wallNormal, out RaycastHit hit, ctx.playerController.radius + 0.4f, ctx.ignoreMask))
        {
            if (!hit.collider.CompareTag("Wall"))
            {
                ctx.SwitchState(PlayerStateEnum.Airborne);
                return;
            }
        }
        else
        {
            ctx.SwitchState(PlayerStateEnum.Airborne);
            return;
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ctx.SwitchState(PlayerStateEnum.WallJump);
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ctx.SwitchState(PlayerStateEnum.Airborne);
            return;
        }

        float progress = runTimer / runLength;

        upMove -= Vector3.up * Time.deltaTime * 4;

        ctx.playerController.Move((runVelocity + upMove) * Time.deltaTime);

        ctx.RotatePlayer();
        ctx.SetCamWallRun(-Vector3.Dot(ctx.transform.right, wallNormal), progress);
    }
}