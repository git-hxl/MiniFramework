using System.Collections;
using System.Collections.Generic;
using MiniFramework;
using UnityEngine;

public class UIExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("Open Panel1"))
        {
            UIManager.Instance.Open("Assets/MiniFramework 1/0.Example/UI/UI Panel1.prefab");
        }
        if (GUILayout.Button("Destroy Panel1"))
        {
            UIManager.Instance.Destroy("Assets/MiniFramework 1/0.Example/UI/UI Panel1.prefab");
        }

        if (GUILayout.Button("Open Panel2"))
        {
            UIManager.Instance.Open("Assets/MiniFramework 1/0.Example/UI/UI Panel2.prefab");
        }
        if (GUILayout.Button("Destroy Panel2"))
        {
            UIManager.Instance.Destroy("Assets/MiniFramework 1/0.Example/UI/UI Panel2.prefab");
        }

        if (GUILayout.Button("Open Panel3"))
        {
            UIManager.Instance.Open("Assets/MiniFramework 1/0.Example/UI/UI Panel3.prefab");
        }
        if (GUILayout.Button("Destroy Panel3"))
        {
            UIManager.Instance.Destroy("Assets/MiniFramework 1/0.Example/UI/UI Panel3.prefab");
        }

        if(GUILayout.Button("Clear"))
        {
            UIManager.Instance.Clear();
        }
    }
}
