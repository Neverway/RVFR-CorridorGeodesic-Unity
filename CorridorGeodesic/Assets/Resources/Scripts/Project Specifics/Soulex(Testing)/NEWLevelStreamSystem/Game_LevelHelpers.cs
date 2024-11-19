//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neverway.Framework;

public class Game_LevelHelpers : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    //private Vector3 relativePosition;
    [IsDomainReloaded] private static Dictionary<int, LevelTransitioner> levelLoadHandlers = new Dictionary<int, LevelTransitioner>();

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=

    //=-----------------=
    // External Functions
    //=-----------------=
    [RuntimeInitializeOnLoadMethod]
    static void ReloadStaticFields()
    {
        levelLoadHandlers = new Dictionary<int, LevelTransitioner>();
    }
    public static LevelTransitioner StoreObjectRelativePosition(int id, Transform saveTransform)
    {
        if (levelLoadHandlers.ContainsKey(id))
            return levelLoadHandlers[id];

        LevelTransitioner levelTransitionInstance = new LevelTransitioner();
        levelLoadHandlers.Add(id, levelTransitionInstance);

        if (Game_LevelEndIdentifier.Instance)
            levelTransitionInstance.levelEndFound = true;

        levelTransitionInstance.SetRelativeEndPosition(saveTransform.position);
        levelTransitionInstance.SetRelativeEndRotation(saveTransform.rotation);

        return levelTransitionInstance;
    }
    public static RotationPositionBinding GetObjectWorldStartPosition(int id)
    {
        bool levelStartFound = false;

        LevelTransitioner levelTransitionInstance = null;

        levelLoadHandlers.TryGetValue(id, out levelTransitionInstance);

        RotationPositionBinding result = new RotationPositionBinding();

        result.position = Vector3.zero;
        result.rotation = Quaternion.identity;

        Game_LevelStartIdentifier levelStart = FindObjectOfType<Game_LevelStartIdentifier>();

        print(levelStart);

        if (levelStart)
            levelStartFound = true;

        if (levelTransitionInstance == null)
        {
            if (levelStartFound)
            {
                result.position = levelStart.startTransform.position;
                result.rotation = levelStart.startTransform.rotation;
            }
            else
                result.position = WorldSettings.GetPlayerStartPoint().position;
        }
        else if (levelStartFound)
        {
            result.position = levelStart.startTransform.TransformPoint(levelTransitionInstance.relativePosition);
            result.rotation = Quaternion.Euler(
                levelStart.startTransform.TransformDirection(levelTransitionInstance.relativeRotation.eulerAngles));
        }
        else
            result.position = WorldSettings.GetPlayerStartPoint().position;

        if (levelTransitionInstance != null)
            levelLoadHandlers.Remove(id);

        if(!levelStartFound)
            Debug.LogError("No Level Start");

        return result;
    }
    public static RotationPositionBinding GetObjectWorldStartPosition(LevelTransitioner levelTransitionInstance)
    {
        bool levelStartFound = false;

        RotationPositionBinding result = new RotationPositionBinding();

        result.position = Vector3.zero;
        result.rotation = Quaternion.identity;

        Game_LevelStartIdentifier levelStart = FindObjectOfType<Game_LevelStartIdentifier>();

        if (levelStart)
            levelStartFound = true;

        if (levelStartFound)
        {
            result.position = levelStart.startTransform.TransformPoint(levelTransitionInstance.relativePosition);
            result.rotation = Quaternion.Euler(
                levelStart.startTransform.TransformDirection(levelTransitionInstance.relativeRotation.eulerAngles));
        }
        else
            result.position = WorldSettings.GetPlayerStartPoint().position;

        return result;
    }
}
public class LevelTransitioner
{
    public bool levelEndFound;
    public Vector3 relativePosition;
    public Quaternion relativeRotation;

    public void SetRelativeEndPosition(Vector3 position)
    {
        if (levelEndFound)
            relativePosition = Game_LevelEndIdentifier.Instance.endTransform.InverseTransformPoint(position);
        else
        {
            Debug.LogError("No Level End");
            relativePosition = Vector3.zero;
        }
    }
    public void SetRelativeEndRotation(Quaternion rotation)
    {
        if (levelEndFound)
            relativeRotation = Quaternion.Euler(
                Game_LevelEndIdentifier.Instance.endTransform.InverseTransformDirection(rotation.eulerAngles));
        else
        {
            Debug.LogError("No Level End");
            relativeRotation = Quaternion.identity;
        }
    }
}
public struct RotationPositionBinding
{
    public Vector3 position;
    public Quaternion rotation;
}