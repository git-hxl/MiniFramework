using System;
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
            private int exceptionCount = 0;

            private string dateTimeFormat = "[HH:mm:ss.fff]";
            private Queue<LogNode> logNodes = new Queue<LogNode>();
            private LogNode selectedNode = null;

            private bool lockScroll = true;
            private bool logFilter = true;
            private bool warningFilter = true;
            private bool errorFilter = true;
            private bool exceptionFilter = true;

            private int logHeight = 30;
            private int contentHeight = 360;
            private Color32 logColor = Color.white;
            private Color32 warningColor = Color.yellow;
            private Color32 errorColor = Color.red;
            private Color32 exceptionColor = Color.magenta;

            private Vector2 logScrollPosition = Vector2.zero;
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
                        if (GUILayout.Button("Clear All", GUILayout.Height(30f)))
                        {
                            Clear();
                        }
                        lockScroll = GUILayout.Toggle(lockScroll, "Lock Scroll", GUILayout.Height(30f));
                        GUILayout.FlexibleSpace();
                        logFilter = GUILayout.Toggle(logFilter, "Info (" + logCount + ")", GUILayout.Height(30f));
                        warningFilter = GUILayout.Toggle(warningFilter, "Warning (" + warningCount + ")", GUILayout.Height(30f));
                        errorFilter = GUILayout.Toggle(errorFilter, "Error (" + errorCount + ")", GUILayout.Height(30f));
                        exceptionFilter = GUILayout.Toggle(exceptionFilter, "Exception (" + exceptionCount + ")", GUILayout.Height(30f));
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical("box");
                    {
                        if (lockScroll)
                        {
                            logScrollPosition.y = logNodes.Count * logHeight;
                        }
                        logScrollPosition = GUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(contentHeight));
                        {
                            int[] logs = GetDrawLogs(logScrollPosition);
                            for (int i = 0; i < logs[0]; i++)
                            {
                                GUILayout.Space(logHeight);
                            }
                            for (int i = logs[0]; i <= logs[logs.Length - 1]; i++)
                            {
                                LogNode[] logsArray = logNodes.ToArray();
                                if (i >= logsArray.Length)
                                {
                                    break;
                                }
                                LogNode logNode = logsArray[i];

                                if (GUILayout.Toggle(selectedNode == logNode, GetLogString(logNode), GUILayout.Height(logHeight)))
                                {
                                    if (selectedNode != logNode)
                                    {
                                        selectedNode = logNode;
                                        lockScroll = false;
                                    }
                                }
                            }
                            for (int i = logs[logs.Length - 1] + 1; i < logNodes.Count; i++)
                            {
                                GUILayout.Space(logHeight);
                            }

                            // foreach (var logNode in logNodes)
                            // {
                            //     if (((IList)logs).Contains(index))
                            //     {
                            //         switch (logNode.LogType)
                            //         {
                            //             case LogType.Log:
                            //                 if (!logFilter)
                            //                 {
                            //                     continue;
                            //                 }
                            //                 break;
                            //             case LogType.Warning:
                            //                 if (!warningFilter)
                            //                 {
                            //                     continue;
                            //                 }
                            //                 break;
                            //             case LogType.Error:
                            //                 if (!errorFilter)
                            //                 {
                            //                     continue;
                            //                 }
                            //                 break;
                            //             case LogType.Exception:
                            //                 if (!exceptionFilter)
                            //                 {
                            //                     continue;
                            //                 }
                            //                 break;
                            //         }
                            //         if (GUILayout.Toggle(selectedNode == logNode, GetLogString(logNode), GUILayout.Height(logHeight)))
                            //         {
                            //             if (selectedNode != logNode)
                            //             {
                            //                 selectedNode = logNode;
                            //                 lockScroll = false;
                            //             }
                            //         }
                            //     }
                            //     else
                            //     {
                            //         GUILayout.Space(logHeight);
                            //     }
                            //     index++;
                            // }
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical("box");
                    {
                        if (selectedNode != null)
                        {
                            GUILayout.Label(selectedNode.LogMsg);
                            GUILayout.Label(selectedNode.StackTrace);
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            int[] GetDrawLogs(Vector2 scorllPos)
            {
                int maxLine = contentHeight / logHeight;
                int logIndex = (int)scorllPos.y / logHeight;
                if (logIndex >= logNodes.Count)
                {
                    logIndex = logNodes.Count - maxLine;
                }
                if (logIndex < 0)
                {
                    logIndex = 0;
                }
                int[] drawLogs = new int[maxLine];

                for (int i = 0; i < maxLine; i++)
                {
                    drawLogs[i] = i + logIndex;
                }
                return drawLogs;
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
