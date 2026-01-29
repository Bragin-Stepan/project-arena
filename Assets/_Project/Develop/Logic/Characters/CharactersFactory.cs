using _Project.Develop.Configs.Characters;
using _Project.Develop.Logic.Movement;
using UnityEngine;

namespace _Project.Develop.Logic.Characters
{
    public class CharactersFactory
    {
        public Character Create(
            CharacterConfigSO config,
            Vector3 spawnPosition)
        {
            Character character = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity, null);
            
            Rigidbody rigidbody = character.GetComponent<Rigidbody>();
            
            IMover _mover = new RigidbodyMover(rigidbody);
            IRotator _rotator = new RigidbodyRotatorDirection(rigidbody);
            
            character.Initialize(_mover, _rotator, config);
            
            return character;
        }

        public AgentCharacter Create(
            AgentCharacterConfigSO config,
            Vector3 spawnPosition)
        {
            AgentCharacter character = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity, null);
            
            Rigidbody rigidbody = character.GetComponent<Rigidbody>();
            
            IMover _mover = new RigidbodyMover(rigidbody);
            IRotator _rotator = new RigidbodyRotatorDirection(rigidbody);
            
            character.Initialize(_mover, _rotator, config);
            
            return character;
        }
    }
}