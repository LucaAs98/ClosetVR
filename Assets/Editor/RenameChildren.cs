using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenameChildren : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(500, 500);
    private string newString;
    private string removeString;
    private bool deepChildren;
    private string prefix;

    [MenuItem("GameObject/Rename children")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<RenameChildren>();
        window.minSize = size;
        window.maxSize = size;
    }

    private void OnGUI()
    {
        GUILayout.Space(15);
        deepChildren = EditorGUILayout.Toggle("Deep children", deepChildren);
        GUILayout.Space(30);
        InitRename();
        GUILayout.Space(50);
        InitAddPrefix();
    }

    private void InitRename()
    {
        newString = EditorGUILayout.TextField("New string", newString);
        removeString = EditorGUILayout.TextField("Remove string", removeString);

        GUILayout.Space(10);
        if (GUILayout.Button("Rename children"))
        {
            Transform objectParent = Selection.gameObjects[0].transform;

            RenameChildrenAndParent(objectParent);
        }
    }

    private void InitAddPrefix()
    {
        prefix = EditorGUILayout.TextField("Prefix", prefix);

        GUILayout.Space(10);

        if (GUILayout.Button("Add prefix"))
        {
            Transform objectParent = Selection.gameObjects[0].transform;
            AddPrefix(objectParent);
        }
    }


    private void RenameChildrenAndParent(Transform rootParent)
    {
        Transform[] allChildren = GetChildren(rootParent);

        foreach (Transform obj in allChildren)
        {
            if (removeString != "")
                obj.name = obj.name.Replace(removeString, newString);
        }
    }

    private void AddPrefix(Transform rootParent)
    {
        Transform[] allChildren = GetChildren(rootParent);
        foreach (Transform obj in allChildren)
        {
            obj.name = prefix + obj.name;
        }
    }

    private Transform[] GetChildren(Transform rootParent)
    {
        Transform[] allChildren;

        if (deepChildren)
        {
            allChildren = rootParent.GetComponentsInChildren<Transform>();
        }
        else
        {
            int children = rootParent.childCount;
            allChildren = new Transform[children];

            for (int i = 0; i < children; ++i)
                allChildren[i] = rootParent.GetChild(i).transform;
        }

        return allChildren;
    }
}