using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.WebRequest;
public class TestWebRequest : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        WebRequestManager.Instance.Get("https://ss0.bdstatic.com/94oJfD_bAAcT8t7mm9GUKT-xh_/timg?image&quality=100&size=b4000_4000&sec=1586694521&di=3d0fe97e9f1427f7a88d2ebad250e200&src=http://a4.att.hudong.com/21/09/01200000026352136359091694357.jpg", (data) =>
        {

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(data.data);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            spriteRenderer.sprite = sprite;
        });

    }

}
