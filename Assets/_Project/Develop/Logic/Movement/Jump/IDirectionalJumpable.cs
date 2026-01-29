using UnityEngine;
using UnityEngine.AI;

public interface IDirectionalJumpable
{
    bool InJumpProcess { get; }
    void Jump(OffMeshLinkData linkData);
    bool IsOnNavMeshLink(out OffMeshLinkData linkData);
}
