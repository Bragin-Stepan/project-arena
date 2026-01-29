using System.Collections.Generic;
using _Project.Develop.Logic.Gameplay;
using UnityEngine;

namespace _Project.Develop.Configs.Gameplay
{
    [CreateAssetMenu(fileName = "LevelConfigSO", menuName = "Project/Gameplay/LevelConfigSO")]
    public class LevelConfigSO : ScriptableObject
    {
        [field: SerializeField] public Vector3 MainHeroSpawnPoint { get; private set; }
        [field: SerializeField] public List<Vector3> EnemiesSpawnPoints { get; private set; }
        [field: SerializeField] public GameplayWinType WinType { get; private set; }
        [field: SerializeField] public int WinValue { get; private set; }
        [field: SerializeField] public GameplayDefeatType DefeatType { get; private set; }
        [field: SerializeField] public int DefeatValue { get; private set; }
        [field: SerializeField] public float SpawnInterval { get; private set; } = 4f;
    }
}