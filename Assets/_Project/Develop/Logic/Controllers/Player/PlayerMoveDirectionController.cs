using System;
using _Project.Develop.Services.Inputs;
using UnityEngine;

namespace _Project.Develop.Logic.Controllers
{
    public class PlayerMoveDirectionController: Controller, IDisposable
    {
        private IDirectionalMovable _movable;
        private PlayerInput _input;
        
        public PlayerMoveDirectionController(
            IDirectionalMovable movable,
            PlayerInput input)
        {
            _movable = movable;
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
            _movable.SetMoveDirection(new Vector3(direction.x, 0, direction.y));
        }

        public override void Dispose()
        {
            _input.OnDirectionInput -= OnDirectionInput;
        }
    }
}