using System;
using UnityEngine;

namespace _Project.Develop.Logic.Triggers
{
    public class DamageDealerTrigger : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _damageCooldown;

        private float _currentTime;

        private void FixedUpdate()
        {
            _currentTime += Time.fixedDeltaTime;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                if (_currentTime >= _damageCooldown)
                {
                    _currentTime = 0;
                    damageable.TakeDamage(_damage);
                }
            }
        }
    }
}

// Мне так было лень уже делать нормальное получение урона
// Ришил таким способом выкрутиться