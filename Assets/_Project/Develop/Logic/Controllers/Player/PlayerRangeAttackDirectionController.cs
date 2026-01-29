using _Project.Develop.Logic.Damage;
using _Project.Develop.Services.Inputs;
using UnityEngine;

namespace _Project.Develop.Logic.Controllers
{
    public class PlayerRangeAttackDirectionController: Controller
    {
        private readonly IRangeDamager _damager;
        private readonly IDirectionalRotatable _rotatable;
        private readonly PlayerInput _input;
    
        public PlayerRangeAttackDirectionController(
            IRangeDamager damager,
            IDirectionalRotatable rotatable,
            PlayerInput input)
        {
            _damager = damager;
            _rotatable = rotatable;
            _input = input;
        }

        public override void Enable()
        {
            base.Enable();
            
            _input.OnAttackPressed += OnAttackPressed;
        }

        private void OnAttackPressed()
        {
            Vector3 direction = _rotatable.CurrentRotation * Vector3.forward;
            _damager.RangeAttack(direction);
        }

        protected override void UpdateLogic(float deltaTime)
        {            
        }

        public override void Disable()
        {
            base.Disable();
            
            _input.OnAttackPressed -= OnAttackPressed;
        }
    }
}