using System;

public class IsDomainReloadedAttribute : Attribute
{

}
public class DomainReloadAttribute : IsDomainReloadedAttribute
{
    public object defaultValue;

    public DomainReloadAttribute(object defaultValue)
    {
        this.defaultValue = defaultValue;
    }
}