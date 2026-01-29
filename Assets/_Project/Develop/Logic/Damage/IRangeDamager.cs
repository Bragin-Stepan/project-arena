using System;
using UnityEngine;

namespace _Project.Develop.Logic.Damage
{
    public interface IRangeDamager : ITransformPosition
    {
        event Action<Vector3, Vector3> OnShoot;
        void RangeAttack(Vector3 direction);
    }
}