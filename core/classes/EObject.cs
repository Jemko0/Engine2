using Engine2.core.interfaces;

namespace Engine2.Object
{
    /// <summary>
    /// Base Object, self registers but does not render, rendering objects are in <see cref="ERenderableObject"/>
    /// </summary>
    public class EObject : IObjectInterface
    {
        public int objectID;
        public EObject()
        {
            objectID = ObjectManager.RegisterObject(this);
        }

        public virtual void UpdateObject()
        {
        }
    }
}
