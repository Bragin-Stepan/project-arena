using System;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;

namespace _Project.Develop.Utils.Timer
{
    public class TimerService
    {
        public event Action<TimerState> ChangedState;

        public float CurrentTime { get; private set; }
        
        private ReactiveVariable<bool> _isRunning { get; } = new ();
        private ReactiveVariable<float> _nextTickTime { get; } = new();
        
        private float _tickInterval;
        
        public TimerService(float tickInterval = 1.0f)
        {
            _tickInterval = tickInterval;
            
            ResetTick();
        }
        
        public IReadOnlyVariable<bool> IsRunning => _isRunning;
        
        public IReadOnlyVariable<float> NextTickTime => _nextTickTime;

        public void Update(float deltaTime)
        {
            if (_isRunning.Value == false || _tickInterval <= 0)
                return;

            CurrentTime += deltaTime;
            
            while (CurrentTime >= _nextTickTime.Value)
                _nextTickTime.Value += _tickInterval;
        }

        public void Start()
        {
            _isRunning.Value = true;
            ChangedState?.Invoke(TimerState.Start);
        }

        public void Pause()
        {
            _isRunning.Value = false;
            ChangedState?.Invoke(TimerState.Pause);
        }

        public void Stop()
        {
            _isRunning.Value = false;
            ResetTick();
            ChangedState?.Invoke(TimerState.Stop);
        }

        public void Reset()
        {
            ResetTick();
            ChangedState?.Invoke(TimerState.Reset);
        }
        
        private void ResetTick()
        {
            CurrentTime = 0;
            
            if (_tickInterval <= 0) 
                return;
            
            float roundedTime = Mathf.Floor(CurrentTime / _tickInterval);
            _nextTickTime.Value = (roundedTime + 1) * _tickInterval;
        }
    }
}