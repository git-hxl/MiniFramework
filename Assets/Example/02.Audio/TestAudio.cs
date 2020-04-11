using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Audio;

public class TestAudio : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("Assets/Example/02.Audio/Audios/金玟岐 - 最初的梦想.mp3");
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnGUI()
    {
        if (GUILayout.Button("Click", GUILayout.Width(100), GUILayout.Height(100)))
        {
            AudioManager.Instance.PlaySound("Assets/Example/02.Audio/Audios/Click.wav", false);
        }
        if (GUILayout.Button("Click One Shot", GUILayout.Width(100), GUILayout.Height(100)))
        {
            AudioManager.Instance.PlaySound("Assets/Example/02.Audio/Audios/Click.wav", true);
        }
        if (GUILayout.Button("Click At Pos", GUILayout.Width(100), GUILayout.Height(100)))
        {
            AudioManager.Instance.PlaySoundAtPoint("Assets/Example/02.Audio/Audios/Click.wav", transform.position);
        }

        if (GUILayout.Button("Set TotalVolume x1"))
        {
            AudioManager.Instance.SetTotalVolume(1);
        }
        if (GUILayout.Button("Set TotalVolume x0.5"))
        {
            AudioManager.Instance.SetTotalVolume(0.5f);
        }
        if (GUILayout.Button("Set TotalVolume x0"))
        {
            AudioManager.Instance.SetTotalVolume(0);
        }
    }
}
