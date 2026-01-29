using _Project.Develop.Logic.Movement;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;

public class RigidbodyRotatorDirection: IRotator
{
    private Vector3 _direction;
    private float _speed;
    private ReactiveVariable<bool> _isStopped = new (false);
    
    private readonly Rigidbody _rigidbody;
    
    private const float DeadZone = 0.05f;

    public RigidbodyRotatorDirection(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public IReadOnlyVariable<bool> IsStopped => _isStopped;

    public Quaternion CurrentRotation => _rigidbody.rotation;
    
    public void Update(float deltaTime)
    {
        if (_direction.sqrMagnitude < DeadZone)
            return;

        Vector3 xzDirection = new Vector3(_direction.x, 0f, _direction.z).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(xzDirection);
        Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _speed * deltaTime);

        _rigidbody.MoveRotation(newRotation);
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
