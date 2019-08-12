using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public partial class DebuggerComponent
    {
        private class ConsoleWindow : IDebuggerWindow
        {
            private int logCount = 0;
            private int warningCount = 0;
            private int errorCount = 0;
            private string dateTimeFormat = "[HH:mm:ss.fff]";
            private Queue<LogNode> logNodes = new Queue<LogNode>();
            private LogNode selectedNode = null;

            private bool lockScroll = true;
            private bool logFilter = true;
            private bool warningFilter = true;
            private bool errorFilter = true;

            private int logHeight = 30;
            private Color32 logColor = Color.white;
            private Color32 warningColor = Color.yellow;
            private Color32 errorColor = Color.red;
            private Vector2 logScrollPosition = Vector2.zero;
            private Vector2 stackTraceScrollPosition = Vector2.zero;
            private readonly static object locker = new object();
            public void Initialize(params object[] args)
            {
                Application.logMessageReceivedThreaded += OnLogMessageReceived;
            }
            public void OnUpdate(float time, float realTime) { }
            public void OnDraw()
            {
                lock (locker)
                {
                    RefreshCount();
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Clear All", GUILayout.Height(30f),GUILayout.Width(100f)))
                        {
                            Clear();
                        }
                        lockScroll = GUILayout.Toggle(lockScroll, "Lock Scroll", GUILayout.Height(30f));
                        GUILayout.FlexibleSpace();
                        logFilter = GUILayout.Toggle(logFilter, "Info (" + logCount + ")", GUILayout.Height(30f));
                        warningFilter = GUILayout.Toggle(warningFilter, "Warning (" + warningCount + ")", GUILayout.Height(30f));
                        errorFilter = GUILayout.Toggle(errorFilter, "Error (" + errorCount + ")", GUILayout.Height(30f));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical("box");
                    {
                        if (lockScroll)
                        {
                            logScrollPosition.y = logNodes.Count * logHeight;
                        }
                        logScrollPosition = GUILayout.BeginScrollView(logScrollPosition);
                        {
                            int maxLine = 20;
                            if (maxLine > logNodes.Count)
                            {
                                maxLine = logNodes.Count;
                            }
                            int logIndex = (int)logScrollPosition.y / logHeight;
                            logIndex = Mathf.Clamp(logIndex, 0, logNodes.Count - maxLine);

                            for (int i = 0; i < logIndex; i++)
                            {
                                GUILayout.Space(logHeight);
                            }
                            for (int i = logIndex; i < logIndex + maxLine; i++)
                            {
                                LogNode[] logsArray = logNodes.ToArray();
                                LogNode logNode = logsArray[i];
                                if (GUILayout.Toggle(selectedNode == logNode, GetLogString(logNode),GUILayout.Height(logHeight)))
                                {
                                    if (selectedNode != logNode)
                                    {
                                        selectedNode = logNode;
                                        lockScroll = false;
                                        stackTraceScrollPosition = Vector2.zero;
                                    }
                                }
                            }
                            for (int i = logIndex + maxLine; i < logNodes.Count; i++)
                            {
                                GUILayout.Space(logHeight);
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();

                    if (selectedNode != null)
                    {
                        GUILayout.BeginVertical("box",GUILayout.Height(100));
                        stackTraceScrollPosition = GUILayout.BeginScrollView(stackTraceScrollPosition);
                        GUILayout.Label(selectedNode.LogMsg + "\n" + selectedNode.StackTrace);
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    }
                }
            }
            public void Close()
            {
                Application.logMessageReceivedThreaded -= OnLogMessageReceived;
                Clear();
            }
            private void OnLogMessageReceived(string logMsg, string stackTrace, LogType logtype)
            {
                if (logtype == LogType.Assert || logtype == LogType.Exception)
                {
                    logtype = LogType.Error;
                }
                if (!logFilter && logtype == LogType.Log)
                {
                    return;
                }
                if (!warningFilter && logtype == LogType.Warning)
                {
                    return;
                }
                if (!errorFilter && logtype == LogType.Error)
                {
                    return;
                }
                lock (locker)
                {
                    LogNode log = new LogNode().Fill(logtype, logMsg, stackTrace);
                    logNodes.Enqueue(log);
                }
            }
            private string GetLogString(LogNode logNode)
            {
                Color32 color = GetLogStringColor(logNode.LogType);
                string hexColor = ColorUtility.ToHtmlStringRGB(color);
                return "<color=#" + hexColor + ">" + logNode.LogTime.ToString(dateTimeFormat) + logNode.LogMsg + "</color>";
            }
            private Color32 GetLogStringColor(LogType logType)
            {
                Color32 color = Color.white;
                switch (logType)
                {
                    case LogType.Log: color = logColor; break;
                    case LogType.Warning: color = warningColor; break;
                    case LogType.Error: color = errorColor; break;
                }
                return color;
            }
            private void RefreshCount()
            {
                logCount = 0;
                warningCount = 0;
                errorCount = 0;
                foreach (LogNode logNode in logNodes)
                {
                    switch (logNode.LogType)
                    {
                        case LogType.Log: logCount++; break;
                        case LogType.Warning: warningCount++; break;
                        case LogType.Error: errorCount++; break;
                    }
                }
            }
            private void Clear()
            {
                logNodes.Clear();
                selectedNode = null;
            }
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        private class LogNode
        {
            public DateTime LogTime;
            public LogType LogType;
            public string LogMsg;
            public string StackTrace;
            public LogNode Fill(LogType logType, string logMsg, string stackTrace)
            {
                LogTime = DateTime.Now;
                LogType = logType;
                LogMsg = logMsg;
                StackTrace = stackTrace;
                return this;
            }
        }
    }
}