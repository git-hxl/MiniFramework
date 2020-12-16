using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using UnityEngine.UI;

public class UITestPanel : UIView
{
    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Button bt = transform.GetComponentInChildren<Button>();
        bt.onClick.AddListener(Close);
    }
}
