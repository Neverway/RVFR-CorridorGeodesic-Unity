using Neverway.Framework.LogicValueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DevTools
{
    private static MenuOption[] allMenuOptions;

    private static Type[] allTypes;
    private static FieldInfo[] staticFields;
    private static FieldInfo[] instanceFields;
    private static MethodInfo[] staticMethods;
    private static MethodInfo[] instanceMethods;
    private static EventInfo[] staticEvents;
    private static EventInfo[] instanceEvents;

    [RuntimeInitializeOnLoadMethod]
    public static void DomainReloadMenuOptions() => allMenuOptions = null;
    public static MenuOption[] GetAllMenuOptions()
    {
        if (allMenuOptions == null)
        {
            GetMemberInfos();
            List<MenuOption> menuOptions = new List<MenuOption>();
            MenuOption menuOption;
            foreach(FieldInfo field in staticFields)
            {
                DevMenuAttribute[] menuAttributes = field.GetCustomAttributes<DevMenuAttribute>().ToArray();
                foreach (DevMenuAttribute attribute in menuAttributes)
                    menuOptions.Add(attribute.GetMenuOption(field));
            }
            foreach (MethodInfo method in staticMethods)
            {
                DevMenuAttribute[] menuAttributes = method.GetCustomAttributes<DevMenuAttribute>().ToArray();
                foreach (DevMenuAttribute attribute in menuAttributes)
                    menuOptions.Add(attribute.GetMenuOption(method));
            }


        }
        return allMenuOptions;
    }
    private static void GetMemberInfos()
    {
        BindingFlags staticMembers = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        BindingFlags instanceMembers = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        Type[] allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes()).ToArray();

        staticFields = allTypes
                .SelectMany(type => type.GetFields(staticMembers)).ToArray();

        instanceFields = allTypes
                .SelectMany(type => type.GetFields(instanceMembers)).ToArray();

        staticMethods = allTypes
                .SelectMany(type => type.GetMethods(staticMembers)).ToArray();

        instanceMethods = allTypes
                .SelectMany(type => type.GetMethods(staticMembers)).ToArray();

        staticEvents = allTypes
                .SelectMany(type => type.GetEvents(staticMembers)).ToArray();

        instanceEvents = allTypes
                .SelectMany(type => type.GetEvents(staticMembers)).ToArray();
    }

    private static MenuOption GetMenuOptionFromStaticField(FieldInfo field)
    {
        return null;
    }
    private static MenuOption GetMenuOptionFromInstanceField(FieldInfo field)
    {

        return null;
    }
    private static MenuOption GetMenuOptionFromStaticField(MethodInfo field)
    {

        return null;
    }
    private static MenuOption GetMenuOptionFromInstanceField(MethodInfo field)
    {

        return null;
    }

    public abstract class MenuOption
    {

    }
    public class MenuButton : MenuOption
    {

    }
    public class MenuToggle : MenuOption
    {

    }
}

public abstract class DevMenuAttribute : Attribute
{
    public string name;
    public string tabGroup;
    public string category;
    public DevMenuAttribute(string name = "", string tabGroup = "", string category = "")
    {

    }

    public abstract DevTools.MenuOption GetMenuOption(MemberInfo memberInfo);
}
[AttributeUsage(AttributeTargets.Method)]
public class DevMenuButtonAttribute : DevMenuAttribute
{

    public override DevTools.MenuOption GetMenuOption(MemberInfo memberInfo)
    {
        if (memberInfo is MethodInfo methodInfo)
        {

        }
        return new DevTools.MenuButton();
    }
}
[AttributeUsage(AttributeTargets.Field)]
public class DevMenuToggleAttribute : DevMenuAttribute
{
    public override DevTools.MenuOption GetMenuOption(MemberInfo memberInfo)
    {
        return new DevTools.MenuToggle();
    }
}