using UnityEngine;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

public class Test : MonoBehaviour {

    [DllImport("test", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int fntest();

    // Use this for initialization
    void Start () {
        byte[] xx = Maria.Encrypt.Crypt.randomkey();
        Debug.Log(ASCIIEncoding.ASCII.GetString(xx));

        int r = fntest();
        Debug.Log(r);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
