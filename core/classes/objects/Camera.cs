using Engine2.DataStructures;
using Engine2.Entities;

namespace Engine2.Object
{
    public class Camera : EObject
    {
        public EEntity followTarget;
        public FVector position;
        public float zoom = 1.0f;
        public RectangleF cachedBounds;

        public Camera()
        {
            Program.getEngine().ResizeEnd += MainWindowHasResized;
            CalcBounds();
        }

        private void MainWindowHasResized(object? sender, EventArgs e)
        {
            CalcBounds();
        }

        private void CalcBounds()
        {
            cachedBounds = RectangleF.FromLTRB(0, 0, Program.getEngine().Width, Program.getEngine().Height);
        }

        public override void UpdateObject()
        {
            if(followTarget != null)
            {
                position = followTarget.Transform.Translation;
            }
        }

        public RectangleF GetBounds()
        {
            return cachedBounds;
        }

        public void SetTarget(EEntity target)
        {
            followTarget = target;
        }

        public bool IsRectInView(FVector coord, FVector scale)
        {
            return coord.x > cachedBounds.Left 
                && coord.x + scale.x < cachedBounds.Right 
                && coord.y > cachedBounds.Top 
                && coord.y + 32 + scale.y < cachedBounds.Bottom;
        }
    }
}
