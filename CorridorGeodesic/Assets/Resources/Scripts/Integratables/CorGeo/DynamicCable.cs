//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class DynamicCable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public NEW_LogicProcessor inputSignal;
    public bool generateWaypointsUsingLength; // Auto-generate waypoints based on the distance between the two anchors
    public float waypointsPerUnit; // How many waypoints should be created per unit of distance between the anchors
    public bool updateOnMove;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private List<GameObject> waypoints = new List<GameObject>();
    private Vector3 lastAnchorAPosition, lastAnchorBPosition;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private NEW_LogicProcessor logicProcessor;
    [SerializeField] private Material cableUnpowered, cablePowered;
    [SerializeField] private GameObject anchorPointA, anchorPointB;
    [SerializeField] private GameObject waypointsRoot;
    [SerializeField] private GameObject waypointReference; // Used as the instantiation reference when auto-generating cable
    private LineRenderer lineRenderer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<NEW_LogicProcessor>();
        lineRenderer = GetComponent<LineRenderer>();
        //if (!generateWaypointsUsingLength) GatherWaypoints();
        if (generateWaypointsUsingLength)
        {
            GenerateWaypoints();
            //GatherWaypoints();
        }
    }

    private void Update()
    {
        if (inputSignal)
        {
            logicProcessor.isPowered = inputSignal.isPowered;
            if (logicProcessor.hasPowerStateChanged)
            {
                SetCablePowered(logicProcessor.isPowered);
            }
        }
        
        if (AnchorPointsMoved() && generateWaypointsUsingLength && updateOnMove)
        {
            GenerateWaypoints();
            //GatherWaypoints();
        }
        
        for (int i = 0; i < waypoints.Count; i++)
        {
            lineRenderer.SetPosition(i, waypoints[i].transform.position);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    /// <summary>
    /// Create waypoints based on the distance between the two anchors
    /// </summary>
    private void GenerateWaypoints()
    {
        // Destroy all the existing waypoints
        for (int i = 0; i < waypointsRoot.transform.childCount; i++)
        {
            Destroy(waypointsRoot.transform.GetChild(i).gameObject);
        }
        waypoints.Clear();

        // Get the distance between the two anchors
        var distance = Vector3.Distance(anchorPointA.transform.position, anchorPointB.transform.position);

        // Calculate the total number of waypoints
        int waypointCount = Mathf.RoundToInt(distance * waypointsPerUnit);
    
        // Calculate the increment for each waypoint
        Vector3 direction = (anchorPointB.transform.position - anchorPointA.transform.position).normalized;
        float step = distance / (waypointCount + 1); // Adding 1 to waypointCount to avoid placing a waypoint at anchorB's position

        // Create new waypoints based on the length and waypoints per unit
        if (anchorPointA) waypoints.Add(anchorPointA);

        for (int i = 1; i <= waypointCount; i++)
        {
            Vector3 waypointPosition = anchorPointA.transform.position + direction * (step * i);
            var newWaypoint = Instantiate(waypointReference, waypointPosition, Quaternion.identity, waypointsRoot.transform);
            newWaypoint.SetActive(true);
            newWaypoint.GetComponent<ConfigurableJoint>().connectedBody = waypoints[waypoints.Count - 1].GetComponent<Rigidbody>();
            newWaypoint.GetComponent<SpringJoint>().connectedBody = waypoints[waypoints.Count - 1].GetComponent<Rigidbody>();
            waypoints.Add(newWaypoint);
        }

        if (anchorPointB)
        {
            anchorPointB.GetComponent<SpringJoint>().connectedBody = waypoints[waypoints.Count - 1].GetComponent<Rigidbody>();
            waypoints.Add(anchorPointB);
        }

        lineRenderer.positionCount = waypoints.Count;
    }
    
    /// <summary>
    /// Get all the waypoints under the waypoint root
    /// </summary>
    private void GatherWaypoints()
    {
        waypoints.Clear();
        if (anchorPointA) waypoints.Add(anchorPointA);
        for (int i = 0; i < waypointsRoot.transform.childCount; i++)
        {
            waypoints.Add(waypointsRoot.transform.GetChild(i).gameObject);
        }
        if (anchorPointB) waypoints.Add(anchorPointB);
        lineRenderer.positionCount = waypoints.Count;
    }

    private bool AnchorPointsMoved()
    {
        if (anchorPointA.transform.position != lastAnchorAPosition || anchorPointB.transform.position != lastAnchorBPosition)
        {
            lastAnchorAPosition = anchorPointA.transform.position;
            lastAnchorBPosition = anchorPointB.transform.position;
            return true;
        }
        return false;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetCablePowered(bool _isPowered)
    {
        if (_isPowered)
        {
            lineRenderer.material = cablePowered;
        }
        if (!_isPowered)
        {
            lineRenderer.material = cableUnpowered;
        }
    }
}
