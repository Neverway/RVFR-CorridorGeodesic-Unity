//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Neverway.Framework;

public class WB_FeedbackMenu: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private RectTransform root;
    [SerializeField] private Slider ratingSlider;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button saveBtn;

    private float timer;

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnEnable()
    {
        saveBtn.onClick.AddListener(()=>DevGAMMManager.Instance.SaveFeedback(
            (int)ratingSlider.value, inputField.text));
        saveBtn.onClick.AddListener(()=> GameInstance.Instance.UI_ShowFeedbackMenu(false));

        ratingSlider.onValueChanged.AddListener(delegate { ratingText.text = $"{ratingSlider.value}/5"; });

        
    }
    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(root);
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 140)
            saveBtn.onClick?.Invoke();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}