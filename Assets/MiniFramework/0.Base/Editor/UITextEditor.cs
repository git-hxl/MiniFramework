using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace MiniFramework
{
    [CustomEditor(typeof(UITextComponent))]
    public class UITextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}