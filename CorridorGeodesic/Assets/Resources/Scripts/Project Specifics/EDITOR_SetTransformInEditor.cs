
using UnityEngine;

[ExecuteInEditMode]
public class EDITOR_SetTransformInEditor : MonoBehaviour
{

#if UNITY_EDITOR
    public bool setLossyScale = true;
    public Vector3 lossyScaleToKeep = Vector3.one;

    public bool setLocalPosition = true;
    public Vector3 localPositionToKeep = Vector3.zero;

    public void Update()
    {
        if (setLossyScale && transform.lossyScale != lossyScaleToKeep)
            SetLossyScale(lossyScaleToKeep);

        if (setLocalPosition && transform.localPosition != localPositionToKeep)
            transform.localPosition = localPositionToKeep;
    }

    private void SetLossyScale(Vector3 desiredLossyScale)
    {
        if (transform.parent != null)
        {
            Vector3 parentLossyScale = transform.parent.lossyScale;
            // Calculate the necessary local scale
            transform.localScale = new Vector3(
                desiredLossyScale.x / parentLossyScale.x,
                desiredLossyScale.y / parentLossyScale.y,
                desiredLossyScale.z / parentLossyScale.z
            );
        }
        else
        {
            // If no parent, local scale is the lossy scale
            transform.localScale = desiredLossyScale;
        }
    }
#endif

}
