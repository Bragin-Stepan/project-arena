using UnityEngine;

namespace _Project.Develop.Logic.Movement
{
    public interface IRotator : IStoppable, IUpdatable
    {
        Quaternion CurrentRotation { get; }
        void Rotate(Vector3 direction, float speed);
    }
}