//=-------- Script by Errynei ----------=//
using Neverway.Framework.LogicValueSystem;
using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A serialized reference to a field of type T from a <see cref="Component"/>. 
/// <para>Use this if you need a reference to a field of another <see cref="Component"/>, 
/// but you want that reference to be serialized and to be locally consistent between
/// instances of prefabs</para>
/// </summary>
/// <typeparam name="T">Type of the field you want to reference. Use <see cref="object"/> to accept all types</typeparam>
[Serializable]
public struct ComponentFieldReference<T>
{
#if UNITY_EDITOR
    [SerializeField] private GameObject EDITOR_targetGameObject;
    [SerializeField] private bool EDITOR_hideTypeFilterText;
#endif
    [SerializeField] private Component targetComponent;
    [SerializeField] private string fieldName;

    [NonSerialized] private T cachedValue;
    [NonSerialized] private bool hasCached;
    [NonSerialized] private FieldInfo _field;

    public ComponentFieldReference(Component target, string field)
    {
#if UNITY_EDITOR
        EDITOR_hideTypeFilterText = false;
        if (target == null)
            EDITOR_targetGameObject = null;
        else
            EDITOR_targetGameObject = target.gameObject;
#endif
        targetComponent = target;
        fieldName = field;
        cachedValue = default;
        hasCached = false;
        _field = null;
    }

    /// <summary>
    /// Cached <see cref="FieldInfo"/> of the referenced field
    /// </summary>
    public FieldInfo Field
    {
        get
        {
            if (_field == null)
            {
                if (IsUndefined)
                    throw new Exception("ComponentFieldReference is not defined, cannot get value");

                _field = targetComponent.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (_field == null)
                    throw new Exception($"ComponentFieldReference could not find field {fieldName}");
            }
            return _field;
        }
    }
    /// <summary>
    /// Whether or not this <see cref="ComponentFieldReference{}"/> has a target <see cref="Component"/> and field name to reference
    /// </summary>
    public bool IsUndefined => (targetComponent == null || string.IsNullOrEmpty(fieldName));


    /// <summary>
    /// Uses reflection to get the value directly from the field
    /// </summary>
    public T Get()
    {
#if UNITY_EDITOR
        _field = null;
#endif
        cachedValue = (T)Field.GetValue(targetComponent);
        hasCached = true;
        return cachedValue;
    }

    /// <summary>
    /// Gets the targeted Component
    /// </summary>
    public Component GetTargetComponent() => targetComponent;

    /// <summary>
    /// Gets the last value returned from <see cref="Get"/>.
    /// <para>If a value has not been cached, will call <see cref="Get"/> instead</para>
    /// </summary>
    public T GetCached() => 
#if UNITY_EDITOR
        Get();
#else
    (hasCached ? cachedValue : Get());
#endif

    /// <summary>
    /// Uses reflection to set the value directly to the field
    /// </summary>
    /// <param name="value">new </param>
    public void Set(T value)
    {
        _field.SetValue(targetComponent, value);
        cachedValue = value;
    }
}