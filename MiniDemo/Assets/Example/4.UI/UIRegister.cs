using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class UIRegister : UIPanel {
    public override void Close()
    {
        Debug.Log("close");
    }

    public override void Open()
    {
       Debug.Log("Open");
    }

    public override void Refresh()
    {
       Debug.Log("Refresh");
    }
}
