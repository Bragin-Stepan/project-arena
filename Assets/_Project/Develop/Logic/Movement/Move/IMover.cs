using UnityEngine;

namespace _Project.Develop.Logic.Movement
{
    public interface IMover : IStoppable, IUpdatable
    {
        Vector3 CurrentVelocity { get; }
        void Move(Vector3 position, float speed);
    }
}