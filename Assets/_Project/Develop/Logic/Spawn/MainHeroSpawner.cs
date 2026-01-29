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
        private readonly CharacterConfigSO _config;
        private readonly ControllersUpdateService _controllersUpdateService;
        private readonly ControllersFactory _controllersFactory;
        private readonly CharactersFactory _charactersFactory;

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
            Character enemy = _charactersFactory.Create(_config, spawnPoint);
            
            CompositeController characterControllers = new (_controllersFactory.CreateMainHeroController(
                enemy,
                _config.LayerMaskToRotate
            ));
            
            characterControllers.Enable();
            
            _controllersUpdateService.Add(characterControllers);
            
            return enemy;
        }
    }
}