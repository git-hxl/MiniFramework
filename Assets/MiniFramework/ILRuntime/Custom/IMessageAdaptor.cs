using System.Collections.Generic;
using System;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using ILRuntime.CLR.TypeSystem;

public class IMessageAdaptor: CrossBindingAdaptor{

    public override Type BaseCLRType
    {
        get
        {
            return typeof(IMessage<ILTypeInstance>);
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(Adaptor);
        }
    }
    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor: IMessage<ILTypeInstance>, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        object[] param = new object[1];
        public Adaptor()
        {

        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        IMethod mDescriptorMethod;
        bool mDescriptorMethodGot;
        public MessageDescriptor Descriptor
        {
            get
            {
                if(!mDescriptorMethodGot)
                {
                    mDescriptorMethod = instance.Type.GetMethod("Descriptor", 0);
                    mDescriptorMethodGot = true;
                }
                if(mDescriptorMethod!=null)
                {
                    var res = appdomain.Invoke(mDescriptorMethod, instance, null);
                    return res as MessageDescriptor;
                }
                else
                {
                    return null;
                }
            }
        }

        IMethod mCalculateSize;
        bool mCalculateSizeGot;
        public int CalculateSize()
        {
            if (!mCalculateSizeGot)
            {
                mCalculateSize = instance.Type.GetMethod("CalculateSize", 0);
                mCalculateSizeGot = true;
            }
            if (mCalculateSize != null)
            {
                var res = appdomain.Invoke(mCalculateSize, instance, null);
                return (int)res;
            }
            else
            {
                return 0;
            }
        }

        IMethod mClone;
        bool mCloneGot;
        public ILTypeInstance Clone()
        {
            if (!mCloneGot)
            {
                mClone = instance.Type.GetMethod("Clone", 0);
                mCloneGot = true;
            }
            if (mClone != null)
            {
                var res = appdomain.Invoke(mClone, instance, null);
                return (ILTypeInstance)res;
            }
            else
            {
                return null;
            }
        }

        IMethod mEquals;
        bool mEqualsGot;
        public bool Equals(ILTypeInstance other)
        {
            if (!mEqualsGot)
            {
                mEquals = instance.Type.GetMethod("Equals", 1);
                mEqualsGot = true;
            }
            if (mEquals != null)
            {
                param[0] = other;
                var res = appdomain.Invoke(mEquals, instance, param);
                return (bool)res;
            }
            else
            {
                return false;
            }
        }

        IMethod mMergeFrom1;
        bool mMergeFrom1Got;
        public void MergeFrom(ILTypeInstance message)
        {
            if (!mMergeFrom1Got)
            {
                //指定参数类型来获得IMethod
                IType intType = appdomain.GetType(typeof(ILTypeInstance));
                //参数类型列表
                List<IType> paramList = new List<ILRuntime.CLR.TypeSystem.IType>();
                paramList.Add(intType);

                mMergeFrom1 = instance.Type.GetMethod("MergeFrom", paramList,null);
                mMergeFrom1Got = true;
            }
            if (mMergeFrom1 != null)
            {
                param[0] = message;
                appdomain.Invoke(mMergeFrom1, instance, param);
            }
        }

        IMethod mMergeFrom2;
        bool mMergeFrom2Got;
        public void MergeFrom(CodedInputStream input)
        {
            if (!mMergeFrom2Got)
            {
                //指定参数类型来获得IMethod
                IType intType = appdomain.GetType(typeof(CodedInputStream));
                //参数类型列表
                List<IType> paramList = new List<ILRuntime.CLR.TypeSystem.IType>();
                paramList.Add(intType);

                mMergeFrom2 = instance.Type.GetMethod("MergeFrom", paramList, null);
                mMergeFrom2Got = true;
            }
            if (mMergeFrom2 != null)
            {
                param[0] = input;
                appdomain.Invoke(mMergeFrom2, instance, param);
            }
        }

        IMethod mWriteTo;
        bool mWriteToGot;
        public void WriteTo(CodedOutputStream output)
        {
            if (!mWriteToGot)
            {
                mWriteTo = instance.Type.GetMethod("WriteTo", 1);
                mWriteToGot = true;
            }
            if (mWriteTo != null)
            {
                param[0] = output;
                appdomain.Invoke(mWriteTo, instance, param);
            }
        }
    }
}