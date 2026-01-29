using System;
using UnityEngine;

namespace _Project.Develop.Logic.Damage
{
    public class Gun : MonoBehaviour, IRangedWeapon
    {
        [SerializeField] private float _damage = 5;
        [SerializeField] private Transform _firePoint;
     
        public event Action<Vector3, Vector3> OnShoot;
        public event Action<Vector3> OnHit;
        
        public void Shoot(Vector3 direction)
        {
            if (RaycastUtils.Shoot(_firePoint.position, direction, out RaycastHit hit))
            {
                OnShoot?.Invoke(_firePoint.position, hit.point);

                if (hit.collider)
                    OnHit?.Invoke(hit.point);
                
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(_damage);
            }
        }
    }
}