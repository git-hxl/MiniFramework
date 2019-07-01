using UnityEngine;
using UnityEngine.Profiling;

namespace MiniFramework
{
    public partial class DebuggerComponent
    {
        private class InformationWindow : IDebuggerWindow
        {
            private const float MBSize = 1024 * 1024;
            private Vector2 logScrollPosition = Vector2.zero;
            public void Initialize(params object[] args)
            {
                //throw new System.NotImplementedException();
            }

            public void OnDraw()
            {
                GUILayout.Label("<b>Profiler Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Mono Used Size:", (Profiler.GetMonoUsedSizeLong() / (float)MBSize).ToString("F3") + " MB");
                    DrawItem("Mono Heap Size:", (Profiler.GetMonoHeapSizeLong() / (float)MBSize).ToString("F3") + " MB");
                    DrawItem("Total Allocated Memory:", (Profiler.GetTotalAllocatedMemoryLong() / (float)MBSize).ToString("F3") + " MB");
                    DrawItem("Total Unused Reserved Memory:", (Profiler.GetTotalUnusedReservedMemoryLong() / (float)MBSize).ToString("F3") + " MB");
                    DrawItem("Total Reserved Memory:", (Profiler.GetTotalReservedMemoryLong() / (float)MBSize).ToString("F3") + " MB");
                }
                GUILayout.EndVertical();

                GUILayout.Label("<b>Device Information</b>");
                GUILayout.BeginVertical("box");
                logScrollPosition = GUILayout.BeginScrollView(logScrollPosition);
                {
                    DrawItem("Device Name:", SystemInfo.deviceName);
                    DrawItem("Device Model:", SystemInfo.deviceModel);  
                    DrawItem("Processor Type:", SystemInfo.processorType);
                    DrawItem("Graphics Device:", SystemInfo.graphicsDeviceName);
                    DrawItem("Memory Size:", SystemInfo.systemMemorySize.ToString() + " MB");
                    DrawItem("Operating System:", SystemInfo.operatingSystem);
                    DrawItem("Device Unique ID:", SystemInfo.deviceUniqueIdentifier);

                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }

            private void DrawItem(string title, string content)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(title);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(content);
                }
                GUILayout.EndHorizontal();
            }

            public void OnUpdate(float time, float realTime)
            {
                //throw new System.NotImplementedException();
            }

            public void Close()
            {
                //throw new System.NotImplementedException();
            }
        }
    }
}