using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.core.interfaces
{
    public interface IObjectRenderInterface
    {
        public void Render(Graphics g);
    }

    public interface IObjectInterface
    {
        public void UpdateObject();
    }
}
