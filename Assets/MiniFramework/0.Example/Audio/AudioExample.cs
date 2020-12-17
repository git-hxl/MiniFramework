using System.Collections;
using System.Collections.Generic;
using MiniFramework;
using UnityEngine;

public class AudioExample : MonoBehaviour
{
    float totalVolume = 1;
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
        if (GUILayout.Button("Shot"))
        {
            AudioManager.Instance.PlaySound("Assets/MiniFramework/0.Example/Audio/shot.mp3");
        }

        if (GUILayout.Button("Music"))
        {
            AudioManager.Instance.PlayMusic("Assets/MiniFramework/0.Example/Audio/bg.mp3");
        }

        if (GUILayout.Button("Pause Music"))
        {
            AudioManager.Instance.PauseMusic();
        }

        totalVolume = GUILayout.HorizontalSlider(totalVolume, 0, 1);
        if (AudioManager.Instance.GetTotalVolume != totalVolume)
            AudioManager.Instance.SetTotalVolume(totalVolume);
        

        if(GUILayout.Button("Clear"))
        {
            AudioManager.Instance.Clear();
        }
    }
}
