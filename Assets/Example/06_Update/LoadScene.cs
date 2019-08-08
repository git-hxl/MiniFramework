using UnityEngine;
using MiniFramework;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string TargetScene;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(900, 500, 200, 200), "加载场景"))
        {
            SceneManager.LoadScene(TargetScene);
        }
    }
}
