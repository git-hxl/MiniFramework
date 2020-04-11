using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Resource;
public class ResLoadExample : MonoBehaviour
{
    public GameObject Cube;
    public AudioClip clip;
    public Texture texture;
    public Sprite sprite;
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
        if (GUILayout.Button("加载Cube", GUILayout.Width(200), GUILayout.Height(150)))
        {
            Cube = ResourceManager.Instance.LoadAsset<GameObject>("Cube");
            Instantiate(Cube);
        }
        if (GUILayout.Button("加载音效", GUILayout.Width(200), GUILayout.Height(150)))
        {
            clip = ResourceManager.Instance.LoadAsset<AudioClip>("Click");
        }
        if (GUILayout.Button("加载纹理", GUILayout.Width(200), GUILayout.Height(150)))
        {
            texture = ResourceManager.Instance.LoadAsset<Texture>("星星_2D");
        }
        if (GUILayout.Button("加载精灵", GUILayout.Width(200), GUILayout.Height(150)))
        {
            sprite = ResourceManager.Instance.LoadAsset<Sprite>("星星");
        }

        if (GUILayout.Button("释放资源", GUILayout.Width(200), GUILayout.Height(150)))
        {
            Cube =null;
            clip = null;
            texture = null;
            sprite = null;
            Resources.UnloadUnusedAssets();
        }
    }
}
