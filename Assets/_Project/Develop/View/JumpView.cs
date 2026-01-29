using System;
using UnityEngine;

public class JumpView : MonoBehaviour
{
     [SerializeField] private Animator _animator;
     [SerializeField] private GameObject _target;
     
     private IDirectionalJumpable _jumpable;
     
     private readonly int _inJumpProcessKey = Animator.StringToHash("InJumpProcess");

     private void Start()
     {
         if (_target.TryGetComponent(out IDirectionalJumpable jumpable))
             _jumpable = jumpable;
     }

     private void LateUpdate()
     {
          _animator.SetBool(_inJumpProcessKey, _jumpable.InJumpProcess);
     }
}
