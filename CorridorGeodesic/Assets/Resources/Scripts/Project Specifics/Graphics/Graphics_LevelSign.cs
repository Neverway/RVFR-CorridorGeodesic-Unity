//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Graphics_LevelSign : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private string section = "1";
    [SerializeField] private string floor = "B";
    [SerializeField] private string info = "";
    [SerializeField] private Hazard hazards;
    [SerializeField] private List<HazardSymbolPair> symbolPairs = new List<HazardSymbolPair>();

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private TextMeshPro screenText;
    [SerializeField] private SpriteRenderer[] signs = new SpriteRenderer[0];

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        SetHazardSymbols();
        SetText();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    void SetHazardSymbols()
    {
        Color color;
        for (int i = 1; i < symbolPairs.Count; i++) 
        {
            color = new Color(0.5f, 0.5f, 0.5f);
            Sprite sprite = symbolPairs[0].symbol;

            if (hazards.HasFlag(symbolPairs[i].hazard))
            {
                sprite = symbolPairs[i].symbol;
                color = Color.white;
            } 

            signs[i-1].sprite = sprite;
            signs[i-1].color = color;
        }
    }
    void SetText()
    {

        screenText.text = $"<b>Section -</b> {section}\r\n<size=2>Floor - {floor}\r\n-------------------------------\r\n{info}";
    }

    //=-----------------=
    // External Functions
    //=-----------------=
}
[System.Serializable]
public class HazardSymbolPair
{
    public Hazard hazard;
    public Sprite symbol;
}