using Engine2.core.interfaces;

namespace Engine2.Object
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
