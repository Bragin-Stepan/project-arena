using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentJumper
{
    private MonoBehaviour _ctx;
    private NavMeshAgent _agent;
    private float _speed;
    private AnimationCurve _yOffsetCurve;
    
    private Coroutine _jumpProcess;

    public AgentJumper(NavMeshAgent agent, float speed,AnimationCurve yOffsetCurve, MonoBehaviour ctx)
    {
        _ctx = ctx;
        _agent = agent;
        _speed = speed;
        _yOffsetCurve = yOffsetCurve;
    }
    
    public bool InProcess => _jumpProcess != null;

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (InProcess)
            return;
        
        _jumpProcess = _ctx.StartCoroutine(JumpProcess(offMeshLinkData));
    }
    
    private IEnumerator JumpProcess(OffMeshLinkData offMeshLinkData)
    {
        Vector3 startPosition = offMeshLinkData.startPos;
        Vector3 endPosition = offMeshLinkData.endPos;
    
        float duration = Vector3.Distance(startPosition, endPosition) / _speed;
        float progress = 0;
        
        while (progress < duration)
        { 
            float yOffset = _yOffsetCurve.Evaluate(progress / duration);
            
            _agent.transform.position = Vector3.Lerp(startPosition, endPosition, progress / duration) + Vector3.up * yOffset;
            progress += Time.deltaTime;
            yield return null;
        }
        
        _agent.CompleteOffMeshLink();
        _jumpProcess = null;
    }
}
