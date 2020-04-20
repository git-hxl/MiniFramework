namespace MiniFramework.Resource
{
    public partial class ResourceManager
    {
        public enum ReadType
        {
            None,//不进行任何操作
            FromStreamingAssets,//读取本地资源
            FromPersistentPath, //检测更新，读取本地资源
        }
    }
}

