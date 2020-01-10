using UnityEditor;
using UnityEngine;

public class HelperTool {

    [MenuItem("Assets/获取AssetPath")]
    static void GetResAssetPath()
    {
        if (Selection.assetGUIDs == null) return;
        string assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        TextEditor textEditor = new TextEditor();
        textEditor.text = assetPath;
        textEditor.OnFocus();
        textEditor.Copy();
    }
}
