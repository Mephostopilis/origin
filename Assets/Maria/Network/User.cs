using UnityEngine;
using System.Collections;

public class User {
    public string Server { get; set; }
    public string Account { set;get;}
    public string Password { set;get;}
    public byte[] Uid { get; set; }
    public byte[] Secret { set;get;}
    public byte[] Subid {set;get;}
}
