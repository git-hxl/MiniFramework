using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniFramework.UI;
public class UIRegister : IPanel
{
    public InputField inputFieldAccout;
    public InputField inputFieldPassword;
    public InputField inputFieldPasswordAgain;

    public Button buttonRegister;
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
        throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        buttonRegister.onClick.AddListener(() =>
        {
            Close();
        });
    }
}
