using System.Collections;
using UnityEngine;
using MiniFramework;
using UnityEngine.UI;

public class DownloadFile : MonoBehaviour
{
    public Image Image;
    HttpDownload httpDownload;
    // Use this for initialization
    IEnumerator Start()
    {
        HttpRequest.Get(this,"https://b-ssl.duitang.com/uploads/item/201805/13/20180513224039_tgfwu.png",callBack);
        httpDownload = new HttpDownload(Application.streamingAssetsPath);
        yield return httpDownload.Download("http://b-ssl.duitang.com/uploads/item/201710/12/20171012051803_FHzS3.png");
    }

    // Update is called once per frame
    void Update()
    {
        if (httpDownload.Progress < 1)
            Debug.Log(httpDownload.Progress);
    }
    void callBack(Texture2D texture)
    {
        Image.sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
    }
}
