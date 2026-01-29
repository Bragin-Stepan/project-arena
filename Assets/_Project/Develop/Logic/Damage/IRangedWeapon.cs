using System;
using UnityEngine;

namespace _Project.Develop.Logic.Damage
{
    public interface IRangedWeapon
    {
        event Action<Vector3, Vector3> OnShoot;
        event Action<Vector3> OnHit;
        void Shoot(Vector3 direction);
    }
}