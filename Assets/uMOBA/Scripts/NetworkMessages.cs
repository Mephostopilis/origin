// Contains all the network messages that we need.
using UnityEngine;
using UnityEngine.Networking;

// client to server ////////////////////////////////////////////////////////////

// server to client ////////////////////////////////////////////////////////////
class ChatWhisperFromMsg : MessageBase {
    public static short MsgId = 2002;
    public string sender;
    public string text;
}

class ChatWhisperToMsg : MessageBase {
    public static short MsgId = 2003;
    public string receiver;
    public string text;
}

class ChatInfoMsg : MessageBase {
    public static short MsgId = 2004;
    public string text;
}

class ChatAllMsg : MessageBase {
    public static short MsgId = 2005;
    public string sender;
    public string text;
}

class ChatTeamMsg : MessageBase {
    public static short MsgId = 2006;
    public string sender;
    public string text;
}