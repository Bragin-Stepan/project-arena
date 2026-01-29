using System;
using System.Collections.Generic;
using _Project.Develop.Configs.Audio;
using _Project.Develop.Configs.Characters;
using _Project.Develop.Configs.Gameplay;

namespace _Project.Develop.Shared.Const
{
    public static class ResourcesPath
    {
        public const string AudioMixer = "Configs/Audio/AudioMixer";
        public const string AudioBackground = "Configs/Audio/BackgroundSource";
        public const string AudioSFX = "Configs/Audio/SFXSource";

        public static readonly IReadOnlyDictionary<Type, string> ScriptableObjects = new Dictionary<Type, string>()
        {
            { typeof(AudioDatabaseSO), "Configs/Audio/AudioDatabaseSO" },
            { typeof(CharacterConfigSO), "Configs/Characters/CharacterConfigSO" },
            { typeof(AgentCharacterConfigSO), "Configs/Characters/AgentCharacterConfigSO" },
            { typeof(LevelConfigSO), "Configs/Gameplay/LevelConfigSO" },
        };
    }
}