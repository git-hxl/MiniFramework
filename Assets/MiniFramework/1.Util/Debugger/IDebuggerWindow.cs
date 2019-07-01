namespace MiniFramework
{
    public interface IDebuggerWindow
    {
        void Initialize(params object[] args);
        void OnDraw();
        void OnUpdate(float time,float realTime);
        void Close();
    }
}

