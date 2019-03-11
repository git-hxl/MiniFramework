using System;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public partial class DebuggerComponent
    {
        private class ConsoleWindow : IDebuggerWindow
        {
            private int maxLine = 100;

            private int logCount = 0;
            private int warningCount = 0;
            private int errorCount = 0;
            private int exceptionCount = 0;

            private string dateTimeFormat = "[HH:mm:ss.fff]";
            private Queue<LogNode> logNodes = new Queue<LogNode>();
            private LogNode selectedNode = null;

            private bool lockScroll = true;

            private bool logFilter = true;
            private bool warningFilter = true;
            private bool errorFilter = true;
            private bool exceptionFilter = true;

            private Color32 logColor = Color.white;
            private Color32 warningColor = Color.yellow;
            private Color32 errorColor = Color.red;
            private Color32 exceptionColor = Color.magenta;

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
                        if (GUILayout.Button("Clear All"))
                        {
                            Clear();
                        }
                        lockScroll = GUILayout.Toggle(lockScroll, "Lock Scroll");
                        GUILayout.FlexibleSpace();
                        logFilter = GUILayout.Toggle(logFilter, "Info (" + logCount + ")");
                        warningFilter = GUILayout.Toggle(warningFilter, "Warning (" + warningCount + ")");
                        errorFilter = GUILayout.Toggle(errorFilter, "Error (" + errorCount + ")");
                        exceptionFilter = GUILayout.Toggle(exceptionFilter, "Exception (" + exceptionCount + ")");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginVertical("box");
                    {
                        if (lockScroll)
                        {
                            logScrollPosition.y = float.MaxValue;
                        }
                        logScrollPosition = GUILayout.BeginScrollView(logScrollPosition);
                        {
                            foreach (LogNode logNode in logNodes)
                            {
                                switch (logNode.LogType)
                                {
                                    case LogType.Log:
                                        if (!logFilter)
                                        {
                                            continue;
                                        }
                                        break;
                                    case LogType.Warning:
                                        if (!warningFilter)
                                        {
                                            continue;
                                        }
                                        break;
                                    case LogType.Error:
                                        if (!errorFilter)
                                        {
                                            continue;
                                        }
                                        break;
                                    case LogType.Exception:
                                        if (!exceptionFilter)
                                        {
                                            continue;
                                        }
                                        break;
                                }
                                if (GUILayout.Toggle(selectedNode == logNode, GetLogString(logNode)))
                                {
                                    if (selectedNode != logNode)
                                    {
                                        selectedNode = logNode;
                                        stackTraceScrollPosition = Vector2.zero;
                                    }
                                }
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical("box");
                    {
                        stackTraceScrollPosition = GUILayout.BeginScrollView(stackTraceScrollPosition, GUILayout.Height(DebuggerComponent.Instance.windowRect.height / 6));
                        {
                            if (selectedNode != null)
                            {
                                GUILayout.BeginHorizontal();

                                GUILayout.Label(selectedNode.LogMsg);
                                if (GUILayout.Button("Copy", GUILayout.Width(60f), GUILayout.Height(30f)))
                                {
                                    TextEditor textEditor = new TextEditor
                                    {
                                        text = selectedNode.LogMsg + "\n" + selectedNode.StackTrace
                                    };
                                    textEditor.OnFocus();
                                    textEditor.Copy();
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.Label(selectedNode.StackTrace);
                            }
                            GUILayout.EndScrollView();
                        }
                    }
                    GUILayout.EndVertical();
                }

            }
            public void Close()
            {
                Application.logMessageReceivedThreaded -= OnLogMessageReceived;
                Clear();
            }

            private void OnLogMessageReceived(string logMsg, string stackTrace, LogType logtype)
            {
                lock (locker)
                {
                    if (logtype == LogType.Assert)
                    {
                        logtype = LogType.Error;
                    }
                    LogNode log = Pool<LogNode>.Instance.Allocate().Fill(logtype, logMsg, stackTrace);
                    logNodes.Enqueue(log);
                    while (logNodes.Count > maxLine)
                    {
                        if (selectedNode == logNodes.Peek())
                        {
                            selectedNode = null;
                        }
                        Pool<LogNode>.Instance.Recycle(logNodes.Dequeue());
                    }
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
                    case LogType.Exception: color = exceptionColor; break;
                }
                return color;
            }
            private void RefreshCount()
            {
                logCount = 0;
                warningCount = 0;
                errorCount = 0;
                exceptionCount = 0;

                foreach (LogNode logNode in logNodes)
                {
                    switch (logNode.LogType)
                    {
                        case LogType.Log: logCount++; break;
                        case LogType.Warning: warningCount++; break;
                        case LogType.Error: errorCount++; break;
                        case LogType.Exception: exceptionCount++; break;
                    }
                }
            }
            private void Clear()
            {
                logNodes.Clear();
            }
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        private class LogNode : IPoolable
        {
            public DateTime LogTime;
            public LogType LogType;
            public string LogMsg;
            public string StackTrace;

            public bool IsRecycled { get; set; }

            public LogNode Fill(LogType logType, string logMsg, string stackTrace)
            {
                LogTime = DateTime.Now;
                LogType = logType;
                LogMsg = logMsg;
                StackTrace = stackTrace;
                return this;
            }

            public void Clear()
            {
                LogTime = default(DateTime);
                LogType = default(LogType);
                LogMsg = default(string);
                StackTrace = default(string);
            }

            public void OnRecycled()
            {
                Clear();
            }
        }
    }
}
