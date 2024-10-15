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
        [field: SerializeField] public ComponentFieldReference<LogicOutput<T>> source;
        public bool IsLinkedToOutput => !source.IsUndefined;
        public LogicInput(T defaultValue) : base(defaultValue) { }

        public override T Get() => IsLinkedToOutput ? source.Get().Get() : base.Get();
    }

    [Serializable]
    public class LogicOutput<T> : LogicValue<T>
    {
        public UnityEvent OnOutputChanged;
        public LogicOutput(T startingValue) : base(startingValue) { }
        public void Set(T newValue)
        {
            if (!value.Equals(newValue))
                OnOutputChanged.Invoke();

            value = newValue;
        }
    }
}