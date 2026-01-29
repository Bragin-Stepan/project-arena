using System;
using _Project.Develop.Runtime.Utils.ReactiveManagement;

public interface IDamageable
{
    IReadOnlyVariable<bool> CanTakeDamage { get; }
    event Action<float> Damaged; 
    void TakeDamage(float damage);
}