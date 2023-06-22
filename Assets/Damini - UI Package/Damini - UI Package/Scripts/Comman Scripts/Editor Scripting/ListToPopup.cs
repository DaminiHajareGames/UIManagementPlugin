using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StringInListAttrib : PropertyAttribute {

    public List<string> list;

    public Type type;
    public string method;

    public StringInListAttrib(Type type, string methodName)
    {
        var method = type.GetMethod(methodName);
        List<string> tempList = (List<string>) method.Invoke(null,null);
        list = tempList;
        this.type = type;
        this.method = methodName;
        Debug.Log("list = " + list);
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(StringInListAttrib))]
public class StringInListDrawer : PropertyDrawer
{
    enum StringEditingTypes { none, add, minus};
    StringEditingTypes currentStringEditingType= StringEditingTypes.none;
    string newName = "";
    int minusIndex = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var stringInList = attribute as StringInListAttrib;

        var method = stringInList.type.GetMethod(stringInList.method);
        var list = (List<string>) method.Invoke(null, null);

        if (list == null)
            return;

        if (property.propertyType == SerializedPropertyType.String) 
        {
            int index = Mathf.Max(0, Array.IndexOf(list.ToArray(), property.stringValue));
            index = EditorGUI.Popup(position, property.displayName, index, list.ToArray());
            property.stringValue = list[index];
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list.ToArray());
        }
        else
        {
            base.OnGUI(position, property, label);
        }

        //var stringInList = attribute as StringInList;

        //EditorGUI.BeginProperty(position, label, property);
        //GUILayout.Label (label);

        //if (stringInList.list.Count != 0)
        //{
        //    Debug.Log("Index = " + index);
        //    if (index >= stringInList.list.Count || index < 0)
        //    {
        //        index = 0;
        //        Debug.Log("Change index again = " + index);
        //    }

        //    index = EditorGUILayout.Popup(index, stringInList.list.ToArray());
        //    Debug.Log("after Index = " + index);

        //    property.stringValue = stringInList.list[index];
        //}

        //if (currentStringEditingType == StringEditingTypes.none)
        //{
        //    if (GUILayout.Button("+"))
        //        currentStringEditingType = StringEditingTypes.add;

        //    if (stringInList.list.Count != 0 && GUILayout.Button("-"))
        //        currentStringEditingType = StringEditingTypes.minus;
        //}
        //else
        //{
        //    if (currentStringEditingType == StringEditingTypes.add)
        //    {
        //        newName = GUILayout.TextField(newName);
        //        if (GUILayout.Button("Add"))
        //        {
        //            if (stringInList.list.Contains(newName))
        //            {
        //                Debug.Log("Same content");
        //                Debug.LogError("Error !!! - You are trying to add repeat content.");
        //                newName = "";
        //                return;
        //            }

        //            stringInList.list.Add(newName);
        //            index = stringInList.list.Count - 1;
        //            currentStringEditingType = StringEditingTypes.none;
        //        }
        //    }
        //    else if (currentStringEditingType == StringEditingTypes.minus)
        //    {
        //        minusIndex = EditorGUILayout.Popup(minusIndex, stringInList.list.ToArray());

        //        if (GUILayout.Button("Remove"))
        //        {
        //            Debug.Log(index + " Remove at = " + minusIndex);
        //            stringInList.list.RemoveAt(minusIndex);
        //            Debug.Log(index + " Remove at = " + minusIndex);

        //            if (minusIndex <= index)
        //            {
        //                index--;
        //                if (index < 0)
        //                {
        //                    index = 0;
        //                }
        //            }
        //            minusIndex = 0;
        //            currentStringEditingType = StringEditingTypes.none;
        //        }
        //    }
        //    EditorGUI.EndProperty();
        }
    }

    // Draw the property inside the given rect

    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //{
    //    //var stringInList = attribute as StringInList;

    //    //if (!string.IsNullOrEmpty(stringInList.listOfStringField))
    //    //{
    //    //    var listfStringProperty = property.FindPropertyRelative(stringInList.listOfStringField);
    //    //}

    //    //var list = stringInList.List;
    //    //if (property.propertyType == SerializedPropertyType.String)
    //    //{
    //    //    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
    //    //    index = EditorGUI.Popup(position, property.displayName, index, list);

    //    //    property.stringValue = list[index];
    //    //}
    //    //else if (property.propertyType == SerializedPropertyType.Integer)
    //    //{
    //    //    property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list);
    //    //}
    //    //else
    //    //{
    //    //    base.OnGUI(position, property, label);
    //    //}
    //}
#endif