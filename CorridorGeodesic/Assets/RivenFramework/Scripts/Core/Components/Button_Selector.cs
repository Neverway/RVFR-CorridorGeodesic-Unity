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
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neverway.Framework
{
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
        public UnityEvent onValueChanged;
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
                if (currentIndex - 1 >= 0)
                {
                    currentIndex--;
                    print(currentIndex);
                    onValueChanged?.Invoke();
                }
            });
            right.onClick.AddListener(delegate
            {
                if (currentIndex + 1 <= selectorOptions.Count-1)
                {
                    currentIndex++;
                    print(currentIndex);
                    onValueChanged?.Invoke();
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

        public void Update()
        {
            // Updated selection
            // I really wish this could have been done inside an onClick event instead of update, but the iterator acts possessed otherwise >:L  ~Liz (Jun12-24)
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
}