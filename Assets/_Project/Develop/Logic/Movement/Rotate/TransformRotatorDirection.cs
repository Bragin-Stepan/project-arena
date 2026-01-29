using _Project.Develop.Logic.Movement;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;

public class TransformRotatorDirection: IRotator
{
    private readonly Transform _transform;
    private Vector3 _direction;
    private float _speed;
    private ReactiveVariable<bool> _isStopped = new (false);
    
    private const float DeadZone = 0.05f;

    public TransformRotatorDirection(Transform transform)
    {
        _transform = transform;
    }
    
    public IReadOnlyVariable<bool> IsStopped => _isStopped;

    public Quaternion CurrentRotation => _transform.rotation;

    public void Update(float deltaTime)
    {
        if (_direction.sqrMagnitude < DeadZone)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(_direction.normalized);
        float step = _speed * deltaTime;

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, lookRotation, step);
    }

    public void Rotate(Vector3 direction, float speed)
    {
        if (_isStopped.Value)
            return;
                
        _direction = direction;
        _speed = speed;
    }

    public void Stop()
    {
        _isStopped.Value = true;
    }
    
    public void Resume()
    {
        _isStopped.Value = false;
    }
}
