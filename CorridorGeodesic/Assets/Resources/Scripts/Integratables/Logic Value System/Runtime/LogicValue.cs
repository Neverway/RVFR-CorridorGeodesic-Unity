using System;
using UnityEngine;
using UnityEngine.Events;

namespace Neverway.Framework.LogicValueSystem
{
    public interface LogicValue
    {
        public bool EditorHandles_GetShowHandle();
        public Transform EditorHandles_GetHandle();
        public string EditorHandles_GetCustomName();
        public abstract Type GetLogicValueType();

        public abstract LogicValue GetClone();
    }
    
    [Serializable]
    public abstract class LogicValue<T> : LogicValue
    {
        [SerializeField] protected bool editorHandles_showHandle = true;
        [SerializeField] protected Transform editorHandles_handleTarget;
        [SerializeField] protected string editorHandles_customName = "";

        [SerializeField] protected T value;
        public virtual T Get() => value;

        public LogicValue(T defaultValue) 
        {
            value = defaultValue; 
        }
        public bool EditorHandles_GetShowHandle() => editorHandles_showHandle;
        public Transform EditorHandles_GetHandle() => editorHandles_handleTarget;
        public string EditorHandles_GetCustomName() => editorHandles_customName;
        public Type GetLogicValueType() => typeof(T);

        public LogicValue GetClone()
        {
            if (this is LogicInput<T> input)
            {
                LogicInput<T> toReturn = new LogicInput<T>(value);
                toReturn.sourceField = input.sourceField;
                toReturn.editorHandles_handleTarget = input.editorHandles_handleTarget;
                toReturn.editorHandles_customName = input.editorHandles_customName;

                return toReturn;
            }
            if (this is LogicOutput<T> output)
            {
                LogicOutput<T> toReturn = new LogicOutput<T>(value);
                toReturn.OnOutputChanged = output.OnOutputChanged;
                toReturn.editorHandles_handleTarget = output.editorHandles_handleTarget;
                toReturn.editorHandles_customName = output.editorHandles_customName;

                return toReturn;
            }
            throw new Exception("Cannot Clone: LogicValue was neither a LogicInput nor a LogicOutput");
        }

        public static implicit operator T(LogicValue<T> logicValue) => logicValue.Get();
    }

    public interface LogicInput : LogicValue
    {
        public abstract bool HasLogicOutputSource { get; }
        public abstract LogicOutput GetSourceLogicOutput();
        public abstract Component GetSourceLogicOutputComponent();
        public abstract object SetFieldReference(Component component, string fieldName);
    }
    [Serializable]
    public class LogicInput<T> : LogicValue<T>, LogicInput
    {
        [SerializeField] public ComponentFieldReference<LogicOutput<T>> sourceField;

        public LogicInput(T defaultValue) : base(defaultValue) { }
        public override T Get() => HasLogicOutputSource ? value = sourceField.Get() : base.Get();
        public void CallOnSourceChanged(UnityAction action) => sourceField.Get().OnOutputChanged.AddListener(action);

        public bool HasLogicOutputSource => !sourceField.IsUndefined;
        public LogicOutput GetSourceLogicOutput() => HasLogicOutputSource ? sourceField.Get() as LogicOutput : null;
        public Component GetSourceLogicOutputComponent() => HasLogicOutputSource ? sourceField.GetTargetComponent() : null;
        public object SetFieldReference(Component component, string fieldName)
        {
            if (component == null)
                fieldName = "";

            if (!string.IsNullOrEmpty(fieldName) && component.GetType().GetField(fieldName) == null)
                throw new ArgumentException($"{component.name} does not contain field named \"{fieldName}\"");

            sourceField = new ComponentFieldReference<LogicOutput<T>>(component, fieldName);
            return sourceField;
        }
    }
    /*
    public interface LogicInputList 
    {
        public abstract int SourceOutputCount { get; }
        public abstract LogicOutput[] GetSourceLogicOutputs();
        public abstract Component GetSourceLogicOutputComponent();
        public abstract object SetFieldReference(Component component, string fieldName);
    }
    [Serializable]
    public class LogicInputList<T> : LogicValue<T[]>, LogicInputList
    {
        [SerializeField] public ComponentFieldReference<LogicOutput<T>>[] sourceFields;

        public LogicInputList(T[] defaultValue) : base(defaultValue) { }
        public override T Get() => HasLogicOutputSource ? value = sourceFields.Get() : base.Get();
        public void CallOnSourceChanged(UnityAction action) => sourceFields.Get().OnOutputChanged.AddListener(action);

        public int SourceOutputCount => !sourceFields.IsUndefined;
        public LogicOutput[] GetSourceLogicOutputs()
        {

        }
        public Component GetSourceLogicOutputComponent() => HasLogicOutputSource ? sourceFields.GetTargetComponent() : null;
        public object SetFieldReference(Component component, string fieldName)
        {
            if (component == null)
                fieldName = "";

            if (!string.IsNullOrEmpty(fieldName) && component.GetType().GetField(fieldName) == null)
                throw new ArgumentException($"{component.name} does not contain field named \"{fieldName}\"");

            sourceFields = new ComponentFieldReference<LogicOutput<T>>(component, fieldName);
            return sourceFields;
        }
    }
    // */

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