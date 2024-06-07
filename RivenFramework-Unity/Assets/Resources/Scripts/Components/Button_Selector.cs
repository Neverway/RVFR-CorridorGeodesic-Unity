//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Provide functionality to selector style buttons
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_Selector : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public List<string> selectorOptions;
    public int currentIndex;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Button left, right;
    [SerializeField] private GameObject indicator, indicatorSelected, indicatorRoot;
    [SerializeField] private TMP_Text text;
    [SerializeField] private List<GameObject> indicators;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        left.onClick.AddListener(delegate
        {
            print("Left");
            if (currentIndex - 1 >= 0)
            {
                currentIndex--;
                print(currentIndex);
            }
        });
        right.onClick.AddListener(delegate
        {
            print("Right");
            if (currentIndex + 1 <= selectorOptions.Count-1)
            {
                currentIndex++;
                print(currentIndex);
            }
        });
        // Create indicators
        for (int i = 0; i < selectorOptions.Count; i++)
        {
            var newIndicator = Instantiate(indicator, indicatorRoot.transform);
            newIndicator.SetActive(true);
            indicators.Add(newIndicator);
        }
    }

    private void Update()
    {
        // Updated selection
        for (int i = 0; i < indicators.Count; i++)
        {
            // Reset all the indicator colors
            indicators[i].GetComponent<Image>().color = indicator.GetComponent<Image>().color;
            // Set selected indicator (via colour, dummy) and text 
            if (i == currentIndex)
            {
                indicators[i].GetComponent<Image>().color = indicatorSelected.GetComponent<Image>().color;
                text.text = selectorOptions[i];
            }
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}