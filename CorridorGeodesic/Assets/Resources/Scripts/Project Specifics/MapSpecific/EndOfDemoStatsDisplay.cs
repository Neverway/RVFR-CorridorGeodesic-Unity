using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndOfDemoStatsDisplay : MonoBehaviour
{
    public TextMeshPro statsDisplayText;
    public LogicComponent stopTracking;
    public int maxBuddies = 12;

    public void Update()
    {
        EndOfDemoStatsTracker stats = EndOfDemoStatsTracker.instance;
        if (stopTracking.isPowered)
        {
            stats.StopTracking();
        }

        statsDisplayText.text =
            "Buddies Fizzled: " + stats.buddies + " / " + maxBuddies + "\n" + 
            "Times Jumped: " + stats.jumps + "\n" +
            "Bulbs Placed: " + stats.bulbs;
    }
}
