using System;
using Engine2.core.interfaces;
using Engine2.core.classes;
namespace Engine2.core.classes.objects.rendering
{
    public class Renderer : EObject
    {
        public Renderer() : base()
        {
        }

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
        /// RawAngle is not supported yet
        /// </summary>
        /// <param name="rawPos"></param>
        /// <param name="rawAngle"></param>
        /// <param name="rawScale"></param>
        /// <returns></returns>
        public static FTransform GetTransformed(FVector rawPos, float rawAngle, FVector rawScale)
        {
            FTransform fTransform = new FTransform();
            fTransform.Translation.x = rawPos.x - Engine.Camera.position.x;
            fTransform.Translation.y = Engine.Camera.position.y - rawPos.y;
            fTransform.Angle = rawAngle;
            fTransform.Scale.x = rawScale.x * Engine.Camera.zoom;
            fTransform.Scale.y = rawScale.y * Engine.Camera.zoom;

            return fTransform;
        }
    }
}
