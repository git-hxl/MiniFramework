using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
namespace MiniFramework
{
    public static class SerializeUtil
    {
        public static bool ToBinary(string path, object obj)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                return true;
            }
        }

        public static object FromBinary(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object data = bf.Deserialize(fs);

                if (data != null)
                {
                    return data;
                }
            }
            return null;
        }

        public static bool ToXML(string path, object obj)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                XmlSerializer xmlserializer = new XmlSerializer(obj.GetType());
                xmlserializer.Serialize(fs, obj);
                return true;
            }
        }

        public static object FromXML<T>(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
                object data = xmlserializer.Deserialize(fs);

                if (data != null)
                {
                    return data;
                }
            }
            return null;
        }

        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        public static void ToJson( string path,object obj)
        {
            FileUtil.WriteFile(path, ToJson(obj));
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T FromJsonFile<T>(string path)
        {   
            string json = FileUtil.ReadFile(path);
            return JsonConvert.DeserializeObject<T>(json);
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
            T t = ProtoBuf.Serializer.Deserialize<T>(new MemoryStream(bytes));
            return t;
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