using System;
using _Project.Develop.Configs.Characters;
using _Project.Develop.Logic;
using _Project.Develop.Logic.Movement;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Develop.Logic.Characters
{
    [SelectionBase]
    public class AgentCharacter : 
        MonoBehaviour,
        IDirectionalRotatable, 
        IDirectionalMovable, 
        IDirectionalJumpable, 
        IDamageable, 
        IHealable, 
        IDeadable,
        IDestroyable
    {
        public event Action<float> Damaged;
        public event Action<IDestroyable> Destroyed;
        public event Action<IDeadable> Dead;
        
        [SerializeField] private NavMeshAgent _agent;
        
        public IReadOnlyVariable<bool> IsStopped => _mover.IsStopped;
        public IReadOnlyVariable<bool> CanHeal => _health.IsDead;
        public IReadOnlyVariable<float> HealthPercent => _health.Percent;
        
        public Vector3 Position => transform.position;
        public Vector3 TargetPosition => _agent.destination;
        public Vector3 CurrentVelocity => _mover.CurrentVelocity;
        public Quaternion CurrentRotation => _rotator.CurrentRotation;
        
        public bool InJumpProcess => _jumper.InProcess;
        
        private IMover _mover;
        private IRotator _rotator;
        private AgentJumper _jumper;
        private Health _health;
        
        private float _moveSpeed ;
        private float _rotateSpeed;
        private AnimationCurve _jumpCurve;
        
        private IDisposable _disposable;
        
        public void Initialize(IMover mover, IRotator rotator, AgentCharacterConfigSO config)
        {
            if (_agent == null)
                throw new NullReferenceException("Agent is null");
            
            _agent.updatePosition = false;
            _agent.updateRotation = false;
        
            _mover = mover;
            
            _moveSpeed = config.MoveSpeed;
            _rotateSpeed = config.RotateSpeed;
            
            _jumper = new AgentJumper(_agent, _moveSpeed, _jumpCurve, this);
            _rotator = rotator;

            _health = new Health(config.MaxHealth);

            _disposable = _health.IsDead.Subscribe(OnDead);
        }

        private void FixedUpdate()
        {
            if (_health.IsDead.Value)
                return;
            
            _mover.Update(Time.fixedDeltaTime);
            _rotator.Update(Time.fixedDeltaTime);
        }

        public void SetMoveDirection(Vector3 direction) => _mover.Move(direction, _moveSpeed);
        
        public void SetRotateDirection(Vector3 direction) => _rotator.Rotate(direction, _rotateSpeed);

        public IReadOnlyVariable<bool> CanTakeDamage => _health.IsDead;
        
        public void TakeDamage(float damage)
        {
            Damaged?.Invoke(damage);
            _health.TryReduce(damage);
        }
        
        public void Heal(float value) =>_health.TryIncrease(value);
        
        public void Kill()
        {
            Damaged?.Invoke(_health.Current.Value);
        }
        
        public void Stop() => _mover.Stop();
    
        public void Resume() => _mover.Resume();
    
        public bool IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData)
        {
            if (_agent.isOnOffMeshLink)
            {
                offMeshLinkData = _agent.currentOffMeshLinkData;
                return true;
            }
    
            offMeshLinkData = default(OffMeshLinkData);
            return false;
        }
    
        public void Jump(OffMeshLinkData offMeshLinkData) => _jumper.Jump(offMeshLinkData);
        
        public void Destroy()
        {
            Destroyed?.Invoke(this);
            Destroy(gameObject);
        }
        
        private void OnDead(bool oldValue, bool newValue)
        {
            Dead?.Invoke(this);
        }

        private void OnDestroy()
        {
            _health.Dispose();
            _disposable.Dispose();
        }
    }
}