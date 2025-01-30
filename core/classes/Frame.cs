﻿using Engine2.DataStructures;
using System.Diagnostics;
using System.Xml.Linq;

namespace Engine2.core.classes
{
    public static class Frame
    {
        public static float deltaTime;
        public static double fps;
        static Stopwatch stopwatch = new Stopwatch();
        static long lastTime = stopwatch.ElapsedMilliseconds;
        public static void StartDeltaCapture()
        {
            stopwatch.Start();
        }

        public static DeltaCaptureResult EndDeltaCapture()
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            double tdeltaTime = (float)(currentTime - lastTime) / 1000; // Convert to seconds
            lastTime = currentTime;
            DeltaCaptureResult result = new DeltaCaptureResult();
            result.DeltaMS = (float)tdeltaTime;
            result.FPS = 1.0 / tdeltaTime;
            deltaTime = result.DeltaMS;
            fps = result.FPS;
            
            return result;
        }

        
        public static Dictionary<string, Stopwatch> captures = new Dictionary<string, Stopwatch>();
        static Dictionary<string, double> captureDeltas = new Dictionary<string, double>();
        static long captureLastTime;
        public static void StartCapture(string name)
        {
            if(!captures.ContainsKey(name))
            {
                Stopwatch sw;
                if (captures.ContainsKey(name))
                {
                    if(captures[name] == null)
                    {
                        sw = new Stopwatch();
                        captures[name] = sw;
                    }
                    
                }
                captures[name].Restart();

            }
        }

        public static double GetCapture(string name)
        {
            return captureDeltas[name];
        }

        public static void EndCapture(string name)
        {
            captures[name].Stop();
            long currentTime = captures[name].ElapsedMilliseconds;
            double tdeltaTime = (float)(currentTime - captureLastTime) / 1000;
            captureDeltas[name] = tdeltaTime;
        }
    }
}
