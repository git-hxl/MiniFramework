namespace MiniFramework.Config
{
    public class ConfigUrl
    {
#if !Release
        //AssetBundle更新下载地址
        public string ResUrl = "http://localhost:8080/";
        //App下载地址
        public string AppUrl = "";
        //登录请求地址
        public string LoginUrl = "";
#else
        //AssetBundle更新下载地址
        public string ResUrl = "http://localhost:8080/";
        //App下载地址
        public string AppUrl = "";
        //登录请求地址
        public string LoginUrl = "";
#endif
    }
}