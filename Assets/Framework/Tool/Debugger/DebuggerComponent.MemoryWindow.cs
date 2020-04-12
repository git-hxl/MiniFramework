using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace MiniFramework
{
    public partial class DebuggerComponent
    {

        private class MemoryWindow : IDebuggerWindow
        {
            private SampleWindow<UnityEngine.Object> objectWindow = new SampleWindow<UnityEngine.Object>();
            private SampleWindow<Texture> textureMemoryWindow = new SampleWindow<Texture>();
            private SampleWindow<Mesh> meshMemoryWindow = new SampleWindow<Mesh>();
            private SampleWindow<Material> materialMemoryWindow = new SampleWindow<Material>();
            private SampleWindow<AnimationClip> animationClipMemoryWindow = new SampleWindow<AnimationClip>();
            private SampleWindow<AudioClip> audioClipMemoryWindow = new SampleWindow<AudioClip>();
            private SampleWindow<Font> fontMemoryWindow = new SampleWindow<Font>();
            private SampleWindow<GameObject> gameObjectMemoryWindow = new SampleWindow<GameObject>();
            private SampleWindow<Component> componentMemoryWindow = new SampleWindow<Component>();

            private Dictionary<string, IDebuggerWindow> windows = new Dictionary<string, IDebuggerWindow>();

            private int curSelectedSampleIndex;
            public void Initialize(params object[] args)
            {
                windows.Add("All", objectWindow);
                windows.Add("纹理", textureMemoryWindow);
                windows.Add("网格", meshMemoryWindow);
                windows.Add("材质", materialMemoryWindow);
                windows.Add("动画", animationClipMemoryWindow);
                windows.Add("音频", audioClipMemoryWindow);
                windows.Add("字体", fontMemoryWindow);
                windows.Add("物体", gameObjectMemoryWindow);
                windows.Add("组件", componentMemoryWindow);
            }

            public void OnDraw()
            {
                List<string> names = new List<string>();
                foreach (var item in windows)
                {
                    names.Add(item.Key);
                }
                int toolbarIndex = GUILayout.Toolbar(curSelectedSampleIndex, names.ToArray(), GUILayout.Height(30f));
                curSelectedSampleIndex = toolbarIndex;
                windows[names[curSelectedSampleIndex]].OnDraw();
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
        private class SampleWindow<T> : IDebuggerWindow where T : UnityEngine.Object
        {
            private int showSampleCount = 300;

            private DateTime sampleTime = DateTime.MinValue;
            private long sampleSize = 0;
            private long duplicateSampleSize = 0;
            private int duplicateSampleCount = 0;

            private List<Sample> samples = new List<Sample>();

            private Vector2 m_ScrollPosition = Vector2.zero;
            public void Close()
            {
                //throw new NotImplementedException();
            }

            public void Initialize(params object[] args)
            {
                //throw new NotImplementedException();
            }

            public void OnDraw()
            {

                string typeName = typeof(T).Name;
                //GUILayout.Label("<b>" + typeName + " Runtime Memory Information</b>");
                GUILayout.BeginVertical("box");
                {
                    m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                    {
                        if (GUILayout.Button("Take Sample for " + typeName, GUILayout.Height(30f)))
                        {
                            TakeSample();
                        }
                        if (sampleTime <= DateTime.MinValue)
                        {
                            GUILayout.Label("<b>Please take sample for " + typeName + " first</b>");
                        }
                        else
                        {
                            if (duplicateSampleCount > 0)
                            {
                                GUILayout.Label("<b>" + samples.Count.ToString() + " " + typeName + "s(" + UnitConvert.ByteConvert(sampleSize) + ") obtained at " + sampleTime.ToString("yyyy-MM-dd HH:mm:ss") + ", while " + duplicateSampleCount.ToString() + " " + typeName + "s (" + UnitConvert.ByteConvert(duplicateSampleSize) + ") might be duplicated.</b>");
                            }
                            else

                            {
                                GUILayout.Label("<b>" + samples.Count.ToString() + " " + typeName + "s(" + UnitConvert.ByteConvert(sampleSize) + ") obtained at " + sampleTime.ToString("yyyy-MM-dd HH:mm:ss") + "</b>");
                            }
                            if (samples.Count > 0)
                            {
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label("<b>Name</b>");
                                    GUILayout.Label("<b>Type</b>", GUILayout.Width(240f));
                                    GUILayout.Label("<b>Size</b>", GUILayout.Width(80f));
                                }
                                GUILayout.EndHorizontal();
                            }
                            int count = 0;
                            for (int i = 0; i < samples.Count; i++)
                            {
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(samples[i].HighLight ? "<color=yellow>" + samples[i].Name + "</color>" : samples[i].Name);
                                    GUILayout.Label(samples[i].HighLight ? "<color=yellow>" + samples[i].Type + "</color>" : samples[i].Type, GUILayout.Width(240f));
                                    GUILayout.Label(samples[i].HighLight ? "<color=yellow>" + UnitConvert.ByteConvert(samples[i].Size) + "</color>" : UnitConvert.ByteConvert(samples[i].Size), GUILayout.Width(80f));
                                }
                                GUILayout.EndHorizontal();

                                count++;
                                if (count >= showSampleCount)
                                {
                                    break;
                                }
                            }
                        }
                        GUILayout.EndScrollView();
                    }
                }
                GUILayout.EndVertical();
            }

            public void OnUpdate(float time, float realTime)
            {
                //throw new NotImplementedException();
            }
            private void TakeSample()
            {
                sampleTime = DateTime.Now;
                sampleSize = 0L;
                duplicateSampleSize = 0L;
                duplicateSampleCount = 0;
                samples.Clear();

                T[] tempSamples = Resources.FindObjectsOfTypeAll<T>();
                for (int i = 0; i < tempSamples.Length; i++)
                {
                    long size = Profiler.GetRuntimeMemorySizeLong(tempSamples[i]);
                    sampleSize += size;
                    samples.Add(new Sample(tempSamples[i].name, tempSamples[i].GetType().Name, size));
                }
                samples.Sort(SampleComparer);
                for (int i = 1; i < samples.Count; i++)
                {
                    if (samples[i].Name == samples[i - 1].Name && samples[i].Type == samples[i - 1].Type && samples[i].Size == samples[i - 1].Size)
                    {
                        samples[i].HighLight = true;
                        duplicateSampleSize += samples[i].Size;
                        duplicateSampleCount++;
                    }
                }
            }
            private int SampleComparer(Sample a, Sample b)
            {
                int result = b.Size.CompareTo(a.Size);
                if (result != 0)
                {
                    return result;
                }

                result = a.Type.CompareTo(b.Type);
                if (result != 0)
                {
                    return result;
                }

                return a.Name.CompareTo(b.Name);
            }
        }
        private class Sample
        {
            public string Name;
            public string Type;
            public long Size;
            public bool HighLight;
            public Sample(string name, string type, long size)
            {
                Name = name;
                Type = type;
                Size = size;
                HighLight = false;
            }
        }
    }
}