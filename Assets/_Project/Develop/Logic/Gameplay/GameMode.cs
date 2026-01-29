using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Configs.Gameplay;
using _Project.Develop.Logic.Characters;
using _Project.Develop.Logic.Spawn;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using _Project.Develop.Utils;
using _Project.Develop.Utils.Audio;
using UnityEngine;

namespace _Project.Develop.Logic.Gameplay
{
    public class GameMode : IDisposable
    {
        public event Action OnWin;
        public event Action OnDefeat;

        public IReadOnlyVariable<int> CurrentTimePerSeconds => _currentTimePerSeconds;
        public IReadOnlyVariable<bool> MainHeroIsDead => _mainHeroIsDead;
        public IReadOnlyVariable<int> KilledEnemies => _killedEnemies;
        public IReadOnlyVariable<bool> IsRunning => _isRunning;
        public int EnemiesCount => _enemies.Count;

        private readonly CoroutineRunner _coroutineRunner;
        private readonly LevelConfigSO _levelConfig;
        private readonly Character _mainHero;
        private readonly AgentCharacterSpawner _agentCharacterSpawner;
        private readonly GameplayAudio _gameplayAudio;
        
        private readonly List<AgentCharacter> _enemies = new();
        private readonly Dictionary<IDestroyable, IEnumerator> _destroyCoroutines = new();

        private Func<GameMode, bool> _winCondition;
        private Func<GameMode, bool> _defeatCondition;
        
        private ReactiveVariable<bool> _isRunning = new();
        private ReactiveVariable<bool> _mainHeroIsDead = new();
        private ReactiveVariable<int> _killedEnemies = new();
        private ReactiveVariable<int> _currentTimePerSeconds = new();
        
        private const float TimeToDestroy = 1.5f;
        private const int TimeTick = 1;

        public GameMode(
            CoroutineRunner coroutineRunner,
            AudioService audioService,
            LevelConfigSO levelConfig,
            Character mainHero,
            AgentCharacterSpawner agentCharacterSpawner)
        {
            _coroutineRunner = coroutineRunner;
            _levelConfig = levelConfig;
            _mainHero = mainHero;
            _agentCharacterSpawner = agentCharacterSpawner;

            _gameplayAudio = new(
                audioService,
                _mainHero,
                IsRunning);
        }

        public void Start(Func<GameMode, bool> winCondition, Func<GameMode, bool> defeatCondition)
        {
            Debug.Log("GameMode Start");
            
            _winCondition = winCondition;
            _defeatCondition = defeatCondition;

            _agentCharacterSpawner.OnSpawned += OnSpawned;
            _mainHero.Dead += MainHeroOnDead;

            _agentCharacterSpawner.StartAutoSpawn(
                _levelConfig.EnemiesSpawnPoints, 
                _levelConfig.SpawnInterval);
            
            _isRunning.Value = true;
            _coroutineRunner.Start(TimerProcess());
        }

        public void Dispose()
        {
            _agentCharacterSpawner.OnSpawned -= OnSpawned;
            _mainHero.Dead -= MainHeroOnDead;
            
            _gameplayAudio.Dispose();
        }

        private void OnSpawned(AgentCharacter enemy)
        {
            if (_isRunning.Value == false) 
                return;
            
            if (enemy.TryGetComponent( out IDeadable deadable))
                deadable.Dead += OnEnemyDead;
            
            _enemies.Add(enemy);

            ConditionsProcess();
        }

        private void OnEnemyDead(IDeadable deadable)
        {
            if (_isRunning.Value == false) 
                return;
            
            if (deadable is IDestroyable destroyable)
            {
                IEnumerator coroutine = TimeToDestroyEnemy(destroyable);
                _destroyCoroutines[destroyable] = coroutine;
                _coroutineRunner.Start(coroutine);
            }
            
            _killedEnemies.Value++;

            ConditionsProcess();
        }

        private IEnumerator TimeToDestroyEnemy(IDestroyable destroyable)
        {
            yield return new WaitForSeconds(TimeToDestroy);
            
            AgentCharacter agentCharacter = destroyable as AgentCharacter;
            
            if (_enemies.Contains(agentCharacter))
                _enemies.Remove(agentCharacter);
            
            destroyable.Destroy();
            
            _destroyCoroutines.Remove(destroyable);
        }
        
        private void MainHeroOnDead(IDeadable deadable)
        {
            if (_isRunning.Value == false)
                return;
            
            _mainHeroIsDead.Value = true;
            ConditionsProcess();
        }

        private IEnumerator TimerProcess()
        {
            while (_isRunning.Value)
            {
                yield return new WaitForSeconds(TimeTick);
                _currentTimePerSeconds.Value += 1;
                ConditionsProcess();
            }
        }
        
        private void ConditionsProcess()
        {
            if (_isRunning.Value == false) 
                return;

            if (_winCondition != null && _winCondition(this))
            {
                StopGame();
                OnWin?.Invoke();
                return;
            }

            if (_defeatCondition != null && _defeatCondition(this))
            {
                StopGame();
                OnDefeat?.Invoke();
            }
        }

        private void StopGame()
        {
            if (_isRunning.Value == false)
                return;
            
            _isRunning.Value = false;
            
            _agentCharacterSpawner.StopAutoSpawn();
            _coroutineRunner.Stop(TimerProcess());
            
            foreach (IEnumerator coroutine in _destroyCoroutines.Values)
                _coroutineRunner.Stop(coroutine);
            
            _destroyCoroutines.Clear();
            
            foreach (AgentCharacter enemy in _enemies)
                enemy.Destroy();
            
            _enemies.Clear();
        }
    }
}