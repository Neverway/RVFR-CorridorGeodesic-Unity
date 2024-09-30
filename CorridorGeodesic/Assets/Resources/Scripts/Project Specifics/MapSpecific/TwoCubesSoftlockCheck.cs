using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoCubesSoftlockCheck : LogicComponent
{
    [LogicComponentHandle] public LogicComponent firstRoomButton;

    public float timeForCubeToSlideIntoLava = 5f;

    public Prop_Respawner physCubeSpawner;
    public Prop_Respawner slipCubeSpawner;

    private Pawn player;
    private float timer;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        player = FindObjectOfType<Pawn>();
    }

    // Update is called once per frame
    void Update()
    {
        bool wasPowered = isPowered;
        CheckForSoftlock();
        if (wasPowered && !isPowered && !IsInSecondRoom(player.transform)) 
        {
            physCubeSpawner.DestroySpawnedObject();
        }
    }

    public void CheckForSoftlock()
    {
        if (physCubeSpawner.spawnedObject != null && slipCubeSpawner.spawnedObject != null)
        {
            //if player is in first room, things are finnnnee
            if (!IsInSecondRoom(player.transform))
            {
                //unless both cubes are in the other room, then things arent so finnnnee
                if (IsInSecondRoom(physCubeSpawner.spawnedObject.transform)
                && IsInSecondRoom(slipCubeSpawner.spawnedObject.transform))
                {
                    isPowered = true;
                    physCubeSpawner.DestroySpawnedObject();
                    return;
                }

                isPowered = false;
                timer = timeForCubeToSlideIntoLava;
                return;
            }
            //if button in first room is powered, things are finnnnnee
            if (firstRoomButton.isPowered)
            {
                isPowered = false;
                timer = timeForCubeToSlideIntoLava;
                return;
            }
            //if both cubes are in 2nd room with player, things are finnnnnee
            if (IsInSecondRoom(physCubeSpawner.spawnedObject.transform)
                && IsInSecondRoom(slipCubeSpawner.spawnedObject.transform))
            {
                isPowered = false;
                timer = timeForCubeToSlideIntoLava;
                return;
            }
            //Missing a cube, player in 2nd room, and door is not open
            //Is player waiting for cube to slide to lava or are they softlocked?

            //Lets give them some time first
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                isPowered = false;
                return;
            }

            //Okay, its been enough time, player is probably softlocked
            isPowered = true;
            return;
        }
    }

    public bool IsInSecondRoom(Transform obj)
    {
        return obj.position.x < transform.position.x;
    }
}
