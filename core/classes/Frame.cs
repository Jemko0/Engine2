using Engine2.DataStructures;
using System.Diagnostics;

namespace Engine2.core.classes
{
    public static class Frame
    {
        public static float deltaTime;
        public static float fps;
        static Stopwatch stopwatch = new Stopwatch();
        static long lastTime = stopwatch.ElapsedMilliseconds;
        public static void StartDeltaCapture()
        {
            stopwatch.Start();
        }

        public static DeltaCaptureResult EndDeltaCapture()
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            float tdeltaTime = (float)(currentTime - lastTime) / 1000; // Convert to seconds
            lastTime = currentTime;
            DeltaCaptureResult result = new DeltaCaptureResult();
            result.DeltaMS = (float)tdeltaTime;

            deltaTime = result.DeltaMS;
            return result;
        }
    }
}
