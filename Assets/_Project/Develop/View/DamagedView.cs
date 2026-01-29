using System;
using UnityEngine;

public class DamagedView : MonoBehaviour
{
     [SerializeField] private Animator _animator;
     [SerializeField] private GameObject _target;
     
     private IDamageable _damageable;
     
     private readonly int _damagedKey = Animator.StringToHash("Damaged");
     
     private void Start()
     {
         if (_target.TryGetComponent(out IDamageable damageable))
         {
             _damageable = damageable;
             
             _damageable.Damaged += OnDamaged;
         }
     }

     private void OnDamaged(float damage)
     {
         _animator.SetTrigger(_damagedKey);
     }


     private void OnDestroy()
     {
         _damageable.Damaged -= OnDamaged;;
     }
}
