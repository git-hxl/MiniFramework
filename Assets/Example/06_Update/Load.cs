using UnityEngine;
using MiniFramework;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    public Texture2D Image;
    public AudioClip Audio;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 200, 200), "Load"))
        {
            Image = ResManager.Instance.Load("image") as Texture2D;
            Audio = ResManager.Instance.Load("audio") as AudioClip;
            ResManager.Instance.LoadScene("scene",LoadSceneMode.Additive);

            AudioManager.Instance.PlaySound(Audio,true);
        }
    }
}
