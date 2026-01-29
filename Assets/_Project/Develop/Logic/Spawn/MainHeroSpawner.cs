using System;
using _Project.Develop.Configs.Characters;
using _Project.Develop.Logic.Characters;
using _Project.Develop.Logic.Controllers;
using _Project.Develop.Logic.Controllers.Agent;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Develop.Logic.Spawn
{
    public class MainHeroSpawner
    {
        public event Action<Character> Spawned;
        
        private readonly CharacterConfigSO _config;
        private readonly ControllersUpdateService _controllersUpdateService;
        private readonly ControllersFactory _controllersFactory;
        private readonly CharactersFactory _charactersFactory;
        
        private CompositeController _currentControllers;

        public MainHeroSpawner(
            CharacterConfigSO config,
            ControllersUpdateService controllersUpdateService, 
            ControllersFactory controllersFactory, 
            CharactersFactory charactersFactory)
        {
            _config = config;
            _controllersUpdateService = controllersUpdateService;
            _controllersFactory = controllersFactory;
            _charactersFactory = charactersFactory;
        }
        
        public Character Spawn(Vector3 spawnPoint)
        {
            Character mainHero = _charactersFactory.Create(_config, spawnPoint);
            
            _currentControllers = new (_controllersFactory.CreateMainHeroController(
                mainHero,
                _config.LayerMaskToRotate
            ));
            
            _currentControllers.Enable();
            
            _controllersUpdateService.Add(_currentControllers);
            
            mainHero.Destroyed += OnDestroyed;
            Spawned?.Invoke(mainHero);
            
            return mainHero;
        }

        private void OnDestroyed(IDestroyable destroyable)
        {
            destroyable.Destroyed -= OnDestroyed;
            _controllersUpdateService.Remove(_currentControllers);
            _currentControllers.Disable();
        }
    }
}