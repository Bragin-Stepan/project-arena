using UnityEngine;

namespace _Project.Develop.View
{
    public class DestroyEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _effectPrefab;
        [SerializeField] private GameObject _target;
        
        private IDestroyable _destroyable;

        private void Awake()
        {
            if (_target.TryGetComponent(out IDestroyable destroyable))
            {
                _destroyable = destroyable;

                _destroyable.Destroyed += OnDestroyed;
            }
        }

        private void OnDestroyed(IDestroyable destroyable)
        {
            Instantiate(_effectPrefab, destroyable.Position, Quaternion.identity, null);
        }

        private void OnDestroy()
        {
            if (_target == null)
                return;
            
            _destroyable.Destroyed -= OnDestroyed;
        }
    }
}