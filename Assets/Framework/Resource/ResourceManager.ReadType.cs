namespace MiniFramework.Resource
{
    public partial class ResourceManager
    {
        public enum ReadType
        {
            None,//编辑器模式下直接读取
            FromStreamingAssets,
            FromPersistentPath, 
        }
    }
}

