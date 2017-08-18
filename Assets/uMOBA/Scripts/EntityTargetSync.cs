// [SyncVar] GameObject doesn't work, [SyncVar] NetworkIdentity works but can't
// be set to null without UNET bugs, so this class is used to serialize an
// Entity's target. We can't use Serialization in classes that already use
// SyncVars, hence why we need an extra class.
//
// We always serialize the entity's GameObject and then use GetComponent,
// because we can't directly serialize the Entity type.
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval=0)] // save bandwidth, only sync when dirty
public class EntityTargetSync : NetworkBehaviour {
    // last target
    Entity last;

    // find out if target changed on server
    [ServerCallback]
    void Update() {
        var target = GetComponent<Entity>().target;
        if (target != last) {
            SetDirtyBit(1);
            last = target;
        }
    }
    
    // server-side serialization
    //
    // I M P O R T A N T
    //
    // always read and write the same amount of bytes. never let any errors
    // happen. otherwise readstr/readbytes out of range bugs happen.
    public override bool OnSerialize(NetworkWriter writer, bool initialState) {
        var target = GetComponent<Entity>().target;
        if (target != null) {
            // write target GameObject
            writer.Write(target.gameObject);
        } else {
            // write null but of type GameObject
            GameObject empty = null; 
            writer.Write(empty);
        }
        return true;
    }

    // client-side deserialization
    //
    // I M P O R T A N T
    //
    // always read and write the same amount of bytes. never let any errors
    // happen. otherwise readstr/readbytes out of range bugs happen.
    public override void OnDeserialize(NetworkReader reader, bool initialState) {
        var go = reader.ReadGameObject();
        GetComponent<Entity>().target = go != null ? go.GetComponent<Entity>() : null;
    }
}
