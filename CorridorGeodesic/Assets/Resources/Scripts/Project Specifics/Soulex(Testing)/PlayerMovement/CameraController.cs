using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private float x, y;

    [SerializeField] private float camSpeed = 2;
    [SerializeField] private Transform camHolder;
    [SerializeField] private Transform camTarget;
    [SerializeField] private AnimationCurve landCurve;

    private float offsetY;

    private float lockPercentage = 100;

    private float zRot;

    private void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerInput();
        HandleCamera();
    }
    void PlayerInput()
    {
        x += Input.GetAxisRaw("Mouse X") * camSpeed;
        y += Input.GetAxisRaw("Mouse Y") * camSpeed;

        y = Mathf.Clamp(y, -90, 90);
    }
    void HandleCamera()
    {
        camHolder.position = Vector3.Lerp(camHolder.position, camTarget.position + Vector3.up * offsetY, Time.deltaTime * 25 * lockPercentage/100);
        camHolder.rotation = Quaternion.Euler(-y, x, zRot);
    }
    public void SetLockPercentage(float value)
    {
        lockPercentage = value;
    }
    public Quaternion GetYRotation()
    {
        return Quaternion.Euler(0, x, 0);
    }
    public void SetZRot(float rot)
    {
        zRot = rot;
    }
    public IEnumerator DoLandEffect()
    {
        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime * 2f;
            offsetY = -landCurve.Evaluate(1 - timer);
            yield return null;
        }
        offsetY = 0;
    }
}