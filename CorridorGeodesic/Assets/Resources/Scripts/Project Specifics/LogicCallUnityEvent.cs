using UnityEngine.Events;

public class LogicCallUnityEvent : LogicComponent
{
    public EventCallType eventCallType = EventCallType.OnSourceChangedWhenPowered;
    [LogicComponentHandle] public LogicComponent input;
    public UnityEvent unityEvent = new UnityEvent(); 

    private void Update()
    {
        if (eventCallType == EventCallType.EveryFrameWhenPowered)
            if (input != null && input.isPowered)
                unityEvent?.Invoke();
    }

    public override void SourcePowerStateChanged(bool powered)
    {
        if (input == null)
            return;

        isPowered = powered;

        if (eventCallType == EventCallType.OnSourceChangedWhenPowered)
        {
            if (input.isPowered)
                unityEvent?.Invoke();
        }    
        else if (eventCallType == EventCallType.OnSourceChanged)
            unityEvent?.Invoke();
    }
}
