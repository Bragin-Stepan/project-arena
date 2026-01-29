using System;

namespace _Project.Develop.Runtime.Utils.ReactiveManagement
{
    public interface IReadOnlyVariable<T>
    {
        T Value { get;  }
        
        IDisposable Subscribe(Action<T, T> action);
    }
}
