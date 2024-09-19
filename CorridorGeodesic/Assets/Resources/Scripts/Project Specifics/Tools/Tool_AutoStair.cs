//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
#if UNITY_EDITOR
public class Tool_AutoStair : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 stairStartPosition;
    private Vector3 stairEndPosition;

    private Vector3 stairVector;

    private int _platformCount;
    private int platformCount
    {
        get { return _platformCount; }
        set 
        {
            if(_platformCount != value)
            {
                _platformCount = value;

                ChangeStairAmount();
            }
        }
    }

    private bool active => stairStart && stairEnd;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Transform stairStart;
    [SerializeField] private Transform stairEnd;

    [SerializeField] private GameObject stairPlatform;
    [SerializeField] private GameObject stairSupport;
    [SerializeField] private GameObject stairSupportBottom;

    [SerializeField] private List<Platform> platforms = new List<Platform>();

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (!active)
            return;

        stairStartPosition = new Vector3(stairStart.position.x, transform.position.y, stairStart.position.z);
        stairEndPosition = new Vector3(stairEnd.position.x, transform.position.y, stairEnd.position.z);

        stairVector = stairEndPosition - stairStartPosition;

        stairStartPosition += stairVector.normalized * 0.5f;
        stairEndPosition += stairVector.normalized * 0.5f;

        stairVector = stairEndPosition - stairStartPosition;

        platformCount = Mathf.FloorToInt(stairVector.magnitude);
    }
    private void OnDrawGizmos()
    {
        if (!active)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(stairStartPosition, stairEndPosition);
        Gizmos.DrawLine(stairStartPosition + Vector3.up * stairVector.magnitude * 0.5f, stairEndPosition);

        stairVector = stairEndPosition - stairStartPosition;

        Gizmos.color = Color.blue;

        for (int i = 0; i <= platformCount; i++)
        {
            Vector3 xzPos = stairVector.normalized * i;
            Vector3 pos = stairStartPosition + new Vector3(xzPos.x, (platformCount - i) * 0.5f, xzPos.z);

            Gizmos.DrawSphere(pos, 0.3f);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void ChangeStairAmount()
    {
        if (platforms.Count > 0)
        {
            int _platformCount = platforms.Count;
            for (int i = 0; i < _platformCount; i++)
            {
                if (platforms[0] != null)
                    DestroyImmediate(platforms[0].platform);

                platforms.RemoveAt(0);
            }
        }

        for (int i = 0; i < platformCount; i++)
        {
            if (platforms.Count >= platformCount)
                break;

            Vector3 bottomPos = stairStartPosition + stairVector.normalized * i;
            float height = (platformCount - i) * 0.5f;

            new Platform(transform, bottomPos, stairVector.normalized, height, stairPlatform, stairSupport, stairSupportBottom, this);
        }
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public void AddPlatform(Platform platform)
    {
        platforms.Add(platform);
    }
}
[System.Serializable]
public class Platform
{
    //private GameObject stairPlatform;
    //private List<GameObject> stairSupports = new List<GameObject>();
    //private GameObject stairSupportBottom;

    public GameObject platform;

    public Platform(Transform parent, Vector3 bottomPosition, Vector3 forward, float height, 
        GameObject stairPlatform, GameObject stairSupport, GameObject stairSupportBottom, Tool_AutoStair autoStair)
    {
        platform = new GameObject("Platform");

        Vector3 right = Vector3.Cross(forward, Vector3.up);
        //Vector3 up = Vector3.Cross(forward, right);

        Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);

        MonoBehaviour.Instantiate(stairPlatform, bottomPosition + Vector3.up * height, rotation, platform.transform);

        int supportCount = Mathf.CeilToInt(height/4);

        for (int i = 0; i < supportCount; i++)
        {
            Vector3 bottomPos = bottomPosition + Vector3.up * (height - 0.2f) + Vector3.up * -4 * i;

            for (int x = 0; x < 2; x++)
            {
                float unitFactor = (x - 0.5f) * 2;

                MonoBehaviour.Instantiate(stairSupport, bottomPos + right * unitFactor * 1.5f, rotation, platform.transform);
                MonoBehaviour.Instantiate(stairSupportBottom, bottomPosition + right * unitFactor * 1.5f, rotation, platform.transform);
            }
        }

        platform.transform.parent = parent;

        autoStair.AddPlatform(this);
    }
}
#endif