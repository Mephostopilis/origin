using UnityEngine;

public class SetNavMeshPathfindingIterationsPerFrame : MonoBehaviour {
    [SerializeField] int iterations = 100; // default

    void Awake() {
        print("Setting NavMesh Pathfinding Iterations Per Frame from " + UnityEngine.AI.NavMesh.pathfindingIterationsPerFrame + " to " + iterations);
        UnityEngine.AI.NavMesh.pathfindingIterationsPerFrame = iterations;
    }
}
