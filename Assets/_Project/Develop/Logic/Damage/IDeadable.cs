using System;
using _Project.Develop.Runtime.Utils.ReactiveManagement;

public interface IDeadable : ITransformPosition
{
    event Action<IDeadable> Dead;
    void Kill();
}