// Clients need to know the server time for cooldown calculations etc.
// Synchronizing the server time every second or so wouldn't be very precise, so
// we calculate an offset that can be added to the client's time in order to
// calculate the server time.
//
// The component should be attached to a NetworkTime GameObject that is always
// in the scene and that has no duplicates.
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval=60)] // resynchronize every now and then
public class NetworkTime : NetworkBehaviour {
    // add offset to Time.time to get the server time
    public static float offset;

    // server-side serialization
    //
    // I M P O R T A N T
    //
    // always read and write the same amount of bytes. never let any errors
    // happen. otherwise readstr/readbytes out of range bugs happen.
    public override bool OnSerialize(NetworkWriter writer, bool initialState) {
        writer.Write(Time.time);
        return true;
    }

    // client-side deserialization
    //
    // I M P O R T A N T
    //
    // always read and write the same amount of bytes. never let any errors
    // happen. otherwise readstr/readbytes out of range bugs happen.
    public override void OnDeserialize(NetworkReader reader, bool initialState) {
        offset = reader.ReadSingle() - Time.time;
    }
}
