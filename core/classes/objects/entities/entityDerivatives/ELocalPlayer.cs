using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;
using Engine2.Physics;

namespace Engine2.Entities
{
    public class ELocalPlayer : EPawn
    {
        bool debugMovement = false;
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
            if(debugMovement)
            {
                Velocity.x = float.Clamp(Velocity.x + LR * maxWalkSpeed, -maxWalkSpeed * 2, maxWalkSpeed * 2);
                Velocity.y = float.Clamp(Velocity.y + UD * maxWalkSpeed, -maxWalkSpeed * 2, maxWalkSpeed * 2);
                if (LR == 0.0 && UD == 0.0)
                {
                    Velocity = FVector.Zero;
                }
                return;
            }
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
#if !RELEASE
            if (keyVal == Keys.G)
            {
                debugMovement = !debugMovement;
                Colliding = !debugMovement;
            }
#endif
        }
    }
}
