using _Project.Develop.Logic.Damage;
using _Project.Develop.Services.Inputs;
using UnityEngine;

namespace _Project.Develop.Logic.Controllers
{
    public class PlayerRangeAttackToPointController: Controller
    {
        private readonly IRangeDamager _damager;
        private readonly LayerMask _hitMask;
        private readonly PlayerInput _input;
    
        public PlayerRangeAttackToPointController(
            IRangeDamager damager,
            PlayerInput input,
            LayerMask hitMask)
        {
            _damager = damager;
            _hitMask = hitMask;
            _input = input;
        }

        public override void Enable()
        {
            base.Enable();
            
            _input.OnClicked += OnClicked;
        }
        

        private void OnClicked(Vector3 hitPoint)
        {
            if (RaycastUtils.TryGetHitWithMask(Camera.main, hitPoint, _hitMask, out RaycastHit hit))
            {
                Vector3 direction = hit.point - _damager.Position;
                
                _damager.RangeAttack(direction);
            }
        }

        protected override void UpdateLogic(float deltaTime)
        {            
        }

        public override void Disable()
        {
            base.Disable();
            
            _input.OnClicked -= OnClicked;
        }
    }
}
