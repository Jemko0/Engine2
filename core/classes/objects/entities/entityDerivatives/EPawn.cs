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
        protected bool isGrounded = false;

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
            // Check if we're grounded based on collision normal
            isGrounded = false;
            if (lastSweep.Collision && lastSweep.Normal.y < 0)
            {
                isGrounded = true;
            }

            Movement();
            base.UpdateObject();
        }

        public virtual void Movement()
        {
            // Only apply gravity if we're not grounded
            if (!isGrounded)
            {
                Velocity.y -= GlobalPhysics.g;
            }

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
            if (isGrounded)
            {
                Velocity.y = jumpPower;
                isGrounded = false;
            }
        }
    }
}
