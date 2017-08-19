// Automatically starts a dedicated server if running in headless mode.
// (because we can't click the button in headless mode)
using UnityEngine;
using UnityEngine.Networking;
using Utils = uMoba.Utils;

public class StartServerIfHeadless : MonoBehaviour {
    void Start() {        
        if (Utils.IsHeadless()) {
            print("headless mode detected, starting dedicated server");
            GetComponent<NetworkManager>().StartServer();
        }
    }
}
