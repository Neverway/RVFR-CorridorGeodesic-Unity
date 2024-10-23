using System;
using UnityEngine;
using UnityEngine.Events;

namespace Neverway.Framework.LogicValueSystem
{
    public interface LogicValue
    {
        public Transform GetHandleTarget();
        public abstract Type GetLogicValueType();
    }
    
    [Serializable]
    public abstract class LogicValue<T> : LogicValue
    {
        [SerializeField] protected Transform handleTarget;

        [SerializeField] protected T value;
        public virtual T Get() => value;

        public LogicValue(T defaultValue) 
        {
            value = defaultValue; 
        }

        public Transform GetHandleTarget() => handleTarget;
        public Type GetLogicValueType() => typeof(T);

        public static implicit operator T(LogicValue<T> logicValue) => logicValue.Get();
    }

    public interface LogicInput
    {
        public abstract bool HasLogicOutputSource { get; }
        public abstract LogicOutput GetSourceLogicOutput();
    }
    [Serializable]
    public class LogicInput<T> : LogicValue<T>, LogicInput
    {
        [SerializeField] public ComponentFieldReference<LogicOutput<T>> sourceField;

        public LogicInput(T defaultValue) : base(defaultValue) { }
        public override T Get() => HasLogicOutputSource ? value = sourceField.GetCached().Get() : base.Get();
        public void CallOnSourceChanged(UnityAction action) => sourceField.GetCached().OnOutputChanged.AddListener(action);

        public bool HasLogicOutputSource => !sourceField.IsUndefined;
        public LogicOutput GetSourceLogicOutput() => (HasLogicOutputSource ? sourceField.GetCached() as LogicOutput : null);
    }

    public interface LogicOutput : LogicValue
    {
        public UnityEvent GetOnOutputChanged();
    }
    [Serializable]
    public class LogicOutput<T> : LogicValue<T>, LogicOutput
    {
        [SerializeField] public UnityEvent OnOutputChanged;

        public LogicOutput(T startingValue) : base(startingValue) { }

        public void Set(T newValue)
        {
            if (!value.Equals(newValue))
            {
                value = newValue;
                OnOutputChanged?.Invoke();
            }
            else
                value = newValue;
        }
        public UnityEvent GetOnOutputChanged() => OnOutputChanged;
    }
}