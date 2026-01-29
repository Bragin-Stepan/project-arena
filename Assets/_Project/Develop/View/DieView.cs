using System;
using UnityEngine;

public class DieView : MonoBehaviour
{
     [SerializeField] private Animator _animator;
     [SerializeField] private GameObject _target;
     
     private readonly int _isDeadKey = Animator.StringToHash("IsDead");
     
     private IDeadable _deadable;
     
     private void Start()
     {
         if (_target.TryGetComponent(out IDeadable deadable))
         {
             _deadable = deadable;
             
             _deadable.Dead += OnDead;
         }
     }

     private void OnDead(IDeadable deadable)
     {
         _animator.SetBool(_isDeadKey, true);
     }

     private void OnDisable()
     {
         _deadable.Dead -= OnDead;
     }
}
