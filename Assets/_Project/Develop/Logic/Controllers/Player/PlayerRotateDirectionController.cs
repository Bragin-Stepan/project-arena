using System;
using _Project.Develop.Services.Inputs;
using UnityEngine;

namespace _Project.Develop.Logic.Controllers
{
    public class PlayerRotateDirectionController: Controller
    {
        private IDirectionalRotatable _rotatable;
        private PlayerInput _input;
        
        public PlayerRotateDirectionController(
            IDirectionalRotatable rotatable,
            PlayerInput input)
        {
            _rotatable = rotatable;
            _input = input;
        }

        public override void Enable()
        {
            base.Enable();
            _input.OnDirectionInput += OnDirectionInput;
        }

        protected override void UpdateLogic(float deltaTime)
        { }
        
        private void OnDirectionInput(Vector2 direction)
        {
            _rotatable.SetRotateDirection(new Vector3(direction.x, 0, direction.y));
        }

        public override void Dispose()
        {
            _input.OnDirectionInput -= OnDirectionInput;
        }
    }
}