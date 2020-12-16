using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public partial class DebuggerComponent
    {
        private class FPSCounter
        {
            private const float calcRate = 0.5f;
            private int frameCount = 0;
            private float rateDuration = 0f;
            private float fps = 0;
            public float CurFps
            {
                get
                {
                    return fps;
                }
            }
            public void Update()
            {
                frameCount++;
                rateDuration += Time.unscaledDeltaTime;
                if (rateDuration > calcRate)
                {
                    fps = frameCount / rateDuration;
                    frameCount = 0;
                    rateDuration = 0f;
                }
            }
        }
    }
}