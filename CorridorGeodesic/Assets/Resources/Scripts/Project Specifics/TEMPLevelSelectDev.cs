using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMPLevelSelectDev : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //Invalidate speedrun timer if skipping levels
            try
            {
                Stopwatch timer = FindObjectOfType<Stopwatch>();
                if (!timer.timer.IsRunning)
                    timer.InvalidateTimer();
            }
            catch { }

            //Clear checkpoints
            Checkpoint.ClearCheckpoints();
            //Go to previous level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //Invalidate speedrun timer if skipping levels
            try
            {
                Stopwatch timer = FindObjectOfType<Stopwatch>();
                timer.InvalidateTimer();
            }
            catch { }

            //Clear checkpoints
            Checkpoint.ClearCheckpoints();
            //Go to next level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
}
