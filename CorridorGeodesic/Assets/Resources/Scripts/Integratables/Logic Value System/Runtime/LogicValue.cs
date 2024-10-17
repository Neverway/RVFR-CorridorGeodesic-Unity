using System;
using UnityEngine;
using UnityEngine.Events;

namespace Neverway.Framework.LogicValueSystem
{
    [Serializable]
    public abstract class LogicValue<T>
    {
        [SerializeField] protected T value;

        public LogicValue(T defaultValue) { value = defaultValue; }
        public virtual T Get() => value;

        public static implicit operator T(LogicValue<T> logicValue) => logicValue.Get();
    }

    [Serializable]
    public class LogicInput<T> : LogicValue<T>
    {
        [SerializeField] public ComponentFieldReference<LogicOutput<T>> sourceField;
        public bool IsLinkedToOutput => !sourceField.IsUndefined;
        public LogicInput(T defaultValue) : base(defaultValue) { }
        public override T Get() => IsLinkedToOutput ? sourceField.GetCached().Get() : base.Get();
        public void CallOnSourceChanged(UnityAction action) => sourceField.GetCached().OnOutputChanged.AddListener(action);
    }

    [Serializable]
    public class LogicOutput<T> : LogicValue<T>
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

    }
}