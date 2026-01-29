using System;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;

namespace _Project.Develop.Services.Inputs
{
    public class PlayerInput
    {
        public event Action<Vector3> OnClicked;
        public event Action OnAttackPressed;
        public event Action OnRestartPressed;
        public event Action<Vector2> OnDirectionInput;

        public Vector3 PointPosition => Input.mousePosition;
        public Vector2 DirectionInput => _lastInput;
        
        private Vector2 _lastInput = Vector2.zero;
        
        private const KeyCode AttackKey = KeyCode.E;
        private const KeyCode RestartKey = KeyCode.R;
        
        private const int LeftMouseButtonKey = 0;
        private const int RightMouseButtonKey = 1;
        
        private const float DeadZone = 0.1f;

        public void Update()
        {
            HandleMovementInput();
            HandleActionInputs();
        }

        private void HandleMovementInput()
        {
            Vector2 currentInput = new (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            if (currentInput.magnitude < DeadZone)
                currentInput = Vector2.zero;
            
            if (currentInput != _lastInput)
            {
                OnDirectionInput?.Invoke(currentInput);
                _lastInput = currentInput;
            }
        }

        private void HandleActionInputs()
        {
            if (Input.GetKeyDown(AttackKey))
                OnAttackPressed?.Invoke();
            
            if (Input.GetKeyDown(RestartKey))
                OnRestartPressed?.Invoke();
            
            if (Input.GetMouseButtonDown(LeftMouseButtonKey))
                OnClicked?.Invoke(PointPosition);
        }
    }
}