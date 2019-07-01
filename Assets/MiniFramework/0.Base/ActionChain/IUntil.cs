using System;
namespace MiniFramework
{
    public interface IUntil
    {
        Sequence Until(Func<bool> condition);
    }
}