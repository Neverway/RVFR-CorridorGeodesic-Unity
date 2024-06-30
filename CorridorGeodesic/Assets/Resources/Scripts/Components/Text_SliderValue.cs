//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: Component
// Purpose: Output the value of a slider to a TMP_Text component
//
//=============================================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class UI_Text_SliderValue : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private float valueOffset;


    //=-----------------=
    // Private Variables
    //=-----------------=
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private TMP_Text tmpText;
    [SerializeField] private Slider slider;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
	    tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
	    if (!slider) return;
	    tmpText.text = (slider.value + valueOffset).ToString();
    }
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    
    
    //=-----------------=
    // External Functions
    //=-----------------=
}

