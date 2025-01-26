using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext : StateMachineContext
{
    [DebugReadOnly] public float x, z;
    [DebugReadOnly] public Vector3 moveVector;
    [DebugReadOnly] public Vector3 gravityVector;
    [DebugReadOnly] public Vector3 groundNormal;
    [DebugReadOnly] public Vector3 lastWallNormal;
    [DebugReadOnly] public Vector3 forceDirection;
    public Vector3 playerCenter => transform.position;
    public Vector3 playerCastCenter => transform.position - Vector3.up + Vector3.up * (playerController.height / 2);

    public CharacterController playerController;
    public Rigidbody playerRb;
    public CameraController camController;
    public Transform cameraTarget;

    public float crouchSpeed;
    public float walkSpeed;
    public float airSpeed;
    public float jumpHeight;
    public float slideSpeed;
    public float wallSlideSpeed;
    public float friction;
    public float staticFriction;

    public float crouchCamSpeed;

    [DebugReadOnly] public bool moving;
    [DebugReadOnly] public bool usingInput;
    [DebugReadOnly] public bool grounded;
    [DebugReadOnly] private bool wallRunning;

    [HideInInspector] public LayerMask ignoreMask;
    public PlayerStateEnum nextPState => (PlayerStateEnum)nextState;
    public PlayerStateEnum previousPState => (PlayerStateEnum)previousState;

    private float lastHitDistance;
    private float camLerp = 0;
    private float camLerpTarget = 0;
    private float camUpHeight = 0.75f;
    private float camDownHeight = 0;
    private float camZTarget;
    private float camZ;

    private void Awake()
    {
        ignoreMask = ~LayerMask.GetMask("Ignore Raycast", "Ignore Player", "Player", "Projectile");
    }
    private void Update()
    {
        grounded = Grounded();
        SetGroundNormal();

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        moveVector = (Vector3.Cross(transform.right, groundNormal) * z - Vector3.Cross(transform.forward, groundNormal) * x).normalized;

        moveVector *= Time.deltaTime;

        moving = moveVector.magnitude > 0;
        usingInput = x != 0 || z != 0;

        if (!wallRunning && moving)
            camZTarget = x * -2.5f;
        else if (!wallRunning)
            camZTarget = 0;

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, (moving && z > 0) ? 100 : 80, Time.deltaTime * 5);

        SetCamHeight();
        SetCamZ();
    }
    private bool Grounded()
    {
        return Physics.SphereCast(playerCenter, playerController.radius, Vector3.down, out RaycastHit hit, playerController.height / 2 + 0.05f, ignoreMask);
    }
    private void SetGroundNormal()
    {
        if (Physics.Raycast(playerCenter - Vector3.up * (playerController.height / 2 - 0.2f), Vector3.down, out RaycastHit hit, 25, ignoreMask))
        {
            groundNormal = hit.normal;
            lastHitDistance = hit.distance;
        }
        else if (lastHitDistance < 0.3f)
            groundNormal = Vector3.up;
    }
    private void SetCamHeight()
    {
        camLerp = Mathf.MoveTowards(camLerp, camLerpTarget, Time.deltaTime * crouchCamSpeed);
        cameraTarget.localPosition = new Vector3(0, Mathf.SmoothStep(camUpHeight, camDownHeight, camLerp), 0);
    }
    private void SetCamZ()
    {
        camZ = Mathf.Lerp(camZ, camZTarget, Time.deltaTime * 5);
        camController.SetZRot(camZ);
    }
    public void AddForce(Vector3 force)
    {
        forceDirection = force;
        SwitchState(CommonStateEnum.AddForce);
    }
    //Common Functions
    public void RotatePlayer()
    {
        transform.rotation = camController.GetYRotation();
    }
    public void Gravity()
    {
        gravityVector -= Vector3.up * 20 * Time.deltaTime;
        playerController.Move(gravityVector * Time.deltaTime);
    }
    public void GroundGravity()
    {
        gravityVector.y = Mathf.Lerp(gravityVector.y, 0, Time.deltaTime * 4);
        Gravity();
    }
    public void CameraLand()
    {
        camController.StartCoroutine(camController.DoLandEffect());
    }
    public void SetCamToCrouchHeight()
    {
        camLerpTarget = 1;
    }
    public void SetCamToStandHeight()
    {
        camLerpTarget = 0;
    }
    public void SetCamWallRun(float direction, float progress)
    {
        camZTarget = progress * direction * 15;
        wallRunning = direction != 0;
    }
    //public void CheckForWall()
    //{
    //    if (Input.GetKey(KeyCode.LeftControl))
    //        return;

    //    Collider[] cols = Physics.OverlapSphere(transform.position, playerController.radius + 0.2f, ignoreMask);

    //    foreach (Collider col in cols)
    //    {
    //        if (col.CompareTag("Wall"))
    //        {
    //            SwitchState(PlayerStateEnum.WallRun);
    //            break;
    //        }
    //    }
    //}
    public void AirMovement(ref Vector3 moveVelocity, float time)
    {
        float moveAllowance = time / 2;
        moveAllowance = Mathf.Clamp01(moveAllowance);

        //float moveAllowance = Mathf.Clamp(playerController.velocity.y, -15, 0)/-15;

        //moveAllowance = Mathf.Clamp01(moveAllowance);

        if (moving)
        {
            moveVelocity += moveVector * airSpeed * moveAllowance;

            if (moveVelocity.magnitude > airSpeed)
                moveVelocity *= airSpeed / moveVelocity.magnitude;
        }
        //else if (jumpTime > 0.2f)
        //{
        //    moveVelocity.x = Mathf.Abs(ctx.playerController.velocity.x) < Mathf.Abs(moveVelocity.x) ? ctx.playerController.velocity.x : moveVelocity.x;
        //    moveVelocity.z = Mathf.Abs(ctx.playerController.velocity.z) < Mathf.Abs(moveVelocity.z) ? ctx.playerController.velocity.z : moveVelocity.z;
        //}
    }
    //public void Grapple()
    //{
    //    SwitchState(PlayerStateEnum.Grapple);
    //}
    //public void UnGrapple()
    //{
    //    SwitchState(PlayerStateEnum.Airborne);
    //}
}