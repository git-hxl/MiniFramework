using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;

public class UILogin : UIPanel
{

    public override void Close()
    {
         gameObject.SetActive(false);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }


    public override void Refresh()
    {
        
    }
}
