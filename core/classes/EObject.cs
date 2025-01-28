using Engine2.core.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.core.classes
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
