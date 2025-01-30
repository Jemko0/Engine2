using Engine2.core.interfaces;
using Engine2.DataStructures;
using Engine2.Object;

namespace Engine2.Rendering
{
    public class Renderer : EObject
    {
        public Renderer() : base()
        {
        }

        public bool useDPI = true;

        public static void RenderFrame(PaintEventArgs e)
        {
            for (int i = 0; i < ObjectManager.renderingObjects.Count; i++)
            {
                IObjectRenderInterface obj = (IObjectRenderInterface)ObjectManager.renderingObjects[i];
                obj.Render(e.Graphics);
            }
        }
    }

    public class URenderer
    {
        /// <summary>
        /// Takes in a raw Transform and converts it into a view space Transform, NECESSARY if you want to render anything on the screen that needs to be in the World (moving relative to the camera).
        /// RawAngle is not supported yet
        /// </summary>
        public static FTransform GetTransformed(FVector rawPos, float rawAngle, FVector rawScale)
        {
            Engine e = Program.getEngine();
            FTransform fTransform = new FTransform();
            fTransform.Translation.x = rawPos.x - e.Camera.position.x + e.Width / (2 * e.Camera.zoom);
            fTransform.Translation.y = e.Camera.position.y - rawPos.y + e.Height / (2 * e.Camera.zoom);
            fTransform.Angle = rawAngle;
            fTransform.Scale.x = rawScale.x * e.Camera.zoom;
            fTransform.Scale.y = rawScale.y * e.Camera.zoom;

            float dpiScale = 1.0f;
            //System.Diagnostics.Debug.Write("DPI: " + dpiScale);
            fTransform.ScaleTransform(dpiScale);

            return fTransform;
        }

        public static FTransform GetTransformedTile(int x, int y)
        {
            Engine e = Program.getEngine();
            float fx;
            float fy;
            fx = (float)(x * 32 - e.Camera.position.x + e.Width / (2 * e.Camera.zoom));
            fy = (float)(e.Camera.position.y - y * 32 + e.Height / (2 * e.Camera.zoom));

            return new FTransform(new FVector(fx * e.Camera.zoom, fy * e.Camera.zoom), new FVector(32 * e.Camera.zoom, 32 * e.Camera.zoom));
        }

        public static float GetRenderDPIScale()
        {
            float w = Program.getEngine().Width;
            float h = Program.getEngine().Height;
            return w < h ? w / 720.0f : h / 480.0f;
        }
    }
}
