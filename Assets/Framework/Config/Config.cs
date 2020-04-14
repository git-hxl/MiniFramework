namespace MiniFramework.Config
{
    public sealed partial class Config:Singleton<Config>
    {
        private Config() { }
        private   ConfigUrl configUrl;
        public  ConfigUrl GetConfigUrl
        { 
            get
            {
                if (configUrl == null)
                {
                    configUrl = new ConfigUrl();
                }
                return configUrl;
            }
        }
    }
}