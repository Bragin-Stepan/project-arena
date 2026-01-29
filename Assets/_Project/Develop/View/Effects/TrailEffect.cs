using System.Collections;
using UnityEngine;

namespace _Project.Develop.View
{
    public class TrailEffect : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailShootPrefab;
        [SerializeField] private float _trailSpeed = 100f;

        public void Play(Vector3 startPoint, Vector3 endPoint)
        {
            TrailRenderer trail = Instantiate(_trailShootPrefab, startPoint, Quaternion.identity, null);
            
            StartCoroutine(AnimateTrail(trail, startPoint, endPoint));
        }

        private IEnumerator AnimateTrail(TrailRenderer trail, Vector3 startPoint, Vector3 endPoint)
        {
            trail.Clear();
            
            float distance = Vector3.Distance(startPoint, endPoint);
            float travelTime = distance / _trailSpeed;
            
            float elapsedTime = 0f;
            
            while (elapsedTime < travelTime)
            {
                if (trail == null) yield break;
                
                float t = elapsedTime / travelTime;
                trail.transform.position = Vector3.Lerp(startPoint, endPoint, t);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            if (trail != null)
            {
                trail.transform.position = endPoint;
                
                yield return new WaitForSeconds(trail.time);
                
                Destroy(trail.gameObject);
            }
        }
    }
}