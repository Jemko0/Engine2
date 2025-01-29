using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;
using Engine2.Physics;

namespace Engine2.Entities
{
    public class ELocalPlayer : EPawn
    {
        public ELocalPlayer()
        {
            Transform.Translation = new FVector(0, 100);
            Transform.Scale = new FVector(20, 50);
            brush = new SolidBrush(Color.Blue);
        }

        public override void UpdateObject()
        {
            Movement();
            base.UpdateObject();
        }

        public override void Movement()
        {
            LR = GlobalLookup.KeyMappings.GetAxis("axisMoveLR");
            UD = GlobalLookup.KeyMappings.GetAxis("axisMoveUD");
            base.Movement();
        }

        public override void KeyInput(Keys keyVal)
        {
            if (keyVal == Keys.Space)
            {
                if(lastSweep.Collision)
                {
                    Jump();
                }
            }
        }
    }
}
