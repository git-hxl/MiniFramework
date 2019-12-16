using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

public class ILRuntimeBehaviour : MonoBehaviour {
    public string ClassName;
	// Use this for initialization
	void Awake () {

        var type = ILRuntimeManager.Instance.appdomain.GetType(ClassName);
        //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
        var ilInstance = new ILTypeInstance(type as ILType, false);//手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                                                                   //接下来创建Adapter实例
        var clrInstance = gameObject.AddComponent<MonoBehaviourAdaptor.Adaptor>();
        //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
        clrInstance.ILInstance = ilInstance;
        clrInstance.AppDomain = ILRuntimeManager.Instance.appdomain;
        //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
        ilInstance.CLRInstance = clrInstance;

        clrInstance.Awake();//因为Unity调用这个方法时还没准备好所以这里补调一次

        if(clrInstance.enabled)
        {
            clrInstance.OnEnable();
        }
    }
}
