using Engine2.DataStructures;

namespace Engine2.core.classes
{
    public static class Frame
    {
        private static DateTime start;
        private static DateTime end;
        public static float deltaTime;
        public static float fps;
        public static void StartDeltaCapture()
        {
            start = DateTime.Now;
        }

        public static DeltaCaptureResult EndDeltaCapture()
        {
            end = DateTime.Now;
            DeltaCaptureResult result = new DeltaCaptureResult();
            result.Delta = (float)((TimeSpan)(end - start)).TotalSeconds;
            result.FPS = (int)(1 / result.Delta);
            deltaTime = result.Delta;
            fps = result.FPS;
            return result;
        }
    }
}
