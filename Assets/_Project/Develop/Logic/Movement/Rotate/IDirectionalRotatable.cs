using UnityEngine;

public interface IDirectionalRotatable : ITransformPosition, IStoppable
{
    Quaternion CurrentRotation { get; }
    void SetRotateDirection(Vector3 direction);
}
