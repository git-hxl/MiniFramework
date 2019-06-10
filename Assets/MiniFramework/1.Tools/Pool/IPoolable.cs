namespace MiniFramework
{
    public interface IPoolable
    {
        void OnRecycled();
        bool IsRecycled { get; set; }
    }
}