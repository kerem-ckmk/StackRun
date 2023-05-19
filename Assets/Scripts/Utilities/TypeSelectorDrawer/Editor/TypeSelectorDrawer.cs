using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TypeSelectorAttribute))]
public class TypeSelectorDrawer : PropertyDrawer
{
    private const string FieldNameSuffix = " Type Name";

    private bool _isInitialized = false;
    private List<Type> _inheritedClassTypes;
    private int _selectedIndex = -1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!_isInitialized)
        {
            _isInitialized = true;

            var typeSelectorAttribute = (TypeSelectorAttribute)attribute;
            _inheritedClassTypes = ReflectionUtilities.GetInheritedClassTypes(typeSelectorAttribute.baseType);
        }

        _selectedIndex = -1;
        string[] options = new string[_inheritedClassTypes.Count];
        for (int i = 0; i < options.Length; i++)
        {
            options[i] = ObjectNames.NicifyVariableName(_inheritedClassTypes[i].Name);

            if (property.stringValue == _inheritedClassTypes[i].FullName)
                _selectedIndex = i;
        }

        int index = property.displayName.IndexOf(FieldNameSuffix);
        string cleanName = (index < 0) ? property.displayName : property.displayName.Remove(index, FieldNameSuffix.Length);
        int newSelectedIndex = EditorGUI.Popup(position, cleanName, _selectedIndex, options);

        if (newSelectedIndex != _selectedIndex)
        {
            _selectedIndex = newSelectedIndex;
            property.stringValue = _inheritedClassTypes[_selectedIndex].FullName;
        }
    }
}