using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
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
        public static string ToXml(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer xmlserializer = new XmlSerializer(obj.GetType());
                xmlserializer.Serialize(stream, obj);
                return Encoding.UTF8.GetString(stream.GetBuffer());
            }
        }
        public static bool ToXml(string path, object obj)
        {
            using (StreamWriter fs = new StreamWriter(path))
            {
                XmlSerializer xmlserializer = new XmlSerializer(obj.GetType());
                xmlserializer.Serialize(fs, obj);
                return true;
            }
        }
        public static object FromXml<T>(string xml)
        {
            using (StringReader reader = new StringReader(xml))
            {
                XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
                object data = xmlserializer.Deserialize(reader);
                if (data != null)
                {
                    return data;
                }
            }
            return null;
        }
        public static object FromXmlFile<T>(string path)
        {
            using (StreamReader fs = new StreamReader(path))
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
            return JsonUtility.ToJson(obj);
        }
        public static void ToJson(string path, object obj)
        {
            FileUtil.WriteFile(path, ToJson(obj));
        }

        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static T FromJsonFile<T>(string path)
        {
            string json = FileUtil.ReadFile(path);
            return JsonUtility.FromJson<T>(json);
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