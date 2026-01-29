using System;

namespace _Project.Develop.Logic.Controllers
{
    public abstract class Controller : IDisposable
    {
        private bool _isEnabled;

        public void Update(float deltaTime)
        {
            if (_isEnabled)
                UpdateLogic(deltaTime);
        }

        protected abstract void UpdateLogic(float deltaTime);

        public virtual void Enable() => _isEnabled = true;

        public virtual void Disable()
        {
            Dispose();
            _isEnabled = false;
        }
        
        public virtual void Dispose()
        {  }
    }
}