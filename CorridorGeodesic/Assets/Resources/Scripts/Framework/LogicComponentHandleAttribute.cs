using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class LogicComponentHandleAttribute : Attribute
{
    public float xOffset { get; }
    public float yOffset { get; }
    public float zOffset { get; }

    public LogicComponentHandleAttribute()
    {
        xOffset = 0f;
        yOffset = 0f;
        zOffset = 0f;
    }
    public LogicComponentHandleAttribute(float x, float y, float z)
    {
        xOffset = x;
        yOffset = y;
        zOffset = z;
    }

    public static LogicComponentHandleAttribute GetFromType(Type type)
    {
        IEnumerable<FieldInfo> handleFields = type.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttributes(typeof(LogicComponentHandleAttribute), true).Length > 0);

        foreach (FieldInfo field in handleFields)
        {
            LogicComponentHandleAttribute[] allLogicHandles =
                (LogicComponentHandleAttribute[])field.GetCustomAttributes(typeof(LogicComponentHandleAttribute), true);

            if (allLogicHandles.Length == 0) continue;

            return allLogicHandles[0];
        }
        return null;
    }
}
public struct LogicComponentHandleInfo
{
    public FieldInfo field;
    public LogicComponentHandleAttribute attribute;

    public LogicComponentHandleInfo(FieldInfo field, LogicComponentHandleAttribute attribute)
    {
        this.field = field;
        this.attribute = attribute;
    }

    public static LogicComponentHandleInfo[] GetFromType(Type type)
    {
        List<LogicComponentHandleInfo> allInfos = new List<LogicComponentHandleInfo>();

        FieldInfo[] handleFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in handleFields)
        {
            LogicComponentHandleAttribute[] allLogicHandles =
                (LogicComponentHandleAttribute[])field.GetCustomAttributes(typeof(LogicComponentHandleAttribute), true);

            if (allLogicHandles.Length == 0) continue;

            allInfos.Add(new LogicComponentHandleInfo(field, allLogicHandles[0]));
        }
        return allInfos.ToArray();
    }

    public void TryAssign<TValue>(LogicComponent obj, TValue value) 
    {
        if (field.FieldType.IsAssignableFrom(value.GetType()))
            field.SetValue(obj, value);

        else if (field.FieldType.IsAssignableFrom(typeof(List<TValue>)))
            ((List<TValue>)field.GetValue(obj)).Add(value);
    }
    public void TryClear(LogicComponent obj)
    {
        if (field.FieldType.IsAssignableFrom(typeof(List<LogicComponent>)))
            ((List<LogicComponent>)field.GetValue(obj)).Clear();

        else if (field.FieldType.IsNullable())
            field.SetValue(obj, null);
    }
}