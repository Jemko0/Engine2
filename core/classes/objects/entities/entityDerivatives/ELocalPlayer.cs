using Engine2.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Entities
{
    public class ELocalPlayer : ECharacter
    {
        public ELocalPlayer()
        {
            Transform.Scale = new FVector(20, 50);
        }

        public override void AxisInput(string id, FVector axisVal)
        {
            
        }

    }
}
