//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: Component
// Purpose: Output the frames per second to a TMP_Text component, this is used
//	for displaying the FPS counter value when selected in the options menu
// Applied to: The FPS counter UI element
//
//=============================================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class UI_Text_FPS : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private const float updateRate = 0.5f;
    private float time;
    private float frameCount;
    private bool active;
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private TMP_Text fpsCounterText;
    [SerializeField] private Image backgroundImage;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
	    fpsCounterText = GetComponent<TMP_Text>();
	    ShowFPSCounter(1);
    }

    private void Update()
    {
	    if (!active) return;
	    time += Time.deltaTime;
	    frameCount++;
	    if (!(time >= updateRate)) return;
	    var frameRate = Mathf.RoundToInt(frameCount / time);
	    fpsCounterText.text = frameRate + " FPS";
	    time -= updateRate;
	    frameCount = 0;
    }
    
    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    
    
    //=-----------------=
    // External Functions
    //=-----------------=
    public void ShowFPSCounter(int _value)
    {
	    if (_value == 0)
	    {
		    if (backgroundImage) backgroundImage.enabled = false;
		    active = false;
		    fpsCounterText.text = "";
	    }
	    else
	    {
		    if (backgroundImage) backgroundImage.enabled = true;
		    active = true;
	    }
    }
}

