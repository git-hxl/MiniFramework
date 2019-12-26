using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MiniFramework;
public class ILRuntimeManager : MonoSingleton<ILRuntimeManager> {

    public ILRuntime.Runtime.Enviorment.AppDomain appdomain;

    public bool IsLocal;
    public string dllPath;
    public void Start()
    {
        StartCoroutine(LoadILRuntime());
    }

    IEnumerator LoadILRuntime()
    {
        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        byte[] dll = null;
        byte[] pdb = null;
        if(IsLocal)
        {
            yield return FileUtil.ReadStreamingFile(Application.streamingAssetsPath + "/Hotfix.dll", (data) =>
            {
                dll = data;
            });
            yield return FileUtil.ReadStreamingFile(Application.streamingAssetsPath + "/Hotfix.pdb", (data) =>
            {
                pdb = data;
            });
        }
        else
        {
            dll = File.ReadAllBytes(dllPath);
        }
//#if UNITY_ANDROID
//    WWW www = new WWW(Application.streamingAssetsPath + "/Hotfix.dll");
//#else
//        WWW www = new WWW("file:///" + Application.streamingAssetsPath + "/Hotfix.dll.txt");
//#endif
//        while (!www.isDone)
//            yield return null;
//        if (!string.IsNullOrEmpty(www.error))
//            Debug.LogError(www.error);
//        byte[] dll = www.bytes;
//        www.Dispose();
//#if UNITY_ANDROID
//    www = new WWW(Application.streamingAssetsPath + "/Hotfix.pdb");
//#else
//        www = new WWW("file:///" + Application.streamingAssetsPath + "/Hotfix.pdb");
//#endif
//        while (!www.isDone)
//            yield return null;
//        if (!string.IsNullOrEmpty(www.error))
//            Debug.LogError(www.error);
//        byte[] pdb = www.bytes;
        using (System.IO.MemoryStream fs = new MemoryStream(dll))
        {
            if(pdb!=null)
            {
                using (System.IO.MemoryStream p = new MemoryStream(pdb))
                {
                    appdomain.LoadAssembly(fs, p, new Mono.Cecil.Pdb.PdbReaderProvider());
                }
            }
            else
            {
                appdomain.LoadAssembly(fs, null, new Mono.Cecil.Pdb.PdbReaderProvider());
            }
        }
        OnILRuntimeInitialized();
    }

    void OnILRuntimeInitialized()
    {
        SetAddCLRRedirection();
        SetGetCLRRedirection();

        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);

        appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
        appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
        appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());

        appdomain.RegisterCrossBindingAdaptor(new IMessageAdaptor());
        appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdaptor());
        appdomain.RegisterCrossBindingAdaptor(new CoroutineAdaptor());

        CustomDelegate.Register(appdomain);
        ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
    }
    unsafe void SetAddCLRRedirection()
    {
        var arr = typeof(GameObject).GetMethods();
        foreach (var i in arr)
        {
            if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
            {
                appdomain.RegisterCLRMethodRedirection(i, AddComponent);
            }
        }
    }
    unsafe void SetGetCLRRedirection()
    {
        var arr = typeof(GameObject).GetMethods();
        foreach (var i in arr)
        {
            if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
            {
                appdomain.RegisterCLRMethodRedirection(i, GetComponent);
            }
        }
    }
    //从Unity主工程获取热更DLL的MonoBehaviour
    public MonoBehaviourAdaptor.Adaptor GetComponent(ILType type)
    {
        var arr = GetComponents<MonoBehaviourAdaptor.Adaptor>();
        for (int i = 0; i < arr.Length; i++)
        {
            var instance = arr[i];
            if (instance.ILInstance != null && instance.ILInstance.Type == type)
            {
                return instance;
            }
        }
        return null;
    }

    unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //CLR重定向的说明请看相关文档和教程，这里不多做解释
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

        var ptr = __esp - 1;
        //成员方法的第一个参数为this
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
        if (instance == null)
            throw new System.NullReferenceException();
        __intp.Free(ptr);

        var genericArgument = __method.GenericArguments;
        //AddComponent应该有且只有1个泛型参数
        if (genericArgument != null && genericArgument.Length == 1)
        {
            var type = genericArgument[0];
            object res;
            if (type is CLRType)
            {
                //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                res = instance.AddComponent(type.TypeForCLR);
            }
            else
            {
                //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                var ilInstance = new ILTypeInstance(type as ILType, false);//手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                //接下来创建Adapter实例
                var clrInstance = instance.AddComponent<MonoBehaviourAdaptor.Adaptor>();
                //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                clrInstance.ILInstance = ilInstance;
                clrInstance.AppDomain = __domain;
                //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                ilInstance.CLRInstance = clrInstance;

                res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance

                clrInstance.Awake();//因为Unity调用这个方法时还没准备好所以这里补调一次
            }

            return ILIntepreter.PushObject(ptr, __mStack, res);
        }

        return __esp;
    }

    unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //CLR重定向的说明请看相关文档和教程，这里不多做解释
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

        var ptr = __esp - 1;
        //成员方法的第一个参数为this
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
        if (instance == null)
            throw new System.NullReferenceException();
        __intp.Free(ptr);

        var genericArgument = __method.GenericArguments;
        //AddComponent应该有且只有1个泛型参数
        if (genericArgument != null && genericArgument.Length == 1)
        {
            var type = genericArgument[0];
            object res = null;
            if (type is CLRType)
            {
                //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                res = instance.GetComponent(type.TypeForCLR);
            }
            else
            {
                //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                var clrInstances = instance.GetComponents<MonoBehaviourAdaptor.Adaptor>();
                for (int i = 0; i < clrInstances.Length; i++)
                {
                    var clrInstance = clrInstances[i];
                    if (clrInstance.ILInstance != null)//ILInstance为null, 表示是无效的MonoBehaviour，要略过
                    {
                        if (clrInstance.ILInstance.Type == type)
                        {
                            res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance
                            break;
                        }
                    }
                }
            }

            return ILIntepreter.PushObject(ptr, __mStack, res);
        }

        return __esp;
    }
}
