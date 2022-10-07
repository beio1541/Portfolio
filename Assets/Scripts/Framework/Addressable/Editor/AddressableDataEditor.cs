#if PACKAGE_ADDRESSABLES_EDITOR
using UnityEditor;
using UnityEngine;

namespace Framework.Addressable.Data.Editor
{
    static public class AddressableDataEditor
    {
        static public void PropertyField(Rect rect, SerializedProperty property)
        {
            SerializedProperty keyProperty = property.FindPropertyRelative("key");
            keyProperty.stringValue = EditorGUI.TextField(rect, "Addressables Key", keyProperty.stringValue);
        }
        static public void PropertyField(SerializedProperty property)
        {
            SerializedProperty keyProperty = property.FindPropertyRelative("key");
            keyProperty.stringValue = EditorGUILayout.TextField("Addressables Key", keyProperty.stringValue);
        }
    }
}

#endif