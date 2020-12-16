using UnityEngine;
namespace MiniFramework
{
    public partial class DebuggerComponent
    {
        private class SettingsWindow : IDebuggerWindow
        {
            public void Close()
            {

            }

            public void Initialize(params object[] args)
            {

            }

            public void OnDraw()
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("0.5x", GUILayout.Height(60f)))
                        {
                            DebuggerComponent.Instance.windowScale = 0.5f;
                        }
                        if (GUILayout.Button("1.0x", GUILayout.Height(60f)))
                        {
                            DebuggerComponent.Instance.windowScale = 1f;
                        }
                        if (GUILayout.Button("1.5x", GUILayout.Height(60f)))
                        {
                            DebuggerComponent.Instance.windowScale = 1.5f;
                        }
                        if (GUILayout.Button("2.0x", GUILayout.Height(60f)))
                        {
                            DebuggerComponent.Instance.windowScale = 2f;
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Reset Window Setting", GUILayout.Height(30f)))
                    {
                        DebuggerComponent.Instance.windowRect = DefaultWindowRect;
                        DebuggerComponent.Instance.smallWindowRect = DefaultSmallWindowRect;
                    }

                    if (GUILayout.Button("Clear PlayerPrefs", GUILayout.Height(30f)))
                    {
                        PlayerPrefs.DeleteAll();
                    }
                }
                GUILayout.EndVertical();
            }

            public void OnUpdate(float time, float realTime)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}