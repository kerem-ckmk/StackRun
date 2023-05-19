using System;
using UnityEngine;

public class TypeSelectorAttribute : PropertyAttribute
{
    public Type baseType;

    public TypeSelectorAttribute(Type baseType)
    {
        this.baseType = baseType;
    }
}