using System;
using System.IO;
using System.Runtime.InteropServices;
using LitJson;
namespace MiniFramework
{
    public static class SerializeUtil
    {
        public static string ToJson(object obj)
        {
            return JsonMapper.ToJson(obj);
        }
        public static T FromJson<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }
        public static JsonData FromJson(string json)
        {
            return JsonMapper.ToObject(json);
        }

        public static byte[] ToProtoBuff(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromProtoBuff<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                T t = ProtoBuf.Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        public static byte[] ToPtr(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] data = new byte[size];
            IntPtr bufferIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, bufferIntPtr, true);
                Marshal.Copy(bufferIntPtr, data, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferIntPtr);
            }
            return data;
        }
        public static T FromPtr<T>(byte[] data)
        {
            object obj;
            int size = Marshal.SizeOf(typeof(T));
            IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(data, 0, allocIntPtr, size);
                obj = Marshal.PtrToStructure(allocIntPtr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(allocIntPtr);
            }
            return (T)obj;
        }
    }
}