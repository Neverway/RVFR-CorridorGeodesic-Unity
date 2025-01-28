using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug_hideUI : MonoBehaviour
{
    public bool hideUI = false;
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ToggleHud();
        }
    }

    private void ToggleHud()
    {
        hideUI = !hideUI;
        target.SetActive(hideUI);
    }
}
