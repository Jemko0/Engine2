using Engine2.core.classes.objects.rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.core.classes.objects.entities
{

    /// <summary>
    /// an Entity has a transform and renders itself as a rectangle by default
    /// </summary>
    public class EEntity : ERenderableObject
    {
        public FTransform Transform = new FTransform();
        public bool rendering = true;
        public EEntity() : base()
        {
            Transform.Translation = new FVector(10.0f, -10.0f);
            Transform.Scale = new FVector(100.0f, 100.0f);
        }
        public override void Render(Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            if(rendering)
            {
                FTransform RenderTransform;
                RenderTransform = URenderer.GetTransformed(Transform.Translation, 0, Transform.Scale);
                Rectangle rectEquivTransform = Rectangle.FromLTRB((int)RenderTransform.Translation.x, (int)RenderTransform.Translation.y, (int)RenderTransform.Translation.x + (int)RenderTransform.Scale.x, (int)RenderTransform.Translation.y + (int)RenderTransform.Scale.y);
                g.DrawRectangle(pen, rectEquivTransform);
            }
        }

        public override void UpdateObject()
        {
            base.UpdateObject();
        }
    }
}
