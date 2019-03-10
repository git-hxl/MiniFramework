namespace MiniFramework
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public AssetLoader AssetLoader;
        public SceneLoader SceneLoader;

        protected override void OnSingletonInit()
        {
            AssetLoader = new AssetLoader(this);
            SceneLoader = new SceneLoader(this);
        }
    }
}

