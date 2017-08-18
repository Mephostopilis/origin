﻿// People should be able to see and report errors to the developer very easily.
//
// Unity's Developer Console only works in development builds and it only shows
// errors. This class provides a console that works in all builds and also shows
// log and warnings in development builds.
//
// Note: we don't include the stack trace, because that can also be grabbed from
// the log files if needed.
//
// Note: there is no 'hide' button because we DO want people to see those errors
// and report them back to us.
using UnityEngine;
using System.Collections.Generic;

class LogEntry {
    public string text;
    public LogType type;
    public LogEntry(string _text, LogType _type) {
        text = _text;
        type = _type;
    }
}

public class GUIConsole : MonoBehaviour {
    List<LogEntry> log = new List<LogEntry>();
    Vector2 scroll = Vector2.zero;

#if !UNITY_EDITOR
    void Awake() {
        Application.logMessageReceived += OnLog;
    }
#endif

    void OnLog(string text, string stackTrace, LogType type) {
        // show errors and exceptions in all cases, logs only in development
        if (type == LogType.Error || type == LogType.Exception || Debug.isDebugBuild) {
            log.Add(new LogEntry(text, type));
            scroll.y = 99999f; // autoscroll
        }
    }

    void OnGUI() {
        if (log.Count == 0) return;        
        scroll = GUILayout.BeginScrollView(scroll, "Box", GUILayout.Width(Screen.width), GUILayout.Height(25));
        foreach (var entry in log) {
            if (entry.type == LogType.Error || entry.type == LogType.Exception)
                GUI.color = Color.red;
            else if (entry.type == LogType.Warning)
                GUI.color = Color.yellow;
            GUILayout.Label(entry.text);
            GUI.color = Color.white;
        }
        GUILayout.EndScrollView();
    }
}
