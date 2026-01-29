using System;
using _Project.Develop.Configs.Characters;
using _Project.Develop.Logic.Damage;
using _Project.Develop.Logic.Movement;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;

namespace _Project.Develop.Logic.Characters
{
    [SelectionBase]
    public class Character : 
        MonoBehaviour, 
        IDirectionalRotatable,
        IDirectionalMovable, 
        IRangeDamager,
        IDamageable,
        IDeadable
    {
        [SerializeField] private Gun _gun;
        
        public event Action<float> Damaged;
        public event Action<IDeadable> Dead;

        public event Action<Vector3, Vector3> OnShoot
        {
            add => _gun.OnShoot += value;
            remove => _gun.OnShoot -= value;
        }

        private IMover _mover;
        private IRotator _rotator;
        private Health _health;

        private float _moveSpeed;
        private float _rotateSpeed;

        public IReadOnlyVariable<bool> IsStopped => _mover.IsStopped;
        public Vector3 Position => transform.position;
        public Vector3 CurrentVelocity => _mover.CurrentVelocity;
        public Quaternion CurrentRotation => _rotator.CurrentRotation;
        
        public void Initialize(
            IMover mover,
            IRotator rotator,
            CharacterConfigSO config)
        {
            _mover = mover;
            _rotator = rotator;
            
            _moveSpeed = config.MoveSpeed;
            _rotateSpeed = config.RotationSpeed;
            
            _health = new Health(config.MaxHealth);

            _health.IsDead.Subscribe(OnDead);
        }

        private void FixedUpdate()
        {
            _mover.Update(Time.fixedDeltaTime);
            _rotator.Update(Time.fixedDeltaTime);
        }
        
        public void RangeAttack(Vector3 direction) => _gun.Shoot(direction);

        public void SetMoveDirection(Vector3 position) => _mover.Move(position, _moveSpeed);
        
        public void SetRotateDirection(Vector3 direction) => _rotator.Rotate(direction, _rotateSpeed);

        public IReadOnlyVariable<bool> CanTakeDamage => _health.IsDead;

        public void TakeDamage(float damage)
        {
            Damaged?.Invoke(damage);
            _health.TryReduce(damage);
        }
        
        public void Kill() => _health.Kill();
        
        public void Heal(float value) =>_health.TryIncrease(value);
        
        public void Stop() => _mover.Stop();
    
        public void Resume() => _mover.Resume();
        
        private void OnDead(bool oldValue, bool newValue)
        {
            if (newValue)
                Dead?.Invoke(this);
        }
    }
}