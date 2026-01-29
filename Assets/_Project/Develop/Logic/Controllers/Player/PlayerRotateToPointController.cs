using _Project.Develop.Services.Inputs;
using UnityEngine;

namespace _Project.Develop.Logic.Controllers
{
    public class PlayerRotateToPointController: Controller
    {
        private readonly LayerMask _rotatableMask;
        
        private IDirectionalRotatable _rotatable;
        private PlayerInput _input;
        
        public PlayerRotateToPointController(
            IDirectionalRotatable rotatable,
            PlayerInput input,
            LayerMask rotatableMask)
        {
            _rotatableMask = rotatableMask;
            _rotatable = rotatable;
            _input = input;
        }

        protected override void UpdateLogic(float deltaTime)
        {
            if (RaycastUtils.TryGetHitWithMask(Camera.main, _input.PointPosition, _rotatableMask, out RaycastHit hit))
            {
                Vector3 direction = hit.point - _rotatable.Position;
                
                _rotatable.SetRotateDirection(direction);
            }
        }
    }
}