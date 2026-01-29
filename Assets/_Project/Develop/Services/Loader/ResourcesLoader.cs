using UnityEngine;

namespace _Project.Develop.Utils
{
    public class ResourcesLoader
    {
        public T Load<T>(string path) where T : Object
            => Resources.Load<T>(path);
    }
}