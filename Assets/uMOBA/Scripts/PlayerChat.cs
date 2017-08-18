// We implemented a chat system that works directly with UNET. The chat supports
// different channels that can be used to communicate with other players:
// 
// - **Team Chat:** by default, all messages that don't start with a **/** are
// addressed to the team.
// - **Whisper Chat:** a player can write a private message to another player by
// using the **/ name message** format.
// - **All Chat:** we implemented all chat support with the **/all message**
// command.
// - **Info Chat:** the info chat can be used by the server to notify all
// players about important news. The clients won't be able to write any info
// messages.
// 
// _Note: the channel names, colors and commands can be edited in the Inspector
// by selecting the Player prefab and taking a look at the PlayerChat
// component._
// 
// A player can also click on a chat message in order to reply to it.
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ChannelInfo {
    public string command; // /w etc.
    public string identifierOut; // for sending
    public string identifierIn; // for receiving
    public Color color;

    public ChannelInfo(string _command, string _identifierOut, string _identifierIn, Color _color) {
        command = _command;
        identifierOut = _identifierOut;
        identifierIn = _identifierIn;
        color = _color;
    }
}

[System.Serializable]
public class MessageInfo {
    public string content; // the actual message
    public string replyPrefix; // copied to input when clicking the message
    public Color color;

    public MessageInfo(string sender, string identifier, string message, string _replyPrefix, Color _color) {
        // construct the message (we don't really need to save all the parts,
        // also this will save future computations)
        content = "<b>" + sender + identifier + ":</b> " + message;
        replyPrefix = _replyPrefix;
        color = _color;
    }
}

public class PlayerChat : NetworkBehaviour {
    // channels
    [Header("Channels")]
    [SerializeField] ChannelInfo chanWhisper = new ChannelInfo("/w", "(TO)", "(FROM)", Color.magenta);
    [SerializeField] ChannelInfo chanTeam = new ChannelInfo("", "(Team)", "(Team)", Color.cyan);
    [SerializeField] ChannelInfo chanAll = new ChannelInfo("/all", "(All)", "(All)", Color.white);
    [SerializeField] ChannelInfo chanInfo = new ChannelInfo("", "(Info)", "(Info)", Color.red);

    [Header("Other")]
    public int maxLength = 70;

    [Client]
    public override void OnStartLocalPlayer() {
        // test messages
        AddMessage(new MessageInfo("", chanInfo.identifierIn, "Type here for team chat!", "", chanInfo.color));
        AddMessage(new MessageInfo("", chanInfo.identifierIn, "  Use /all for all chat", "",  chanInfo.color));
        AddMessage(new MessageInfo("", chanInfo.identifierIn, "  Use /w NAME to whisper", "",  chanInfo.color));
        AddMessage(new MessageInfo("Someone", chanAll.identifierIn, "Hello Everyone!", "",  chanAll.color));

        // register message handlers
        NetworkManager.singleton.client.RegisterHandler(ChatWhisperFromMsg.MsgId, OnMsgWhisperFrom);
        NetworkManager.singleton.client.RegisterHandler(ChatWhisperToMsg.MsgId, OnMsgWhisperTo);
        NetworkManager.singleton.client.RegisterHandler(ChatInfoMsg.MsgId, OnMsgInfo);
        NetworkManager.singleton.client.RegisterHandler(ChatAllMsg.MsgId, OnMsgAll);
        NetworkManager.singleton.client.RegisterHandler(ChatTeamMsg.MsgId, OnMsgTeam);
    }

    // submit tries to send the string and then returns the new input text
    [Client]
    public string OnSubmit(string s) {
        // not empty and not only spaces?
        if (!Utils.IsNullOrWhiteSpace(s)) {
            // command in the commands list?
            // note: we don't do 'break' so that one message could potentially
            //       be sent to multiple channels (see mmorpg local chat)
            var lastcommand = "";
            if (s.StartsWith(chanWhisper.command)) {
                // whisper
                var parsed = ParsePM(chanWhisper.command, s);
                var user = parsed[0];
                var msg = parsed[1];
                if (!Utils.IsNullOrWhiteSpace(user) && !Utils.IsNullOrWhiteSpace(msg)) {
                    if (user != name) {
                        lastcommand = chanWhisper.command + " " + user + " ";
                        CmdMsgWhisper(user, msg);
                    } else {
                        print("cant whisper to self");
                    }
                } else {
                    print("invalid whisper format: " + user + "/" + msg);
                }
            } else if (!s.StartsWith("/")) {
                // local chat is special: it has no command
                lastcommand = "";
                CmdMsgTeam(s);
            } else if (s.StartsWith(chanAll.command)) {
                // all
                var msg = ParseGeneral(chanAll.command, s);
                lastcommand = chanAll.command + " ";
                CmdMsgAll(msg);
            }

            // input text should be set to lastcommand
            return lastcommand;
        }

        // input text should be cleared
        return "";
    }

    [Client]
    void AddMessage(MessageInfo mi) {
        FindObjectOfType<UIChat>().AddMessage(mi);
    }

    // parse a message of form "/command message"
    static string ParseGeneral(string command, string msg) {
        if (msg.StartsWith(command + " "))
            // remove the "/command " prefix
            return msg.Substring(command.Length + 1); // command + space
        return "";
    }

    static string[] ParsePM(string command, string pm) {
        // parse to /w content
        var content = ParseGeneral(command, pm);

        // now split the content in "user msg"
        if (content != "") {
            // find the first space that separates the name and the message
            var i = content.IndexOf(" ");
            if (i >= 0) {
                var user = content.Substring(0, i);
                var msg = content.Substring(i+1);
                return new string[] {user, msg};
            }
        }
        return new string[] {"", ""};
    }

    // networking //////////////////////////////////////////////////////////////
    [Command]
    void CmdMsgAll(string message) {
        if (message.Length > maxLength) return;

        print("sending all msg:" + message);

        // send to each player
        foreach(var entry in NetworkServer.objects) {
            if (entry.Value.GetComponent<PlayerChat>() != null) {
                // send message
                print("sending all msg to:" + entry.Value.name);
                var msg = new ChatAllMsg();
                msg.sender = name;
                msg.text = message;
                entry.Value.GetComponent<NetworkIdentity>().connectionToClient.Send(ChatAllMsg.MsgId, msg);
            }
        }
    }

    [Command]
    void CmdMsgTeam(string message) {
        if (message.Length > maxLength) return;

        print("sending team msg:" + message);

        // send to each player in the same team
        foreach(var entry in NetworkServer.objects) {
            if (entry.Value.GetComponent<PlayerChat>() != null &&
                entry.Value.GetComponent<Player>().team == GetComponent<Player>().team) {
                print("sending team msg to:" + entry.Value.name);
                // send message
                var msg = new ChatTeamMsg();
                msg.sender = name;
                msg.text = message;
                entry.Value.GetComponent<NetworkIdentity>().connectionToClient.Send(ChatTeamMsg.MsgId, msg);
            }
        }
    }

    [Command]
    void CmdMsgWhisper(string playerName, string message) {
        if (message.Length > maxLength) return;

        // find the player with that name (note: linq version is too ugly)
        foreach(var entry in NetworkServer.objects) {
            if (entry.Value.name == playerName && entry.Value.GetComponent<PlayerChat>() != null) {
                // receiver gets a 'from' message
                var msgF = new ChatWhisperFromMsg();
                msgF.sender = name;
                msgF.text = message;
                entry.Value.GetComponent<NetworkIdentity>().connectionToClient.Send(ChatWhisperFromMsg.MsgId, msgF);
                
                // sender gets a 'to' message
                var msgT = new ChatWhisperToMsg();
                msgT.receiver = entry.Value.name;
                msgT.text = message;
                GetComponent<NetworkIdentity>().connectionToClient.Send(ChatWhisperToMsg.MsgId, msgT);
                
                return;
            }
        }
    }

    // message handlers ////////////////////////////////////////////////////////
    // note: we can't use ClientRpc because that would send messages to everyone
    [Client]
    void OnMsgWhisperFrom(NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<ChatWhisperFromMsg>();
        // add message with identifierIn
        string identifier = chanWhisper.identifierIn;
        string reply = chanWhisper.command + " " + msg.sender + " "; // whisper
        AddMessage(new MessageInfo(msg.sender, identifier, msg.text, reply, chanWhisper.color));
    }

    [Client]
    void OnMsgWhisperTo(NetworkMessage netMsg) {
        print("OnMsgWhisperTo");
        var msg = netMsg.ReadMessage<ChatWhisperToMsg>();
        // add message with identifierOut
        string identifier = chanWhisper.identifierOut;
        string reply = chanWhisper.command + " " + msg.receiver + " "; // whisper
        AddMessage(new MessageInfo(msg.receiver, identifier, msg.text, reply, chanWhisper.color));
    }

    [Client]
    void OnMsgInfo(NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<ChatInfoMsg>();
        AddMessage(new MessageInfo("", chanInfo.identifierIn, msg.text, "", chanInfo.color));
    }

    [Client]
    void OnMsgAll(NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<ChatAllMsg>();
        AddMessage(new MessageInfo(msg.sender, chanAll.identifierIn, msg.text, "", chanAll.color));
    }

    [Client]
    void OnMsgTeam(NetworkMessage netMsg) {
        var msg = netMsg.ReadMessage<ChatTeamMsg>();
        AddMessage(new MessageInfo(msg.sender, chanTeam.identifierIn, msg.text, "", chanTeam.color));
    }
}
