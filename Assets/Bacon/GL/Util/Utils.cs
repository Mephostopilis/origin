// This class contains some helper functions.
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

namespace Bacon.GL.Util { 
public class Utils {
    // Mathf.Max only works for float and int, not long
    public static long MaxLong(long a, long b) {
        return a > b ? a : b;
    }

    // Mathf.Min only works for float and int, not long
    public static long MinLong(long a, long b) {
        return a < b ? a : b;
    }

    // Mathf.Clamp only works for float and int, not long
    public static long ClampLong(long value, long min, long max) {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
    
    // generate a random vector on the x-z plane (y=0)
    public static Vector3 RandVec3XZ() {
        // note: '.f' is important so that Random.Range knows we want floats
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
    }

    // is any of the keys UP?
    public static bool AnyKeyUp(KeyCode[] keys) {
        foreach (KeyCode k in keys)
            if (Input.GetKeyUp(k)) return true;
        return false;
    }

    // is any of the keys DOWN?
    public static bool AnyKeyDown(KeyCode[] keys) {
        foreach (KeyCode k in keys)
            if (Input.GetKeyDown(k)) return true;
        return false;
    }

    // detect headless mode (which has graphicsDeviceType Null)
    public static bool IsHeadless() {
        return SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    }

    // String.IsNullOrWhiteSpace that exists in NET4.5
    // note: can't be an extension because then it can't detect null strings
    //       like null.IsNullorWhitespace
    public static bool IsNullOrWhiteSpace(string value) {
        return System.String.IsNullOrEmpty(value) || value.Trim().Length == 0;
    }

    // Distance between two ClosestPointOnBounds
    // this is needed in cases where entites are really big. in those cases,
    // we can't just move to entity.transform.position, because it will be
    // unreachable. instead we have to go the closest point on the boundary.
    //
    // Vector3.Distance(a.transform.position, b.transform.position):
    //    _____        _____
    //   |     |      |     |
    //   |  x==|======|==x  |
    //   |_____|      |_____|
    //
    //
    // Utils.ClosestDistance(a.collider, b.collider):
    //    _____        _____
    //   |     |      |     |
    //   |     |x====x|     |
    //   |_____|      |_____|
    //  
    public static float ClosestDistance(Collider a, Collider b) {
        var posA = a.transform.position;
        var posB = b.transform.position;
        return Vector3.Distance(a.ClosestPointOnBounds(posB),
                                b.ClosestPointOnBounds(posA));
    }

    // pretty print seconds as hours:minutes:seconds
    public static string PrettyTime(float seconds) {
        var t = System.TimeSpan.FromSeconds(seconds);
        var res = "";
        if (t.Days > 0) res += t.Days + "d";
        if (t.Hours > 0) res += " " + t.Hours + "h";
        if (t.Minutes > 0) res += " " + t.Minutes + "m";
        if (t.Seconds > 0) res += " " + t.Seconds + "s";
        // if the string is still empty because the value was '0', then at least
        // return the seconds instead of returning an empty string
        return res != "" ? res : "0s";
    }

    // hard mouse scrolling that is consistent between all platforms
    //   Input.GetAxis("Mouse ScrollWheel") and
    //   Input.GetAxisRaw("Mouse ScrollWheel")
    //   both return values like 0.01 on standalone and 0.5 on WebGL, which
    //   causes too fast zooming on WebGL etc.
    // normally GetAxisRaw should return -1,0,1, but it doesn't for scrolling
    public static float GetAxisRawScrollUniversal() {
        var scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll < 0) return -1f;
        if (scroll > 0) return 1f;
        return 0f;
    }

    // find local player (clientsided)
    public static Player ClientLocalPlayer() {
        // note: ClientScene.localPlayers.Count cant be used as check because
        // nothing is removed from that list, even after disconnect. It still
        // contains entries like: ID=0 NetworkIdentity NetID=null Player=null
        // (which might be a UNET bug)
        var p = ClientScene.localPlayers.Find(pc => pc.gameObject != null);
        return p != null ? p.gameObject.GetComponent<Player>() : null;
    }

    // check if the cursor is over a UI or OnGUI element right now
    // note: for UI, this only works if the UI's CanvasGroup blocks Raycasts
    // note: for OnGUI: hotControl is only set while clicking, not while zooming
    public static bool IsCursorOverUserInterface() {
        return EventSystem.current.IsPointerOverGameObject() ||
               GUIUtility.hotControl != 0;
    }
}
}