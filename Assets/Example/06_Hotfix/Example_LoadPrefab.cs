using UnityEngine;
using MiniFramework;
using UnityEngine.SceneManagement;

public class Example_LoadPrefab : MonoBehaviour
{
    public string TargetScene;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 400, 200, 200), "生成Cube"))
        {
            Instantiate(ResManager.Instance.Load("Prefab/Cube"));
        }
        if (GUI.Button(new Rect(0, 600, 200, 200), TargetScene))
        {
            SceneManager.LoadScene(TargetScene);
        }
    }
}
