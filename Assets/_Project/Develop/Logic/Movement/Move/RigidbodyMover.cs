using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;

namespace _Project.Develop.Logic.Movement
{
    public class RigidbodyMover: IMover
    {
        private Vector3 _currentDirection;
        private Rigidbody _rigidbody;
        private float _speed;
        private ReactiveVariable<bool> _isStopped = new (false);
        
        private const float DeadZone = 0.1f;
        
        public RigidbodyMover(Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;
        }
        
        public void Update(float deltaTime)
        {
            if (_currentDirection.sqrMagnitude < DeadZone || _isStopped.Value)
            {
                _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
                return;
            }

            Vector3 xzDirection = new (_currentDirection.x, 0f, _currentDirection.z);
            Vector3 velocity = xzDirection * _speed;
            
            _rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
        }

        public IReadOnlyVariable<bool> IsStopped => _isStopped;

        public Vector3 CurrentVelocity => _rigidbody.velocity;

        public void Move(Vector3 direction, float speed)
        {
            _currentDirection = direction.normalized;
            _speed = speed;
        }

        public void Stop() {
            _currentDirection = Vector3.zero;
            _isStopped.Value = true;
        }
        
        public void Resume()
        {
            _isStopped.Value = false;
        }
    }
}