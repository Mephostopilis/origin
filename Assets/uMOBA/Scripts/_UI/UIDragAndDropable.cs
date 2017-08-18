// Drag and Drop support for UI elements. Drag and Drop actions will be sent to
// the GameObject with the 'handlerTag' tag.
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDragAndDropable : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
    // drag options
    [SerializeField] PointerEventData.InputButton button = PointerEventData.InputButton.Left;
    [SerializeField] GameObject drageePrefab;
    GameObject current; // currently dragged object

    // status
    public bool dragable = true;
    public bool dropable = true;

    [HideInInspector] public bool draggedToSlot = false;    
    
    public void OnBeginDrag(PointerEventData d) {
        // one mouse button is enough for dnd
        if (dragable && d.button == button) {
            // load current
            current = (GameObject)Instantiate(drageePrefab, transform.position, Quaternion.identity);
            current.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
            current.transform.SetParent(transform.root, true); // canvas
            current.transform.SetAsLastSibling(); // last one means foreground
        }
    }
    
    public void OnDrag(PointerEventData d) {
        // one mouse button is enough for dnd
        if (dragable && d.button == button) {
            // move current
            current.transform.position = d.position;
        }
    }
    
    // called after the slot's OnDrop
    public void OnEndDrag(PointerEventData d) {            
        // delete current in any case
        Destroy(current);

        // one mouse button is enough for dnd
        if (dragable && d.button == button) {
            // try destroy if not dragged to a slot (flag will be set by slot)
            if (!draggedToSlot) {
                // send a message to the DnD handler, it will take care of game
                // specific actions
                FindObjectOfType<PlayerDndHandling>().OnDragAndClear(tag, name.ToInt());
            }
            draggedToSlot = false; // rest flag
        }
    }

    // d.pointerDrag is the object that was dragged
    public void OnDrop(PointerEventData d) {
        // one mouse button is enough for dnd
        if (dropable && d.button == button) {
            // cache
            var drop = d.pointerDrag; // the dropped GameObject

            // was it a UIDragAndDropable?
            var dropDragable = drop.GetComponent<UIDragAndDropable>();
            if (dropDragable) {
                // let the dragable know that it was dropped onto a slot
                dropDragable.draggedToSlot = true;

                // only do something if we didn't drop something on itself
                // -> this way we don't have to ignore raycasts etc.
                if (dropDragable != this) {
                    // send a message to the DnD handler, it will take care of
                    // game specific actions
                    FindObjectOfType<PlayerDndHandling>().OnDragAndDrop(dropDragable.tag, dropDragable.name.ToInt(),
                                                                        tag, name.ToInt());
                }
            }
        }
    }

    void OnDisable() {
        Destroy(current);
    }

    void OnDestroy() {
        Destroy(current);
    }
}
