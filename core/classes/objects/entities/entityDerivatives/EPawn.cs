using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;

namespace Engine2.Entities
{
    public class EPawn : ECharacter
    {
        public float maxWalkSpeed = 200f;
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
