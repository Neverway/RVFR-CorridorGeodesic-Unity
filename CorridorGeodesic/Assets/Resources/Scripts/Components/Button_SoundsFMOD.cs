//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Plays sound effects when hovering or selecting a UI button
// Notes: Requires FMOD events for "hover" and "select"
//
//=============================================================================

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Plays sound effects when hovering or selecting a UI button
/// </summary>
[RequireComponent(typeof(Button))]
public class Button_SoundsFMOD : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Button button;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioClip select;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter( PointerEventData _pointerEventData ) 
    {
        Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.hover);
        if (audioSource) audioSource.PlayOneShot(hover);
    }

    public void OnPointerDown( PointerEventData _pointerEventData ) 
    {
        Audio_FMODAudioManager.PlayOneShot(Audio_FMODEvents.Instance.select);
        if (audioSource) audioSource.PlayOneShot(select);
    }    
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}