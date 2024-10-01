using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoCubesSoftlockCheck : LogicComponent
{
    public float timeForCubeToSlideIntoLava = 5f;


    [LogicComponentHandle] public LogicComponent firstRoomButton;
    [LogicComponentHandle] public LogicComponent isPlayerInExtensionOfSecondRoom;
    [LogicComponentHandle] public LogicComponent playerAtEndCheck;
    [LogicComponentHandle] public LogicComponent physPropAtEndCheck;

    public Prop_Respawner physCubeSpawner;
    public Prop_Respawner slipCubeSpawner;

    private Pawn player;
    private float timer;
    private Vector3 previousSlipCubePos;
    private bool waitAFrame = false;

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
        if (waitAFrame)
        {
            waitAFrame = false;
            return;
        }

        if (physCubeSpawner.spawnedObject != null && slipCubeSpawner.spawnedObject != null)
        {
            //If a cube is at the end but the player isnt, thats a problem >:(
            if (!playerAtEndCheck.isPowered && physPropAtEndCheck.isPowered)
            {
                if (physCubeSpawner.spawnedObject.transform.position.x < 
                    slipCubeSpawner.spawnedObject.transform.position.x)
                {
                    physCubeSpawner.DestroySpawnedObject();
                }
                else
                {
                    slipCubeSpawner.DestroySpawnedObject();
                }
                isPowered = true;
                waitAFrame = true;
                return;
            }

            //if player is in first room, things are finnnnee
            if (!IsInSecondRoom(player.transform) && !isPlayerInExtensionOfSecondRoom.isPowered)
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

            //That missing cube was the physCube, no need for timer
            if (IsInSecondRoom(slipCubeSpawner.spawnedObject.transform))
            {
                isPowered = true;
                return;
            }

            //Since the missing cube is a slipCube, give them some time first, wait until slip cube settles for a bit
            Vector3 slipCubePos = slipCubeSpawner.spawnedObject.transform.position;
            float slipCubeDeltaDistance = Vector3.Distance(previousSlipCubePos, slipCubePos);
            if (!isPowered && slipCubeDeltaDistance > 0.1f)
            {
                timer = timeForCubeToSlideIntoLava;
                previousSlipCubePos = slipCubePos;
            }
            timer -= Time.deltaTime;
            if (timer > 0) //Timer not finished yet, return for now come back later
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
