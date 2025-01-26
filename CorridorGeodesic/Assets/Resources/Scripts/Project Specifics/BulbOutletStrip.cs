#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProBuilder;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class BulbOutletStrip : MonoBehaviour, BulbCollisionBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float snapDistance;

    [Space, Header("EditorHelpers")]
    public ProBuilderMesh meshToModify;

    public Vector3 StripVectorNormalized => StripVector.normalized;
    public Vector3 StripVector => (endPoint.position - startPoint.position);

    public bool OnBulbCollision(Projectile_Vacumm bulb, RaycastHit hit)
    {
        //Get closest point between start and end in a line
        Vector3 attachPos = GetClosestPointOnLineSegment(hit.point, startPoint.position, endPoint.position);

        //Process position snapping
        attachPos = SnapPosition(attachPos);

        //Attach bulb to position
        bulb.Attach(attachPos, startPoint.forward);
        return true;
    }

    public Vector3 SnapPosition(Vector3 position)
    {
        if (snapDistance <= 0f)
            return position;

        Vector3 attachVector = position - startPoint.position;
        float snappedMagnitude = Mathf.Round(attachVector.magnitude / snapDistance) * snapDistance;
        attachVector = (attachVector.normalized) * snappedMagnitude;
        return startPoint.position + attachVector;
    }

#if UNITY_EDITOR
    public void EDITOR_StretchProbuilderMesh(Vector3 oldPoint, Vector3 newPoint, Vector3 otherPoint)
    {
        oldPoint = meshToModify.transform.InverseTransformPoint(oldPoint);
        newPoint = meshToModify.transform.InverseTransformPoint(newPoint);
        otherPoint = meshToModify.transform.InverseTransformPoint(otherPoint);

        Vector3 moveVector = newPoint - oldPoint;

        // Get mesh vertices
        var positions = meshToModify.GetVertices();

        // Move all vertices above the threshold
        for (int i = 0; i < positions.Length; i++)
        {
            Vertex vertex = positions[i];

            //if newPoint is closer
            if (Vector3.Distance(newPoint, vertex.position) <= Vector3.Distance(otherPoint, vertex.position))
                vertex.position = vertex.position + moveVector;
        }

        // Apply the changes to the mesh
        meshToModify.SetVertices(positions);
        meshToModify.ToMesh();
        meshToModify.Refresh();
    }
#endif

    // Finds the closest point on a line segment between start and end to a given point
    public static Vector3 GetClosestPointOnLineSegment(Vector3 point, Vector3 start, Vector3 end)
    {
        // Calculate the vector from start to end
        Vector3 lineVector = end - start;

        // Calculate the vector from start to the point
        Vector3 pointVector = point - start;

        // Calculate the projection of pointVector onto lineVector
        float lineLengthSquared = lineVector.sqrMagnitude; // Length of the line segment squared
        float dotProduct = Vector3.Dot(pointVector, lineVector); // Dot product of the vectors

        // Get the parameter t that represents how far along the line segment the projection falls
        float t = Mathf.Clamp01(dotProduct / lineLengthSquared);

        // Calculate the closest point using the parameter t
        return start + t * lineVector;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BulbOutletStrip))]
public class BulbOutletStripEditor : Editor
{
    private void OnSceneGUI()
    {
        BulbOutletStrip outletStrip = (BulbOutletStrip)target;
        Vector3 oldStartPos = outletStrip.startPoint.position;
        Vector3 oldEndPos = outletStrip.endPoint.position;

        // Start Handle
        EditorGUI.BeginChangeCheck();
        Vector3 newStartPosition = Handles.PositionHandle(oldStartPos, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            newStartPosition = outletStrip.SnapPosition(newStartPosition);
            newStartPosition = BulbOutletStrip.GetClosestPointOnLineSegment(
                newStartPosition, 
                oldStartPos + outletStrip.StripVectorNormalized * -20f,
                oldEndPos + outletStrip.StripVectorNormalized * 20f);
            if (Vector3.Distance(newStartPosition, oldEndPos) > 1f)
            {
                Object[] undoObjects = { outletStrip.GetComponent<ProBuilderMesh>(), outletStrip.startPoint };
                Undo.RecordObjects(undoObjects, "Moved Start Handle of Bulb Outlet Strip and modified ProBuilder Mesh");

                outletStrip.EDITOR_StretchProbuilderMesh(outletStrip.startPoint.position, newStartPosition, outletStrip.endPoint.position);
                outletStrip.startPoint.position = newStartPosition;

                UnityEditor.EditorUtility.SetDirty(outletStrip);
                UnityEditor.EditorUtility.SetDirty(outletStrip.startPoint);
                ProBuilderEditor.Refresh(true);
            }
        }

        // End Handle
        EditorGUI.BeginChangeCheck();
        Vector3 newEndPosition = Handles.PositionHandle(oldEndPos, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            newEndPosition = outletStrip.SnapPosition(newEndPosition);
            newEndPosition = BulbOutletStrip.GetClosestPointOnLineSegment(
                newEndPosition,
                oldStartPos + outletStrip.StripVectorNormalized * -20f,
                oldEndPos + outletStrip.StripVectorNormalized * 20f);
            if (Vector3.Distance(newEndPosition, oldStartPos) > 1f)
            {
                Object[] undoObjects = { outletStrip.GetComponent<ProBuilderMesh>(), outletStrip.endPoint };
                Undo.RecordObjects(undoObjects, "Moved End Handle of Bulb Outlet Strip and modified ProBuilder Mesh");

                outletStrip.EDITOR_StretchProbuilderMesh(outletStrip.endPoint.position, newEndPosition, outletStrip.startPoint.position);
                outletStrip.endPoint.position = newEndPosition;

                UnityEditor.EditorUtility.SetDirty(outletStrip);
                UnityEditor.EditorUtility.SetDirty(outletStrip.endPoint);
                ProBuilderEditor.Refresh(true);
            }
        }
    }
}
#endif