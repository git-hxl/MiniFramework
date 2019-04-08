using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;

public static class SerializeHelper
{
    public static bool SerializeBinary(string path, object obj)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            return true;
        }
    }

    public static object DeserializeBinary(string path)
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

    public static bool SerializeXML(string path, object obj)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            XmlSerializer xmlserializer = new XmlSerializer(obj.GetType());
            xmlserializer.Serialize(fs, obj);
            return true;
        }
    }

    public static object DeserializeXML<T>(string path)
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

    public static string SerializeJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static T DeserializeJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static byte[] SerializeProtoBuff(ObjectCreationDelegate obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static T DeserializeProtoBuff<T>(byte[] bytes)
    {
        T t = ProtoBuf.Serializer.Deserialize<T>(new MemoryStream(bytes));
        return t;
    }

    public static byte[] SerializeByMarshal(object obj)
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
    public static T DeserializeByMarshal<T>(byte[] data)
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