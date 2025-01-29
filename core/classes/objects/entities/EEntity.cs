using Engine2.DataStructures;
using Engine2.core.classes.objects.rendering;
using Engine2.core.classes.objects;

namespace Engine2.Entities
{

    /// <summary>
    /// an Entity has a transform and renders itself as a rectangle by default
    /// </summary>
    public class EEntity : ERenderableObject
    {
        public FTransform Transform = new FTransform();
        public bool rendering = true;
        public SolidBrush brush = new SolidBrush(Color.Red);
        public EEntity() : base()
        {
            Transform.Translation = new FVector(0.0f, 0.0f);
            Transform.Scale = new FVector(100.0f, 100.0f);
        }

        public override void Render(Graphics g)
        {
            if(rendering)
            {
                FTransform RenderTransform;
                RenderTransform = URenderer.GetTransformed(Transform.Translation, 0, Transform.Scale);
                Rectangle rectEquivTransform = Rectangle.FromLTRB((int)RenderTransform.Translation.x, (int)RenderTransform.Translation.y, (int)RenderTransform.Translation.x + (int)RenderTransform.Scale.x, (int)RenderTransform.Translation.y + (int)RenderTransform.Scale.y);
                g.FillRectangle(brush, rectEquivTransform);
            }
        }

        public override void UpdateObject()
        {
            base.UpdateObject();
        }

        //basic

        public void SetLocation(FVector Location)
        {
            Transform.Translation = Location;
        }
    }
}
