using System;
using UnityEngine;

namespace _Project.Develop
{
    public interface IDestroyable : ITransformPosition
    {
        event Action<IDestroyable> Destroyed;
        void Destroy();
    }
}