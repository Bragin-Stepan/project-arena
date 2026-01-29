using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Configs.Characters;
using _Project.Develop.Logic.Characters;
using _Project.Develop.Logic.Controllers;
using _Project.Develop.Utils;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Develop.Logic.Spawn
{
    public class AgentCharacterSpawner
    {
        public event Action<AgentCharacter> OnSpawned;
        
        private readonly AgentCharacterConfigSO _config;
        private readonly ControllersUpdateService _controllersUpdateService;
        private readonly ControllersFactory _controllersFactory;
        private readonly CharactersFactory _charactersFactory;
        private readonly CoroutineRunner _coroutineRunner;

        private bool _isAutoSpawn;
        private float _spawnInterval;
        private List<Vector3> _spawnPoints;
        
        private readonly Dictionary<AgentCharacter, Controller> _charactersControllers = new();

        public AgentCharacterSpawner(
            CoroutineRunner coroutineRunner,
            AgentCharacterConfigSO config,
            ControllersUpdateService controllersUpdateService, 
            ControllersFactory controllersFactory, 
            CharactersFactory charactersFactory)
        {
            _config = config;
            _coroutineRunner = coroutineRunner;
            _controllersUpdateService = controllersUpdateService;
            _controllersFactory = controllersFactory;
            _charactersFactory = charactersFactory;
        }
        
        public AgentCharacter Spawn(Vector3 spawnPoint)
        {
            AgentCharacter agentCharacter = _charactersFactory.Create(_config, spawnPoint);
            
            NavMeshQueryFilter navMeshFilter = new()
            {
                agentTypeID = 0,
                areaMask = 1
            };
            
            CompositeController characterControllers = new (_controllersFactory.CreateAgentRandomWalkMoveController(
                agentCharacter,
                agentCharacter,
                navMeshFilter,
                spawnPoint,
                _config.AreaRadius,
                _config.TimeToWait
            ));
            
            characterControllers.Enable();
            
            _charactersControllers.Add(agentCharacter, characterControllers);
            _controllersUpdateService.Add(characterControllers);
            
            if (agentCharacter.TryGetComponent(out IDestroyable destroyable))
                destroyable.Destroyed += OnDestroyed;
            
            OnSpawned?.Invoke(agentCharacter);
            
            return agentCharacter;
        }

        public void StartAutoSpawn(List<Vector3> spawnPoints, float spawnInterval)
        {
            _spawnPoints = spawnPoints;
            _spawnInterval = spawnInterval;
            _isAutoSpawn = true;
            
            _coroutineRunner.Start(SpawnProcess());
        }
        
        public void StopAutoSpawn()
        {
            _isAutoSpawn = false;
            _coroutineRunner.Stop(SpawnProcess());
        }
        
        private IEnumerator SpawnProcess()
        {
            while (_isAutoSpawn)
            {
                yield return new WaitForSeconds(_spawnInterval);

                if (_isAutoSpawn == false)
                    yield break;

                Vector3 randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

                Spawn(randomPoint);
            }
        }

        private void OnDestroyed(IDestroyable destroyable)
        {
            destroyable.Destroyed -= OnDestroyed;
            
            if (_charactersControllers.TryGetValue(destroyable as AgentCharacter, out Controller controller))
            {
                controller.Disable();
                _charactersControllers.Remove(destroyable as AgentCharacter);
            }
        }
    }
    
    // Решил что если он включает контроллеры, значит ему их и выключать
    // 
    // Наверное лучше вообще управление контроллерами вынести в отдельный сервис, но на данную задачу оставил так
}