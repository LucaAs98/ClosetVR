using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenameChildren : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(250, 100);
    private string newString;
    private string removeString;


    [MenuItem("GameObject/Rename children")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<RenameChildren>();
        window.minSize = size;
        window.maxSize = size;
    }

    private void OnGUI()
    {
        newString = EditorGUILayout.TextField("New string", newString);
        removeString = EditorGUILayout.TextField("Remove string", removeString);
        if (GUILayout.Button("Rename children"))
        {
            Transform objectParent = Selection.gameObjects[0].transform;

            RenameChildrenAndParent(objectParent);
        }
    }

    private void RenameChildrenAndParent(Transform rootParent)
    {
        Transform[] allChildren = rootParent.GetComponentsInChildren<Transform>();
        foreach (Transform obj in allChildren)
        {
            obj.name = obj.name.Replace(removeString, newString);
        }
    }
}