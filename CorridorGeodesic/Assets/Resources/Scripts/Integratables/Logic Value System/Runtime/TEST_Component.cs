using Neverway.Framework.LogicValueSystem;
using System;
using UnityEngine;

public class TEST_Component : MonoBehaviour
{ 
    public LogicInput<bool> inputA = new(false);
    public LogicInput<bool> inputB = new(false);

    public LogicOutput<bool> output;

    [Space]
    public int TESTINGGGG = 0;  
    [Space]

    [SerializeReference][Polymorphic] public Filter someFilter;
    [Polymorphic] public Transform someField;
    [Polymorphic] public Transform someField2;
    public void Awake()
    {
        inputA.CallOnSourceChanged(OnInputUpdate);
        inputB.CallOnSourceChanged(OnInputUpdate);
    }

    public void OnInputUpdate()
    {
        output.Set(inputA.Get() && inputB.Get());
    }
}
[Serializable]
public class BaseClass
{
    public string someStringField;
}

[Serializable]
public class SomeClassA : BaseClass
{
    public int someFieldA;
}
[Serializable]
public class SomeClassA2 : SomeClassA
{
    public int someFieldA2;
}
[Serializable]
public class SomeClassB : BaseClass
{
    public AnimationCurve someFieldB;
}

[Serializable]
public abstract class Filter
{
    public abstract bool IncludeObjectInFilter(GameObject obj);
}
[Serializable]
public class TagFilter : Filter
{
    public string tag;
    public override bool IncludeObjectInFilter(GameObject obj)
    {
        return obj.tag == tag;
    }
}
[Serializable]
public class IsOnLayersFilter : Filter
{
    public LayerMask layers;
    public override bool IncludeObjectInFilter(GameObject obj)
    {
        return layers == (layers | (1 << obj.layer));
    }
}
[Serializable]
public class ChildrenCountFilter : Filter
{
    public int childrenMinimum;
    public int childrenMaximum;
    public override bool IncludeObjectInFilter(GameObject obj)
    {
        return obj.transform.childCount >= childrenMinimum && obj.transform.childCount <= childrenMaximum;
    }
}
[Serializable]
public class ThreeFilters : Filter
{
    //[SerializeReference, Polymorphic] public Filter[] filtersToPass;
    [SerializeReference][Polymorphic] public Filter filter1;
    [Space][SerializeReference][Polymorphic] public Filter filter2;
    [Space][SerializeReference][Polymorphic] public Filter filter3;

    public override bool IncludeObjectInFilter(GameObject obj)
    {
        bool passesAllFilters = true;
        //foreach(Filter filter in filtersToPass)
        //    if (filter != null)
        //        passesAllFilters &= filter.IncludeObjectInFilter(obj);

        if (filter1 != null) passesAllFilters &= filter1.IncludeObjectInFilter(obj);
        if (filter2 != null) passesAllFilters &= filter2.IncludeObjectInFilter(obj);
        if (filter3 != null) passesAllFilters &= filter3.IncludeObjectInFilter(obj);

        return passesAllFilters;
    }
}
[Serializable]
public class ArrayFilters : Filter
{
    [SerializeReference, Polymorphic] public Filter[] filtersToPass;

    public override bool IncludeObjectInFilter(GameObject obj)
    {
        bool passesAllFilters = true;
        foreach(Filter filter in filtersToPass)
            if (filter != null)
                passesAllFilters &= filter.IncludeObjectInFilter(obj);

        return passesAllFilters;
    }
}