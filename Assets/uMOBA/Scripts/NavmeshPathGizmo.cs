// Draws the agent's path as Gizmo.
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NavmeshPathGizmo : MonoBehaviour {
    
    void OnDrawGizmos() {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        var path = agent.path;

        // color depends on status
        Color c = Color.white;
        switch (path.status) {
            case UnityEngine.AI.NavMeshPathStatus.PathComplete: c = Color.white; break;
            case UnityEngine.AI.NavMeshPathStatus.PathInvalid: c = Color.red; break;
            case UnityEngine.AI.NavMeshPathStatus.PathPartial: c = Color.yellow; break;
        }

        // draw the path
        for (int i = 1; i < path.corners.Length; ++i)
            Debug.DrawLine(path.corners[i-1], path.corners[i], c);
    }
}