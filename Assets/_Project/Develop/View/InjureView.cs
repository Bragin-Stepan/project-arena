using System;
using UnityEngine;

public class InjureView : MonoBehaviour
{
     [SerializeField] private Animator _animator;
     [SerializeField] private GameObject _target;
     
     private int _injureLayerIndex;
     
     private const float InjureHealthPercent = 0.3f;
     
     private const float MinLayerWeightValue = 0;
     private const float MaxLayerWeightValue = 1;
     
     private const string InjureLayerName = "InjuredLayer";
     
     private IHealable _healTaker;
     
     private void Start()
     {
         if (_target.TryGetComponent(out IHealable healTaker))
         {
             _healTaker = healTaker;

             _healTaker.HealthPercent.Subscribe(HealthPercentChanged);
         }
         
         _injureLayerIndex = _animator.GetLayerIndex(InjureLayerName);
     }

     private void HealthPercentChanged(float oldValue, float newValue)
     {
         _animator.SetLayerWeight(_injureLayerIndex,
             newValue < InjureHealthPercent ?
                 MaxLayerWeightValue : 
                 MinLayerWeightValue);
     }
}
