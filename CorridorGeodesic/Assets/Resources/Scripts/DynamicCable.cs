//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Neverway.Framework.LogicSystem
{
    public class DynamicCable : LogicComponent
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [LogicComponentHandle] public LogicComponent inputSignal;

        public bool
            generateWaypointsUsingLength; // Auto-generate waypoints based on the distance between the two anchors

        public float waypointsPerUnit; // How many waypoints should be created per unit of distance between the anchors

        public int
            extraWaypoints; //How many extra waypoints to be added after waypointsPerUnity calculation (Increases slack in cable)

        public bool updateOnMove;

        //=-----------------=
        // Private Variables
        //=-----------------=
        private List<GameObject> waypoints = new List<GameObject>();
        private List<SpringJoint> waypointSpringJoints = new List<SpringJoint>();
        private Vector3 lastAnchorAPosition, lastAnchorBPosition;
        private Rigidbody anchorPointARigidBody, anchorPointBRigidBody;
        private float lastCableDistance;
        private float onRiftClosedTimer;

        private float ActualLerpAmount =>
            (Alt_Item_Geodesic_Utility_GeoGun.deployedRift ? Alt_Item_Geodesic_Utility_GeoGun.lerpAmount : 0f);

        //=-----------------=
        // Reference Variables
        //=-----------------=
        [SerializeField] private Material cableUnpowered, cablePowered;
        [SerializeField] private GameObject anchorPointA, anchorPointB;
        [SerializeField] private GameObject waypointsRoot;

        [SerializeField]
        private GameObject waypointReference; // Used as the instantiation reference when auto-generating cable

        private LineRenderer lineRenderer;


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            //if (!generateWaypointsUsingLength) GatherWaypoints();
            if (generateWaypointsUsingLength)
            {
                GenerateWaypoints();
                //GatherWaypoints();
            }

            anchorPointARigidBody = anchorPointA.GetComponent<Rigidbody>();
            anchorPointBRigidBody = anchorPointB.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (inputSignal)
            {
                if (isPowered != inputSignal.isPowered)
                    SourcePowerStateChanged(inputSignal.isPowered);
            }


            if (AnchorPointsMoved() && generateWaypointsUsingLength && updateOnMove)
            {
                GenerateWaypoints();
                //GatherWaypoints();
            }

            bool changeMeshCollider = onRiftClosedTimer > 0f;
            if (ActualLerpAmount != 0f)
                onRiftClosedTimer = 0.1f;
            else
                onRiftClosedTimer -= Time.deltaTime;


            for (int i = 0; i < waypoints.Count; i++)
            {
                lineRenderer.SetPosition(i, waypoints[i].transform.position);

                if (changeMeshCollider)
                {
                    Collider collider = waypoints[i].GetComponent<Collider>();
                    if (collider != null)
                        collider.enabled = !(onRiftClosedTimer > 0f);
                }
            }
        }

        private void LateUpdate()
        {
            anchorPointARigidBody.isKinematic = true;
            anchorPointBRigidBody.isKinematic = true;
        }


#if UNITY_EDITOR
        private new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (UnityEngine.Application.isPlaying)
                return;

            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
                if (lineRenderer == null)
                    return;
            }

            Vector3 positionA = anchorPointA.transform.position;
            Vector3 positionB = anchorPointB.transform.position;

            lineRenderer.positionCount =
                GetWaypointCount(Vector3.Distance(anchorPointA.transform.position, anchorPointB.transform.position));
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                float factor = ((float)i) / ((float)(lineRenderer.positionCount - 1));
                Vector3 position = Vector3.Lerp(positionA, positionB, factor);
                position += Vector3.down * Mathf.Sin(factor * Mathf.PI) * (extraWaypoints * 0.2f + 1f);
                lineRenderer.SetPosition(i, position);
            }
        }
#endif

        //=-----------------=
        // Internal Functions
        //=-----------------=
        /// <summary>
        /// Create waypoints based on the distance between the two anchors
        /// </summary>
        private void GenerateWaypoints()
        {
            Vector3[] oldWaypointPositions = new Vector3[waypointsRoot.transform.childCount];


            // Destroy all the existing waypoints
            for (int i = 0; i < waypointsRoot.transform.childCount; i++)
            {
                Transform child = waypointsRoot.transform.GetChild(i);
                oldWaypointPositions[i] = child.position;
                Destroy(child.gameObject);
            }

            waypoints.Clear();
            waypointSpringJoints.Clear();

            // Get the distance between the two anchors
            var distance = Vector3.Distance(anchorPointA.transform.position, anchorPointB.transform.position);

            // Calculate the total number of waypoints
            int waypointCount = GetWaypointCount(distance);

            // Calculate the increment for each waypoint
            Vector3 direction = (anchorPointB.transform.position - anchorPointA.transform.position).normalized;
            float
                step = distance /
                       (waypointCount +
                        1); // Adding 1 to waypointCount to avoid placing a waypoint at anchorB's position

            // Create new waypoints based on the length and waypoints per unit
            if (anchorPointA) waypoints.Add(anchorPointA);

            for (int i = 1; i <= waypointCount; i++)
            {
                Vector3 waypointPosition = anchorPointA.transform.position + direction * (step * i);
                //lineRenderer.get
                if (i < oldWaypointPositions.Length)
                {
                    waypointPosition = oldWaypointPositions[i];
                }
                else if (oldWaypointPositions.Length > 0)
                    waypointPosition = oldWaypointPositions[oldWaypointPositions.Length - 1];

                var newWaypoint = Instantiate(waypointReference, waypointPosition, Quaternion.identity,
                    waypointsRoot.transform);
                newWaypoint.SetActive(true);
                //newWaypoint.GetComponent<ConfigurableJoint>().connectedBody = waypoints[waypoints.Count - 1].GetComponent<Rigidbody>();
                SpringJoint newWaypointSpring = newWaypoint.GetComponent<SpringJoint>();
                newWaypointSpring.connectedBody = waypoints[waypoints.Count - 1].GetComponent<Rigidbody>();
                newWaypointSpring.maxDistance = (distance / ((waypointCount - extraWaypoints) * 3f));
                waypoints.Add(newWaypoint);
                waypointSpringJoints.Add(newWaypointSpring);
            }

            if (anchorPointB)
            {
                SpringJoint anchorPointBSpringJoint = anchorPointB.GetComponent<SpringJoint>();
                anchorPointBSpringJoint.connectedBody = waypoints[waypoints.Count - 1].GetComponent<Rigidbody>();
                anchorPointBSpringJoint.maxDistance = (distance / ((waypointCount - extraWaypoints) * 3f));
                waypoints.Add(anchorPointB);
                waypointSpringJoints.Add(anchorPointBSpringJoint);
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

        /// <summary>
        /// Returns True when anchor points have moved enough distance that it should create or remove an anchor point
        /// </summary>
        /// <returns></returns>
        private bool AnchorPointsMoved()
        {
            if (anchorPointA.transform.position != lastAnchorAPosition ||
                anchorPointB.transform.position != lastAnchorBPosition)
            {
                lastAnchorAPosition = anchorPointA.transform.position;
                lastAnchorBPosition = anchorPointB.transform.position;

                float newCableDistance =
                    Vector3.Distance(anchorPointA.transform.position, anchorPointB.transform.position);
                if (GetWaypointCount(newCableDistance) != GetWaypointCount(lastCableDistance))
                {
                    lastCableDistance = newCableDistance;
                    return true;
                }

                return false;
            }

            return false;
        }

        private int GetWaypointCount(float distance)
        {
            int waypointCount = Mathf.RoundToInt(distance * waypointsPerUnit) + extraWaypoints;
            if (waypointCount < 0) waypointCount = 0;

            return waypointCount;
        }

        //=-----------------=
        // External Functions
        //=-----------------=
        //public override void AutoSubscribe()
        //{
        //    subscribeLogicComponents.Add(inputSignal);
        //    base.AutoSubscribe();
        //}
        public override void SourcePowerStateChanged(bool powered)
        {
            base.SourcePowerStateChanged(powered);

            isPowered = powered;

            SetCablePowered(isPowered);
        }

        public void SetCablePowered(bool _isPowered)
        {
            if (!lineRenderer)
                return;

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
}
