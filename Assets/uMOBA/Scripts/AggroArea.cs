// Catches the Aggro Sphere's OnTrigger functions and forwards them to the
// Entity. Make sure that the aggro area's layer is IgnoreRaycast, so that
// clicking on the area won't cause the monster to be selected.
//
// Note that a player's collider might be on the pelvis for animation reasons,
// so we need to use GetComponentInParent to find the Player script.
using UnityEngine;

[RequireComponent(typeof(SphereCollider))] // aggro area trigger
public class AggroArea : MonoBehaviour {
    // cache
    Entity parent;

    // Use this for initialization
    void Awake () {
        parent = transform.parent.GetComponent<Entity>();
    }

    // same as OnTriggerStay
    void OnTriggerEnter(Collider co) {
        var entity = co.GetComponentInParent<Entity>();
        if (entity) parent.OnAggro(entity);
    }

    void OnTriggerStay(Collider co) {
        var entity = co.GetComponentInParent<Entity>();
        if (entity) parent.OnAggro(entity);
    }
}
