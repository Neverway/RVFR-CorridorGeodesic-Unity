using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicValueChangedHandler : MonoBehaviour
{
    [IsDomainReloaded] private static LogicValueChangedHandler Instance;
    private List<object> toProcess = new List<object>();

    private void LateUpdate()
    {
        if (toProcess.Count == 0)
            return;

        List<object> toRemove = new List<object>();

        foreach(object processing in toProcess)
        {
            if (processing != null)
            {
                object fieldValue = processing.GetType().GetField("attachedTo").GetValue(processing);
                if (fieldValue is Component component && component != null)
                {
                    Debug.Log(component.name);
                    toRemove.Add(processing);
                }
            }
        }
        foreach (LogicInput<object> removing in toRemove)
            toProcess.Remove(removing);
    }

    public static void AutoSubscribeLogicInput(object input)
    {
        Instance.toProcess.Add(input);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeLoad()
    {
        GameObject instance = new GameObject();
        Instance = instance.AddComponent<LogicValueChangedHandler>();
        DontDestroyOnLoad(instance);
    }
}