using UnityEngine;
using UnityEngine.AI;

namespace _Project.Develop.Logic.Controllers.Agent
{
    public class AgentRandomWalkMoveController : Controller
    {
        private readonly IDirectionalMovable _movable;
        private readonly IDirectionalRotatable _rotatable;
        
        private readonly NavMeshQueryFilter _queryFilter;
        private readonly Vector3 _target;
        private readonly Vector3 _areaRadius;
        
        private readonly float _timeToWait;
        private readonly float _moveSpeed;
        private readonly float _minDistanceToTarget;

        private NavMeshPath _pathToTarget = new ();
        private float _currentTime;
        private Vector3 _targetPosition;
        private int _currentWaypointIndex = 0;
        private bool _isMoving;
        private bool _hasValidPath;
        
        private const int MaxAttempts = 10;
        private const float MaxDistanceOffsetRadius = 3f;
        private const float WaypointReachedDistance = 0.5f;
        private const float DeadZone = 0.1f;

        public AgentRandomWalkMoveController(
            IDirectionalMovable movable,
            IDirectionalRotatable rotatable,
            NavMeshQueryFilter queryFilter,
            Vector3 target,
            Vector3 areaRadius,
            float timeToWait,
            float minDistanceToTarget = 0.05f)
        {
            _movable = movable;
            _rotatable = rotatable;
            _queryFilter = queryFilter;
            _target = target;
            _areaRadius = areaRadius;
            _timeToWait = timeToWait;
            _minDistanceToTarget = minDistanceToTarget;
        }
        
        protected override void UpdateLogic(float deltaTime)
        {
            if (_isMoving)
                MovementProcess();
            else
                WaitingProcess(deltaTime);
        }
        
        private void MovementProcess()
        {
            if (_hasValidPath == false || _currentWaypointIndex >= _pathToTarget.corners.Length)
            {
                StopMovement();
                return;
            }
            
            Vector3 currentWaypoint = _pathToTarget.corners[_currentWaypointIndex];
            
            Vector3 direction = (currentWaypoint - _movable.Position).normalized;
            direction.y = 0;

            if (direction.magnitude > DeadZone)
                MoveToDirection(direction);

            float distanceToWaypoint = CalculateDistanceToTarget(_movable.Position, currentWaypoint);

            if (distanceToWaypoint <= WaypointReachedDistance)
            {
                _currentWaypointIndex++;
                
                if (_currentWaypointIndex >= _pathToTarget.corners.Length)
                {
                    float distance = CalculateDistanceToTarget(_movable.Position, _targetPosition);
                    
                    if (distance <= _minDistanceToTarget)
                        StopMovement();
                    else
                        RecalculatePath();
                }
            }
        }

        private void MoveToDirection (Vector3 direction)
        {
            _movable.SetMoveDirection(direction);
            _rotatable.SetRotateDirection(direction);
        }

        private void RecalculatePath()
        {
            _currentWaypointIndex = 0;
                
            _hasValidPath = NavMeshUtils.TryGetPath(
                _movable.Position, 
                _targetPosition, 
                _queryFilter, 
                _pathToTarget
            );
            
            if (_hasValidPath && _pathToTarget.corners.Length > 0)
                _currentWaypointIndex = Mathf.Clamp(_currentWaypointIndex, 0, _pathToTarget.corners.Length - 1);
            else
                _hasValidPath = false;
        }
        
        private void WaitingProcess(float deltaTime)
        {
            _currentTime += deltaTime;

            if (_currentTime > _timeToWait)
            {
                _currentTime = 0;
                
                if (TryFindRandomPoint(out Vector3 targetPosition))
                    StartMovement(targetPosition);
            }
        }
        
        private void StartMovement(Vector3 targetPosition)
        {
            ResumeMovement();
            
            _targetPosition = targetPosition;
            _currentWaypointIndex = 0;
            
            _hasValidPath = NavMeshUtils.TryGetPath(
                _movable.Position, 
                _targetPosition, 
                _queryFilter, 
                _pathToTarget
            );
            
            if(_hasValidPath == false && _pathToTarget.corners.Length == 0)
            {
                Vector3 direction = (_targetPosition - _movable.Position).normalized;
                direction.y = 0;
                
                ResumeMovement();
                
                _movable.SetMoveDirection(direction);
                _rotatable.SetRotateDirection(direction);
            }
        }

        private void ResumeMovement()
        {
            _isMoving = true;
            _movable.Resume();
            _rotatable.Resume();
        }

        private void StopMovement()
        {
            _movable.Stop();
            _rotatable.Stop();
            _isMoving = false;
            _currentTime = 0;
            _hasValidPath = false;
            _currentWaypointIndex = 0;
        }

        private bool TryFindRandomPoint(out Vector3 targetPosition)
        {
            targetPosition = Vector3.zero;
            
            for (int i = 0; i < MaxAttempts; i++)
            {
                Vector3 randomPointHorizontal = _target + new Vector3(
                    Random.Range(-_areaRadius.x, _areaRadius.x),
                    0,
                    Random.Range(-_areaRadius.z, _areaRadius.z)
                );
                
                if (NavMesh.SamplePosition(randomPointHorizontal, out NavMeshHit navMeshHit, MaxDistanceOffsetRadius, NavMesh.AllAreas))
                {
                    targetPosition = navMeshHit.position;
                    return true;
                }
            }

            return false;
        }
        
        private float CalculateDistanceToTarget(Vector3 currentPosition, Vector3 targetPosition)
            =>  Vector3.Distance(
                new Vector3(currentPosition.x, 0, currentPosition.z),
                new Vector3(targetPosition.x, 0, targetPosition.z));
    }
}