using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;
using Engine2.Object;
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
        protected float groundCheckDistance = 1.0f; // Small distance to check for ground

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
            // Check ground state before movement
            CheckGroundState();
            Movement();
            base.UpdateObject();
        }

        private void CheckGroundState()
        {
            isGrounded = false;
            
            // Create a small downward check
            FVector groundCheckVelocity = new FVector(0, -groundCheckDistance);
            
            // Check all potential ground collisions
            for(int i = 0; i < ObjectManager.renderingObjects.Count; i++) 
            {
                ECharacter entity = ObjectManager.renderingObjects[i] as ECharacter;
                if (entity == null || entity == this) continue;

                // Store original velocity
                FVector originalVelocity = Velocity;
                // Set velocity to ground check
                Velocity = groundCheckVelocity;
                
                SweptAABBResult sweep = SweptAABB(entity, Frame.deltaTime);
                
                // Restore original velocity
                Velocity = originalVelocity;

                if (sweep.Collision && sweep.Normal.y < 0)
                {
                    isGrounded = true;
                    break;
                }
            }
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
