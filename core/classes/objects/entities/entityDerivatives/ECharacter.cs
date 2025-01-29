using Engine2.core.classes;
using Engine2.core.interfaces;
using Engine2.DataStructures;

namespace Engine2.Entities
{
    public class ECharacter : EEntity, IInputInterface
    {
        public FVector Velocity = new FVector(0f, 0);

        public ECharacter()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            Transform.Scale = new FVector(50.0f, 50.0f);
        }

        private (FVector Min, FVector Max) GetAABB()
        {
            FVector min = Transform.Translation;
            FVector max = Transform.Translation + Transform.Scale;
            return (min, max);
        }


        // Perform swept AABB collision test against another entity
        private SweptAABBResult SweptAABB(ECharacter other, float deltaTime)
        {
            SweptAABBResult result = new SweptAABBResult
            {
                Collision = false,
                CollisionTime = 1.0f,
                Normal = new FVector(),
                HitEntity = null
            };

            // Get the AABBs for both entities
            var (myMin, myMax) = GetAABB();
            var (otherMin, otherMax) = other.GetAABB();

            // Calculate relative velocity
            FVector relativeVel = Velocity * deltaTime;

            // Early exit if there's no relative movement
            if (relativeVel.x == 0 && relativeVel.y == 0)
                return result;

            // Calculate inverse of velocity to avoid division
            FVector invEntry = new FVector();
            FVector invExit = new FVector();

            // Calculate distance to collision entry and exit for each axis
            CalculateEntryExitTimes(
                myMin, myMax, otherMin, otherMax,
                relativeVel, ref invEntry, ref invExit);

            // Find entry and exit times for each axis
            float entryTime = Math.Max(invEntry.x, invEntry.y);
            float exitTime = Math.Min(invExit.x, invExit.y);

            // Check if there's no collision
            if (entryTime > exitTime || entryTime > 1.0f || entryTime < 0.0f)
                return result;

            // We have a collision! Set up the result
            result.Collision = true;
            result.CollisionTime = entryTime;
            result.HitEntity = other;

            // Calculate the normal of the collision
            if (invEntry.x > invEntry.y)
            {
                result.Normal = new FVector(relativeVel.x < 0 ? 1 : -1, 0);
            }
            else
            {
                result.Normal = new FVector(0, relativeVel.y < 0 ? 1 : -1);
            }

            return result;
        }

        // Helper function to calculate entry and exit times
        private void CalculateEntryExitTimes(
            FVector myMin, FVector myMax,
            FVector otherMin, FVector otherMax,
            FVector relativeVel,
            ref FVector invEntry,
            ref FVector invExit)
        {
            // X-axis calculations
            if (relativeVel.x > 0)
            {
                invEntry.x = (otherMin.x - myMax.x) / relativeVel.x;
                invExit.x = (otherMax.x - myMin.x) / relativeVel.x;
            }
            else if (relativeVel.x < 0)
            {
                invEntry.x = (otherMax.x - myMin.x) / relativeVel.x;
                invExit.x = (otherMin.x - myMax.x) / relativeVel.x;
            }
            else
            {
                invEntry.x = float.MinValue;
                invExit.x = float.MaxValue;
            }

            // Y-axis calculations
            if (relativeVel.y > 0)
            {
                invEntry.y = (otherMin.y - myMax.y) / relativeVel.y;
                invExit.y = (otherMax.y - myMin.y) / relativeVel.y;
            }
            else if (relativeVel.y < 0)
            {
                invEntry.y = (otherMax.y - myMin.y) / relativeVel.y;
                invExit.y = (otherMin.y - myMax.y) / relativeVel.y;
            }
            else
            {
                invEntry.y = float.MinValue;
                invExit.y = float.MaxValue;
            }
        }

        public override void UpdateObject()
        {
            // Store the original position
            FVector originalPos = Transform.Translation;

            // First, update position as normal
            Transform.Translation += Velocity * Frame.deltaTime;
            base.UpdateObject();

            // Check for collisions with all other entities
            foreach (ECharacter entity in ObjectManager.renderingObjects)
            {
                if (entity == this) continue;

                SweptAABBResult sweep = SweptAABB(entity, Frame.deltaTime);

                if (sweep.Collision)
                {
                    // Move to point of collision
                    Transform.Translation = originalPos + Velocity * Frame.deltaTime * sweep.CollisionTime;

                    // Calculate slide response
                    FVector remainingTime = Velocity * Frame.deltaTime * (1.0f - sweep.CollisionTime);
                    FVector reflection = remainingTime - 2 * FVector.Dot(remainingTime, sweep.Normal) * sweep.Normal;

                    // Apply sliding motion
                    Transform.Translation += reflection;

                    // Optionally modify velocity for future updates
                    Velocity = ReflectVelocity(sweep.Normal);
                    break;
                }
            }
        }

        // Helper function to reflect velocity off a surface
        private FVector ReflectVelocity(FVector normal)
        {
            // For a bounce effect:
            // return Velocity - 2 * FVector.Dot(Velocity, normal) * normal;

            // For a slide effect:
            return Velocity - FVector.Dot(Velocity, normal) * normal;
        }

        public virtual void AxisInput(string id, FVector axisVal)
        {
            throw new NotImplementedException();
        }

        public virtual void KeyInput(Keys keyVal)
        {
            throw new NotImplementedException();
        }
    }
}
