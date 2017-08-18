// UNET's current NetworkTransform is really laggy, so we make it smooth by
// simply synchronizing the agent's destination. We could also lerp between
// the transform positions, but this is much easier and saves lots of bandwidth.
//
// Using a NavMeshAgent also has the benefit that no rotation has to be synced
// while moving.
//
// Notes:
//
// - Teleportations have to be detected and synchronized properly
// - Caching the agent won't work because serialization sometimes happens
// before awake/start
// - We also need the stopping distance, otherwise entities move too far.
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[NetworkSettings(sendInterval=0)] // save bandwidth, only sync when dirty
public class NetworkNavMeshAgent : NetworkBehaviour {
    // last destination
    Vector3 last;

    // find out if destination changed on server
    [ServerCallback]
    void Update() {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent.destination != last) {            
            SetDirtyBit(1);
            last = agent.destination;
        }
    }

    // server-side serialization
    //
    // I M P O R T A N T
    //
    // always read and write the same amount of bytes. never let any errors
    // happen. otherwise readstr/readbytes out of range bugs happen.
    public override bool OnSerialize(NetworkWriter writer, bool initialState) {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // basics
        writer.Write(transform.position); // for teleport detection
        writer.Write(agent.destination);
        writer.Write(agent.speed);
        writer.Write(agent.stoppingDistance);

        // area costs (important for monster lane costs)
        // -> we can't get a name from an index, hence we can't filter out non-
        //    empty areas. we have to sync all of them all the time.
        // -> only syncing the current one isn't enough either if we want to
        //    calculate the same path on clients and servers
        // -> 32 because Window->Navigation->Areas has 32 entries
        for (int i = 0; i < 32; ++i) writer.Write(agent.GetAreaCost(i));

        return true;
    }

    // client-side deserialization
    //
    // I M P O R T A N T
    //
    // always read and write the same amount of bytes. never let any errors
    // happen. otherwise readstr/readbytes out of range bugs happen.
    public override void OnDeserialize(NetworkReader reader, bool initialState) {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // basics
        var pos = reader.ReadVector3();
        var dest = reader.ReadVector3();
        agent.speed = reader.ReadSingle();
        agent.stoppingDistance = reader.ReadSingle();

        // area costs (see OnSerialize comment)        
        for (int i = 0; i < 32; ++i) agent.SetAreaCost(i, reader.ReadSingle());

        // only try to set the destination if the agent is on a navmesh already
        // (it might not when falling from the sky after joining)
        if (agent.isOnNavMesh) {
            // detect teleports + teleport if too far behind
            // -> agent moves 'speed' meter per seconds
            // -> if we are 2 speed units behind, then we teleport
            //    (using speed is better than using a hardcoded value)
            if (Vector3.Distance(transform.position, pos) > agent.speed * 2)
                agent.Warp(pos);

            // set destination afterwards, so that we never stop going there
            // even after being warped etc.
            agent.destination = dest;
        }
    }
}
