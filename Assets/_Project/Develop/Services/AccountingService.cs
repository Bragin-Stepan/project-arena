using System;
using System.Collections.Generic;
using _Project.Develop.Logic.Characters;

namespace _Project.Develop
{
    public class AccountingService<T>
    {
        public event Action<T> OnAdded;
        public event Action<T> OnRemoved;
        
        public IReadOnlyList<T> List => _list;
        
        private List<T> _list = new();

        public void Add(T obj)
        {
            if (Exists(obj))
                throw new ArgumentException($"[Accounting] Object {obj} already exists in the list");
            
            _list.Add(obj);
            OnAdded?.Invoke(obj);
        }

        public void Remove(T obj)
        {
            if (Exists(obj) == false)
                throw new ArgumentException($"[Accounting] Object {obj} not found in the list");
            
            OnRemoved?.Invoke(obj);
            _list.Remove(obj);
        }

        public bool Exists(T obj) => _list.Contains(obj);
        
        public void Clear() => _list.Clear();
    }
}