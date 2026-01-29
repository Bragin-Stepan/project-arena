using _Project.Develop.Logic.Characters;
using UnityEngine;

namespace _Project.Develop.Configs.Characters
{
    [CreateAssetMenu(fileName = "CharacterConfigSO", menuName = "Project/Characters/CharacterConfigSO")]
    public class CharacterConfigSO : ScriptableObject
    {
        [field: SerializeField] public Character Prefab { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField] public float MaxHealth { get; private set; } = 60;
        [field: SerializeField] public LayerMask LayerMaskToRotate { get; private set; }
    }
}