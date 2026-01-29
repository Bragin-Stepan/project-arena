using UnityEngine;

public interface IDirectionalMovable : ITransformPosition, IStoppable
{
    Vector3 CurrentVelocity { get; }
    void SetMoveDirection(Vector3 direction);
}
