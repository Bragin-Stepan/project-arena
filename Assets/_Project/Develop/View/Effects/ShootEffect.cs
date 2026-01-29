using _Project.Develop.Logic.Damage;
using UnityEngine;

namespace _Project.Develop.View
{
    public class ShootEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _shootWeapon;
        
        [SerializeField] private Transform _firePoint;
        
        [SerializeField] private GameObject _shootEffectPrefab;
        [SerializeField] private TrailEffect _trail;

        private IRangedWeapon _weapon;
        
        private Coroutine _currentTrailCoroutine;

        private void Awake()
        {
            if (_shootWeapon.TryGetComponent(out IRangedWeapon weapon))
            {
                _weapon = weapon;

                _weapon.OnShoot += OnShoot;
            }
        }

        private void OnShoot(Vector3 startPoint, Vector3 endPoint)
        {
            Instantiate(_shootEffectPrefab, _firePoint.position, Quaternion.LookRotation(endPoint - startPoint), null);
            _trail.Play(startPoint, endPoint);
        }

        private void OnDestroy()
        {
            if (_weapon == null)
                return;
            
            _weapon.OnShoot -= OnShoot;
            
            if (_currentTrailCoroutine != null)
                StopCoroutine(_currentTrailCoroutine);
        }
    }
}