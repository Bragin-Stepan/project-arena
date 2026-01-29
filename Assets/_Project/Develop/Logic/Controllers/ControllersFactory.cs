using _Project.Develop.Logic.Characters;
using _Project.Develop.Logic.Controllers.Agent;
using _Project.Develop.Logic.Damage;
using _Project.Develop.Services.Inputs;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Develop.Logic.Controllers
{
    public class ControllersFactory
    {
        private readonly PlayerInput _input;
        
        public ControllersFactory(PlayerInput input)
        {
            _input = input;
        }

        public AgentRandomWalkMoveController CreateAgentRandomWalkMoveController(
            IDirectionalMovable movable,
            IDirectionalRotatable rotatable,
            NavMeshQueryFilter queryFilter,
            Vector3 target,
            Vector3 areaRadius,
            float timeToWait,
            float minDistanceToTarget = 0.05f)
        {
            return new AgentRandomWalkMoveController(movable, rotatable, queryFilter, target, areaRadius, timeToWait, minDistanceToTarget);
        }

        public PlayerMoveDirectionController CreatePlayerMoveDirectionController(IDirectionalMovable movable)
        {
            return new PlayerMoveDirectionController(movable, _input);
        }
        
        public PlayerRangeAttackDirectionController CreatePlayerRangeAttackDirectionController(
            IRangeDamager damager,
            IDirectionalRotatable rotatable)
        {
            return new PlayerRangeAttackDirectionController(damager, rotatable, _input);
        }
        
        public PlayerRangeAttackToPointController CreatePlayerRangeAttackToPointController(
            IRangeDamager damager,
            LayerMask hitMask)
        {
            return new PlayerRangeAttackToPointController(damager, _input, hitMask);
        }

        public PlayerRotateToPointController CreatePlayerRotateToPointController(
            IDirectionalRotatable rotatable,
            LayerMask rotatableMask)
        {
            return new PlayerRotateToPointController(rotatable, _input, rotatableMask);
        }

        public CompositeController CreateMainHeroController(Character character, LayerMask _layerMaskToRotate)
        {
            return new CompositeController(
                CreatePlayerMoveDirectionController(character),
                CreatePlayerRotateToPointController(character, _layerMaskToRotate),
                CreatePlayerRangeAttackToPointController(character, _layerMaskToRotate),
                CreatePlayerRangeAttackDirectionController(character, character));
        }
    }
}