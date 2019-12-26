using ILRuntime.Runtime.Enviorment;
class CustomDelegate
{
    public static void Register(AppDomain appdomain)
    {
        RegisterMethodDelegate(appdomain);
        RegisterFunctionDelegate(appdomain);
        RegisterDelegateConvertor(appdomain);
    }
    /// <summary>
    /// 注册Action类型
    /// </summary>
    /// <param name="appdomain"></param>
    static void RegisterMethodDelegate(AppDomain appdomain)
    {
        appdomain.DelegateManager.RegisterMethodDelegate<int>();
        appdomain.DelegateManager.RegisterMethodDelegate<float>();
        appdomain.DelegateManager.RegisterMethodDelegate<string>();
    }
    /// <summary>
    /// 注册Func类型
    /// </summary>
    /// <param name="appdomain"></param>
    static void RegisterFunctionDelegate(AppDomain appdomain)
    {
        //带返回值的委托的话需要用RegisterFunctionDelegate，返回类型为最后一个
        appdomain.DelegateManager.RegisterFunctionDelegate<IMessageAdaptor.Adaptor>();
    }
    /// <summary>
    /// 委托转换器
    /// </summary>
    /// <param name="appdomain"></param>
    static void RegisterDelegateConvertor(AppDomain appdomain)
    {
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((action) =>
        {
            return new UnityEngine.Events.UnityAction(() =>
            {
                ((System.Action)action)();
            });
        });

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<float>>((action) =>
        {
            return new UnityEngine.Events.UnityAction<float>((a) =>
            {
                ((System.Action<float>)action)(a);
            });
        });
    }
}
