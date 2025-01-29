using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;

namespace Engine2.Entities
{
    public class ELocalPlayer : ECharacter
    {
        public float maxWalkSpeed = 0.2f;
        float LR;
        float UD;
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

        public void Movement()
        {
            LR = GlobalLookup.KeyMappings.GetAxis("axisMoveLR");
            UD = GlobalLookup.KeyMappings.GetAxis("axisMoveUD");

            if (LR == 0.0f && UD == 0.0f)
            {
                Velocity /= 1.4f;
            }
            else
            {
                Velocity.x = LR * maxWalkSpeed;
                Velocity.y = UD * maxWalkSpeed;
            }
        }
    }
}
