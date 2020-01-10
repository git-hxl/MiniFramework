namespace MiniFramework
{
    /// <summary>
    /// 用于对数据的操作
    /// </summary>
    class Controller:Singleton<Controller>
    {
        private Model model;

        public void Init()
        {
            model = new Model();
        }
    }
}
