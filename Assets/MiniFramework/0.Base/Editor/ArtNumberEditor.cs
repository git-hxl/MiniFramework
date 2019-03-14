
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using MiniFramework;
namespace MiniFramework
{
    [CustomEditor(typeof(ArtNumber))]
    public class ArtNumberEditor : Editor
    {
        ArtNumber artNumber;
        private void OnEnable()
        {
            artNumber = (ArtNumber)target;
            artNumber.Generate();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            artNumber.Number = serializedObject.FindProperty("number").stringValue;
            artNumber.transform.localScale = new Vector3(1, 1, 1) * serializedObject.FindProperty("size").floatValue;
        }
    }
}