//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Sounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
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
        print("1");
        audioSource.clip = hover;
        audioSource.Play();
    }

    public void OnPointerDown( PointerEventData _pointerEventData ) 
    {
        print("2");
        audioSource.clip = select;
        audioSource.Play();
    }    
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
