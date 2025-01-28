using Engine2.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.core.interfaces
{
    public interface IInputInterface
    {
        public void AxisInput(string id, FVector axisVal);
        public void KeyInput(Keys keyVal);
    }
}
