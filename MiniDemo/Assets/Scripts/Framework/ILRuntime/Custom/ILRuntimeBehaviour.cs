using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

public class ILRuntimeBehaviour : MonoBehaviour
{
    public string ClassName;
    // Use this for initialization
    void Awake()
    {
        IType type = ILRuntimeManager.Instance.appdomain.GetType(ClassName);
        ILTypeInstance ilInstance = new ILTypeInstance(type as ILType, false);
        MonoBehaviourAdaptor.Adaptor clrInstance = gameObject.GetComponent<MonoBehaviourAdaptor.Adaptor>();
        if (clrInstance == null)
            clrInstance = gameObject.AddComponent<MonoBehaviourAdaptor.Adaptor>();
        clrInstance.ILInstance = ilInstance;
        clrInstance.AppDomain = ILRuntimeManager.Instance.appdomain;
        ilInstance.CLRInstance = clrInstance;
        clrInstance.Awake();
        clrInstance.OnEnable();
    }

    
}
