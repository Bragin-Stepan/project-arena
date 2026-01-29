using _Project.Develop.Logic.Movement;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using UnityEngine;
using UnityEngine.AI;

public class AgentMover: IMover
{
    private ReactiveVariable<bool> _isStopped = new (false);
    
    private NavMeshAgent _agent;
    
    public AgentMover(NavMeshAgent agent)
    {
        _agent = agent;
        _agent.acceleration = 999;
    }
    
    public Vector3 CurrentVelocity => _agent.desiredVelocity;
    
    public IReadOnlyVariable<bool> IsStopped => _isStopped;
    
    public void Update(float deltaTime)
    {

    }

    public void Move(Vector3 position, float speed)
    {
        _agent.speed = speed;
        _agent.SetDestination(position);
    }

    public void Stop()
    {
        _agent.isStopped = true;
        _isStopped.Value = true;
    }
    
    public void Resume()
    {
        _agent.isStopped = false;
        _isStopped.Value = false;
    }
}
