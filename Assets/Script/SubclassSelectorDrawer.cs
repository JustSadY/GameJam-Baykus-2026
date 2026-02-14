using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.LabelField(position, label.text, "Use [SubclassSelector] with [SerializeReference]");
            return;
        }

        Rect foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth,
            EditorGUIUtility.singleLineHeight);
        Rect buttonRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y,
            position.width - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

        string currentTypeName = property.managedReferenceFullTypename.Split(' ').LastOrDefault();
        if (string.IsNullOrEmpty(currentTypeName)) currentTypeName = "Null (Select Type)";

        if (GUI.Button(buttonRect, currentTypeName, EditorStyles.layerMaskField))
        {
            ShowTypeMenu(property);
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    private void ShowTypeMenu(SerializedProperty property)
    {
        Type baseType = GetFieldType(property);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Null"), string.IsNullOrEmpty(property.managedReferenceFullTypename), () =>
        {
            property.managedReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        });

        foreach (var type in types)
        {
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                property.managedReferenceValue = Activator.CreateInstance(type);
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }

    private Type GetFieldType(SerializedProperty property)
    {
        string[] parts = property.managedReferenceFieldTypename.Split(' ');
        return Type.GetType($"{parts[1]}, {parts[0]}");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
#endif
public class SubclassSelectorAttribute : PropertyAttribute
{
}