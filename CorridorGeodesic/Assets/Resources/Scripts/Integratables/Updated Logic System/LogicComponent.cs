//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose: Used for processing logic events for puzzle mechanics
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class LogicComponent : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private bool _isPowered;

    public delegate void PowerStateChanged(bool powered);
    public event PowerStateChanged OnPowerStateChanged;

    public bool isPowered
    {
        get { return _isPowered; }
        set
        {
            if (_isPowered != value)
            {
                _isPowered = value;

                OnPowerStateChanged?.Invoke(_isPowered);
                LocalPowerStateChanged(_isPowered);
            }
        }
    }

    //=-----------------=
    // Private Variables
    //=-----------------=
    private List<LogicComponent> subscribeLogicComponents = new List<LogicComponent>();

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    public virtual void Awake()
    {
        Debug.Log (gameObject.name);
        LogicComponentHandleInfo[] infos = LogicComponentHandleInfo.GetFromType(this.GetType());

        foreach (LogicComponentHandleInfo info in infos)
        {
            if (typeof(LogicComponent).IsAssignableFrom(info.field.FieldType))
                subscribeLogicComponents.Add((LogicComponent)info.field.GetValue(this));
            else if (typeof(IEnumerable<LogicComponent>).IsAssignableFrom(info.field.FieldType))
                subscribeLogicComponents.AddRange((IEnumerable<LogicComponent>)info.field.GetValue(this));
        }

        Subscribe();

        //AutoSubscribe();
    }
    public virtual void OnEnable()
    {
        //Fixes Animators to update its power state when re-enabled
        if (subscribeLogicComponents.Count > 0 && subscribeLogicComponents.TrueForAll(s => s))
            SourcePowerStateChanged(isPowered);

        OnPowerStateChanged?.Invoke(isPowered);
    }

    protected void OnDestroy()
    {
        if (subscribeLogicComponents.Count <= 0)
            return;
        subscribeLogicComponents.ForEach(component =>
        {
            if (component)
                component.OnPowerStateChanged -= SourcePowerStateChanged;
        });

        isPowered = false;
    }
#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        LogicComponentHandleInfo[] infos = LogicComponentHandleInfo.GetFromType(this.GetType());

        foreach (LogicComponentHandleInfo info in infos)
        {
            if (typeof(LogicComponent).IsAssignableFrom(info.field.FieldType))
                DrawLinkArrow((LogicComponent)info.field.GetValue(this), info.attribute);

            else if (typeof(IEnumerable<LogicComponent>).IsAssignableFrom(info.field.FieldType))
                foreach (LogicComponent logic in (IEnumerable<LogicComponent>)info.field.GetValue(this))
                    DrawLinkArrow(logic, info.attribute);
        }
    }
    
    void DrawLinkArrow(LogicComponent from, LogicComponentHandleAttribute handle)
    {
        //todo: Try making this method not the ugliest thing you've ever seen? It draws fancy arrows tho!

        if (from == null) return;

        Vector3 targetPos = transform.TransformPoint(new Vector3(handle.xOffset, handle.yOffset, handle.zOffset));
        Vector3 startPos = from.transform.position;

        Vector3 dir = startPos - targetPos;
        dir.Normalize();
        Handles.color = (from.isPowered ? Color.cyan : Color.blue);
        Handles.DrawLine(startPos, targetPos, 2f);
        Vector3 crossVector = Vector3.Cross(dir, SceneView.lastActiveSceneView.camera.transform.forward);

        float time = 1f - ((Time.time * 0.2f) % 1f);
        Vector3 arrowPos = Vector3.Lerp(targetPos, startPos, time);
        Handles.DrawLine(arrowPos, arrowPos + Vector3.Lerp(dir, crossVector * 1f, 0.4f) * (0.3f + 0.4f * Mathf.Sin(Mathf.PI * time)), 4f);
        Handles.DrawLine(arrowPos, arrowPos + Vector3.Lerp(dir, crossVector * -1f, 0.4f) * (0.3f + 0.4f * Mathf.Sin(Mathf.PI * time)), 4f);

        time = 1f - (((Time.time * 0.2f) + 0.33f) % 1f);
        arrowPos = Vector3.Lerp(targetPos, startPos, time);
        Handles.DrawLine(arrowPos, arrowPos + Vector3.Lerp(dir, crossVector * 1f, 0.4f) * (0.3f + 0.4f * Mathf.Sin(Mathf.PI * time)), 4f);
        Handles.DrawLine(arrowPos, arrowPos + Vector3.Lerp(dir, crossVector * -1f, 0.4f) * (0.3f + 0.4f * Mathf.Sin(Mathf.PI * time)), 4f);

        time = 1f - (((Time.time * 0.2f) + 0.66f) % 1f);
        arrowPos = Vector3.Lerp(targetPos, startPos, time);
        Handles.DrawLine(arrowPos, arrowPos + Vector3.Lerp(dir, crossVector * 1f, 0.4f) * (0.3f + 0.4f * Mathf.Sin(Mathf.PI * time)), 4f);
        Handles.DrawLine(arrowPos, arrowPos + Vector3.Lerp(dir, crossVector * -1f, 0.4f) * (0.3f + 0.4f * Mathf.Sin(Mathf.PI * time)), 4f);
    }
#endif

    //=-----------------=
    // Internal Functions
    //=-----------------=
    //public virtual void AutoSubscribe()
    //{
    //    Subscribe();
    //}
    public virtual void SourcePowerStateChanged(bool powered)
    {

    }
    public virtual void LocalPowerStateChanged(bool powered)
    {

    }
    void Subscribe()
    {
        if (subscribeLogicComponents.Count <= 0)
            return;
        subscribeLogicComponents.ForEach(component =>
        {
            if (component)
                component.OnPowerStateChanged += SourcePowerStateChanged;
        });
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
