using Engine2.DataStructures;
using System.Diagnostics;

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
    }
}
