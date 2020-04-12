namespace MiniFramework.Config
{
    public sealed partial class Config
    {
        private static ConfigUrl configUrl;
        public static ConfigUrl URL
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