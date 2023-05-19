using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectionUtilities
{
    public static List<Type> GetInheritedClassTypes(params Type[] baseTypes)
    {
        var inheritedClassTypes = new List<Type>();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] assemblyTypes;
            try
            {
                assemblyTypes = assembly.GetTypes();
            }
            catch
            {
                continue;
            }

            foreach (var type in assemblyTypes)
            {
                foreach (var baseType in baseTypes)
                {
                    if (type.IsSubclassOf(baseType))
                        inheritedClassTypes.Add(type);
                }
            }
        }

        return inheritedClassTypes;
    }

    public static T CreateInstanceFromTypeName<T>(string typeFullName, params object[] constructorParameters) where T : class
    {
        Type type = null;
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(typeFullName);

            if (type != null)
                break;
        }

        T instance = type != null ? (T)Activator.CreateInstance(type, constructorParameters) : null;

        return instance;
    }
}
