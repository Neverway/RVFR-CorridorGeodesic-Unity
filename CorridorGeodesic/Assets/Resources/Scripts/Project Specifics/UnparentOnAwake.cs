using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentOnAwake : MonoBehaviour
{
    public bool enableUnparent;
    [Tooltip("If checked, this component will stay parented if there is no CorGeo_ActorData or Mesh_Sliceable as a parent. " +
        "Check this as false if you want it to always unparent")]
    public bool onlyIfNecessaryForCorGeoActorDataOrMeshSliceable = false;
    private void Awake()
    {
        //Debug.Log($"I have an unparent script!", gameObject);
        if (!enableUnparent) return;
        if (onlyIfNecessaryForCorGeoActorDataOrMeshSliceable && transform.parent != null)
        {
            bool abortUnparent = true;

            if (transform.parent.GetComponentInParent<CorGeo_ActorData>() != null)
                abortUnparent = false;
            if (transform.parent.GetComponentInParent<Mesh_Slicable>() != null)
                abortUnparent = false;

            if (abortUnparent)
                return;
        }
        transform.SetParent(null);
        Destroy(this);
    }
}
