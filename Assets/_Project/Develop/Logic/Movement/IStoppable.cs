using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;

public partial interface IStoppable
{
    void Stop();
    void Resume();
    IReadOnlyVariable<bool> IsStopped { get; }
}