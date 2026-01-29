using System;
using System.Collections;
using _Project.Develop.Configs.Characters;
using _Project.Develop.Configs.Gameplay;
using _Project.Develop.Logic.Characters;
using _Project.Develop.Logic.Controllers;
using _Project.Develop.Logic.Spawn;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using _Project.Develop.Services.Inputs;
using _Project.Develop.Shared.Const;
using _Project.Develop.Utils;
using _Project.Develop.Utils.Audio;
using UnityEngine;

namespace _Project.Develop.Logic.Gameplay
{
    public class GameplayCycle : IDisposable
    {
        private readonly AudioService _audioService;
        private readonly ConfigsLoader _configsLoader;
        private readonly PlayerInput _playerInput;
        private readonly CoroutineRunner _coroutineRunner;

        private readonly ControllersUpdateService _controllersUpdateService = new ();
        private readonly GameplayConditionFactory _conditionsFactory = new ();
        
        private GameMode _gameMode;
        private MainHeroSpawner _mainHeroSpawner;
        private AgentCharacterSpawner _agentCharacterSpawner;
        private LevelConfigSO _levelConfig;
        
        public GameplayCycle(
            CoroutineRunner coroutineRunner,
            ConfigsLoader configsLoader,
            AudioService audioService,
            PlayerInput playerInput)
        {
            _coroutineRunner = coroutineRunner;
            _audioService = audioService;
            _configsLoader = configsLoader;
            _playerInput = playerInput;
        }

        public IEnumerator Start()
        {
            _gameMode = new (_coroutineRunner, _audioService, _levelConfig, _mainHeroSpawner, _agentCharacterSpawner);

            _gameMode.OnWin += OnGameModeWin;
            _gameMode.OnDefeat += OnGameModeDefeat;

            Func<GameMode, bool> winCondition = _conditionsFactory.CreateWinCondition(_levelConfig.WinType, _levelConfig.WinValue);
            Func<GameMode, bool> defeatCondition = _conditionsFactory.CreateDefeatCondition(_levelConfig.DefeatType, _levelConfig.DefeatValue);

            yield return _gameMode.Start(winCondition, defeatCondition);
        }

        public void Update(float deltaTime)
        {
            _controllersUpdateService.Update(deltaTime);
        }
        
        public IEnumerator LoadAsync()
        {
            _levelConfig = _configsLoader.GetConfig<LevelConfigSO>();
            
            CharacterConfigSO characterConfig = _configsLoader.GetConfig<CharacterConfigSO>();
            AgentCharacterConfigSO agentCharacterConfig = _configsLoader.GetConfig<AgentCharacterConfigSO>();
            
            ControllersFactory controllersFactory = new (_playerInput);
            CharactersFactory charactersFactory = new ();
            
            _mainHeroSpawner = new (characterConfig, _controllersUpdateService, controllersFactory, charactersFactory);
            _agentCharacterSpawner = new (_coroutineRunner, agentCharacterConfig, _controllersUpdateService, controllersFactory, charactersFactory);
            
            yield return null;
        }
        
        public void Dispose()
        {
            OnGameModeEnded();
        }
        
        private void OnGameModeDefeat()
        {
            Debug.Log("Defeat");
            OnGameModeEnded();

            _coroutineRunner.Start(Start());
        }

        private void OnGameModeWin()
        {
            Debug.Log("Win");
            OnGameModeEnded();
            
            _coroutineRunner.Start(Start());
        }

        private void OnGameModeEnded()
        {
            _gameMode.Dispose();
            _gameMode.OnWin -= OnGameModeWin;
            _gameMode.OnDefeat -= OnGameModeDefeat;
        }
    }
}