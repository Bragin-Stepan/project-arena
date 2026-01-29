using System.Collections;
using UnityEngine;

namespace _Project.Develop.Utils
{
    public class CoroutineRunner
    {
        private MonoBehaviour _context;

        public CoroutineRunner(MonoBehaviour context)
        {
            _context = context;
        }
        
        public void Start(IEnumerator coroutine)
            => _context.StartCoroutine(coroutine);
        
        public void Stop(IEnumerator coroutine)
            => _context.StopCoroutine(coroutine);
    }
}