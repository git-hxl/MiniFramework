using UnityEngine;
using UnityEngine.UI;
using MiniFramework.UI;
public class UILogin : IPanel
{
    public InputField inputFieldAccount;
    public InputField inputFieldPassword;

    public Button buttonLogin;
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
        buttonLogin.onClick.AddListener(() =>
        {

        });
        buttonRegister.onClick.AddListener(() =>
        {
            UIManager.Instance.Open<UIRegister>("Assets/Example/01.UI/Prefabs/UIRegister.prefab");
        });
    }
}
