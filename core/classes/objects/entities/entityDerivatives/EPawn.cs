using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;
using Engine2.Physics;

namespace Engine2.Entities
{
    public class EPawn : ECharacter
    {
        public float maxWalkSpeed = 200f;
        public float jumpPower = 500f;
        protected float LR;
        protected float UD;

        /// <summary>
        /// A Pawn has basic Movement functionality that is ready to be implemented in
        /// deriving classes
        /// </summary>
        public EPawn()
        {
            Transform.Scale = new FVector(20, 50);
            brush = new SolidBrush(Color.YellowGreen);
        }

        public override void UpdateObject()
        {
            Movement();
            base.UpdateObject();
        }

        public virtual void Movement()
        {
            Velocity.y -= GlobalPhysics.g;

            if (LR == 0.0f && UD == 0.0f)
            {
                Velocity.x /= 1 + 8f * Frame.deltaTime;
            }
            else
            {
                Velocity.x = LR * maxWalkSpeed;
            }
        }

        public virtual void Jump()
        {
            Velocity.y = jumpPower;
        }
    }
}
