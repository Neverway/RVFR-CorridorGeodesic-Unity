using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vent : LogicComponent
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Transform airCurrent;
    public Transform sparks;
    //public CorGeo_ActorData airStart;
    //public CorGeo_ActorData airEnd;

    [Space]
    public Transform[] ventBlades;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField, LogicComponentHandle] private LogicComponent inputSignal;
    [SerializeField, LogicComponentHandle] private LogicComponent isBeingSliced;

    //=-----------------=
    // Reference Variables
    //=-----------------='
    [Header("References")]
    [SerializeField] private Animator animator;
    //[SerializeField] private Material poweredMaterial, unpoweredMaterial;
    //[SerializeField] private GameObject indicatorMesh;
    private UnityEvent onPowered;
    private UnityEvent onUnpowered;

    //private float storedAirMagnitude;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    //protected new void Awake()
    //{
    //    base.Awake();
    //    Vector3 airVector = airEnd.transform.position - airStart.transform.position;
    //    storedAirMagnitude = airVector.magnitude / 5f;
    //    airEnd.transform.position = airStart.transform.position + (airVector * 0.1f);
    //}
    protected new void OnEnable()
    {
        base.OnEnable();
        SourcePowerStateChanged(isPowered);
    }
    [ExecuteAlways]
    protected void Update()
    {
        airCurrent.gameObject.SetActive(isPowered);
        sparks.gameObject.SetActive(isBeingSliced.isPowered);

        UpdateAirCurrent();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void SetBladesAnimation(bool isOpen)
    {
        float targetBladeAngle = (isOpen ? -20f : -90f);
        foreach (Transform t in ventBlades)
        {
            t.DOKill();
            t.DOLocalRotate(new Vector3(targetBladeAngle, 0, 0), 1f);
        }
    }
    public void UpdateAirCurrent()
    {
        //if (!isPowered)
        //    return;
        //
        //Vector3 airDireciton = airEnd.transform.position - airStart.transform.position;
        //
        //Alt_Item_Geodesic_Utility_GeoGun.
        //
        ////position
        //airCurrent.transform.position = airStart.transform.position;
        ////rotation
        //airCurrent.transform.LookAt(airEnd.transform.position);
        ////scale
        //Vector3 newScale = airCurrent.transform.localScale;
        //newScale.z = storedAirMagnitude;
        //airCurrent.transform.localScale = newScale;
    }

    //=-----------------=
    // External Functions
    //=-----------------=
    public override void SourcePowerStateChanged(bool powered)
    {
        base.SourcePowerStateChanged(powered);

        if(inputSignal == null)
        {
            isPowered = !isBeingSliced.isPowered;
            SetBladesAnimation(isPowered);
            return;
        }


        isPowered = inputSignal.isPowered && !isBeingSliced.isPowered;

        if (isPowered)
            onPowered?.Invoke();
        else
            onUnpowered?.Invoke();

        SetBladesAnimation(isPowered);
    }
}
