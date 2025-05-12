using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEditor;
using UnityEngine;
using System.Linq;

[ExecuteAlways]
public class SimplifiedFields : MonoBehaviour
{
#if UNITY_EDITOR
    //FieldContainer is just to make all the fields here into a single field itself
    //so that I can use the EasyDrawer class I made. This is bad, change this later
    [SerializeField] public FieldContainer fc;
    [System.Serializable] public struct FieldContainer
    {
        [SerializeField] public Category[] categories;
    }

    [System.Serializable]
    public struct Category
    {
        public string categoryName;
        [SerializeField] public FieldReference[] fields;
    }

    [System.Serializable]
    public struct FieldReference
    {
        public GameObject targetGameObject;
        public Component targetComponent;
        public string fieldName;

        public void Validate(SimplifiedFields target)
        {
            if (targetGameObject == null)
            {
                if (targetComponent == null)
                {
                    fieldName = null;
                    return;
                }
                targetGameObject = targetComponent.gameObject;
            }

            if (!targetGameObject.transform.IsChildOf(target.transform))
            {
                targetGameObject = null;
                targetComponent = null;
                fieldName = null;
            }
        }
    }

    //public void Update()
    //{
    //    FieldReference[] fieldReferences = fc.categories.SelectMany(cat => cat.fields).ToArray();
    //    foreach(FieldReference fr in fieldReferences)
    //        fr.Validate(this);
    //}
#endif
}

