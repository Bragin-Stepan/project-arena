using _Project.Develop.Logic.Damage;
using UnityEngine;

namespace _Project.Develop.View
{
    public class RangeHitEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _shootWeapon;
        [SerializeField] private GameObject _hitEffectPrefab;

        private IRangedWeapon _weapon;

        private void Awake()
        {
            if (_shootWeapon.TryGetComponent(out IRangedWeapon weapon))
            {
                _weapon = weapon;

                _weapon.OnHit += OnHit;
            }
        }

        private void OnHit(Vector3 point)
        {
            Instantiate(_hitEffectPrefab, point, Quaternion.identity, null);
        }

        private void OnDestroy()
        {
            if (_weapon == null)
                return;
            
            _weapon.OnHit -= OnHit;
        }
    }
}