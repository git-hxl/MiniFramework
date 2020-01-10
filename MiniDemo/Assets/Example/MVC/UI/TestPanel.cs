using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using UnityEngine.UI;

public class TestPanel : BasePanel {

    public Button bt_close;
    public Button bt_destroy;
    public override void Destroy()
    {
        Destroy(gameObject);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override BasePanel Show()
    {
        gameObject.SetActive(true);
        return this;
    }

    // Use this for initialization
    void Start () {
        bt_close.onClick.AddListener(() => { Hide(); });
        bt_destroy.onClick.AddListener(() => Destroy());

    }

}
