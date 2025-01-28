using Engine2.core.interfaces;

namespace Engine2.core.classes.objects
{
    public class ERenderableObject : EObject, IObjectRenderInterface
    {
        public ERenderableObject() : base()
        {
        }

        public virtual void Render(Graphics g)
        {
            //pass
        }
    }
}
