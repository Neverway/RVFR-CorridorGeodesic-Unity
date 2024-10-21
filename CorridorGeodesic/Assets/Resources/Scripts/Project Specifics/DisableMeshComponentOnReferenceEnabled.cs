using UnityEngine;

public class DisableMeshComponentOnReferenceEnabled : MonoBehaviour
{
    public GameObject referencedGameObject;
    public bool ignoreHierarchy;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;

    private void Update()
    {
        if (referencedGameObject != null)
        {
            if (meshRenderer != null)
                meshRenderer.enabled = ignoreHierarchy ? 
                    referencedGameObject.activeSelf : referencedGameObject.activeInHierarchy;

            if (meshCollider != null)
                meshCollider.enabled = ignoreHierarchy ? 
                    referencedGameObject.activeSelf : referencedGameObject.activeInHierarchy;
        }
    }
}
