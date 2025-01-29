using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;

namespace Engine2.Entities
{
    internal static class TEST
    {

    }

    public class ELocalPlayer : EPawn
    {
        public ELocalPlayer()
        {
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
    }
}
