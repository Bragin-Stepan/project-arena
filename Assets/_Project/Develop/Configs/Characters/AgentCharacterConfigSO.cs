using _Project.Develop.Logic.Characters;
using UnityEngine;

namespace _Project.Develop.Configs.Characters
{
    [CreateAssetMenu(fileName = "AgentCharacterConfigSO", menuName = "Project/Characters/AgentCharacterConfigSO")]
    public class AgentCharacterConfigSO : ScriptableObject
    {
        [field: SerializeField] public AgentCharacter Prefab { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5;
        [field: SerializeField] public float RotateSpeed { get; private set; } = 900;
        [field: SerializeField] public float MaxHealth { get; private set; } = 20;
        [field: SerializeField] public float JumpSpeed { get; private set; } = 5;
        [field: SerializeField] public float TimeToWait { get; private set; } = 3;
        [field: SerializeField] public Vector3 AreaRadius { get; private set; } = new(8f, 0, 8f);
    }
}