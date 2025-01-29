using Engine2.core.classes;
using Engine2.core.interfaces;
using Engine2.DataStructures;
using Engine2.Object;

namespace Engine2.Entities
{
    /// <summary>
    /// A Character is an <see cref="EEntity"/>
    /// that can be controlled by a <see cref="EPlayerController"/>
    /// </summary>
    public class ECharacter : EEntity, IInputInterface
    {
        public FVector Velocity;

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
            // For a top-left origin system, Y needs to be handled differently than X
            FVector min = new FVector(
                Transform.Translation.x,
                Transform.Translation.y - Transform.Scale.y
            );
            FVector max = new FVector(
                Transform.Translation.x + Transform.Scale.x,
                Transform.Translation.y
            );
            return (min, max);
        }

        private SweptAABBResult SweptAABB(ECharacter other, float deltaTime)
        {
            SweptAABBResult result = new SweptAABBResult
            {
                Collision = false,
                CollisionTime = 1.0f,
                Normal = new FVector(),
                HitEntity = null
            };

            var (myMin, myMax) = GetAABB();
            var (otherMin, otherMax) = other.GetAABB();

            FVector relativeVel = Velocity * deltaTime;

            if (relativeVel.x == 0 && relativeVel.y == 0)
                return result;

            // Calculate entry and exit times
            FVector invEntry = new FVector();
            FVector invExit = new FVector();

            CalculateEntryExitTimes(
                myMin, myMax, otherMin, otherMax,
                relativeVel, ref invEntry, ref invExit);

            // Find entry and exit times for each axis
            float entryTime = Math.Max(invEntry.x, invEntry.y);
            float exitTime = Math.Min(invExit.x, invExit.y);

            // Check if there's no collision
            if (entryTime > exitTime || entryTime > 1.0f || entryTime < 0.0f)
                return result;

            // if collision
            result.Collision = true;
            result.CollisionTime = entryTime;
            result.HitEntity = other;

            // Calculate the normal based on which axis had the later entry time
            // AND direction of approach
            if (invEntry.x > invEntry.y)
            {
                // X-axis collision
                if (myMin.x < otherMin.x)
                {
                    result.Normal = new FVector(1, 0); // Colliding from left
                }
                else
                {
                    result.Normal = new FVector(-1, 0); // Colliding from right
                }
            }
            else
            {
                // Y-axis collision
                if (myMin.y < otherMin.y)
                {
                    result.Normal = new FVector(0, 1); // Colliding from bottom
                }
                else
                {
                    result.Normal = new FVector(0, -1); // Colliding from top
                }
            }

            return result;
        }

        private void CalculateEntryExitTimes(
    FVector myMin, FVector myMax,
    FVector otherMin, FVector otherMax,
    FVector relativeVel,
    ref FVector invEntry,
    ref FVector invExit)
        {
            // X axis
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
                // When velocity == 0, check if objects overlap on this axis
                if (myMax.x <= otherMin.x || myMin.x >= otherMax.x)
                {
                    // No coll no overlap
                    invEntry.x = float.MaxValue;
                    invExit.x = float.MinValue;
                }
                else
                {
                    // obj overlap on this axis
                    invEntry.x = float.MinValue;
                    invExit.x = float.MaxValue;
                }
            }

            // Y-axisisisis
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
                // When velocity is 0, check if objects overlap on this axis
                if (myMax.y <= otherMin.y || myMin.y >= otherMax.y)
                {
                    // No overlap, no collision possible
                    invEntry.y = float.MaxValue;
                    invExit.y = float.MinValue;
                }
                else
                {
                    // Objects overlap on this axis
                    invEntry.y = float.MinValue;
                    invExit.y = float.MaxValue;
                }
            }
        }

        private void AlignWithCollisionSurface(ECharacter other, FVector normal)
        {
            var (myMin, myMax) = GetAABB();
            var (otherMin, otherMax) = other.GetAABB();

            const float EPSILON = 0.001f;

            if (normal.x > 0)
            {
                float newX = otherMin.x - Transform.Scale.x - EPSILON;
                Transform.Translation = new FVector(newX, Transform.Translation.y);
            }
            else if (normal.x < 0)
            {
                float newX = otherMax.x + EPSILON;
                Transform.Translation = new FVector(newX, Transform.Translation.y);
            }
            else if (normal.y > 0)
            {
                float newY = otherMin.y - Transform.Scale.y - EPSILON;
                Transform.Translation = new FVector(Transform.Translation.x, newY);
            }
            else if (normal.y < 0)
            {
                float newY = otherMax.y + EPSILON;
                Transform.Translation = new FVector(Transform.Translation.x, newY);
            }
        }

        public override void UpdateObject()
        {
            FVector originalPos = Transform.Translation;

            // intended movement
            FVector movement = Velocity * Frame.deltaTime;

            foreach (ECharacter entity in ObjectManager.renderingObjects)
            {
                if (entity == this) continue;

                SweptAABBResult sweep = SweptAABB(entity, Frame.deltaTime);

                if (sweep.Collision)
                {
                    // Move only up to the collision
                    Transform.Translation = originalPos + Velocity * Frame.deltaTime * sweep.CollisionTime;

                    // Calculate slide vector
                    FVector remainingTime = Velocity * Frame.deltaTime * (1.0f - sweep.CollisionTime);
                    FVector slideVector = remainingTime - FVector.Dot(remainingTime, sweep.Normal) * sweep.Normal;

                    // Update velocity for future frames (before applying slide)
                    Velocity = ReflectVelocity(sweep.Normal);

                    // Test if sliding would cause another collision
                    Transform.Translation += slideVector;
                    var slideCollision = SweptAABB(entity, Frame.deltaTime * (1.0f - sweep.CollisionTime));

                    if (slideCollision.Collision)
                    {
                        Transform.Translation = originalPos + Velocity * Frame.deltaTime * sweep.CollisionTime;
                    }

                    return;
                }
            }

            // If no collision, complete movement
            Transform.Translation = originalPos + movement;
            base.UpdateObject();
        }

        private FVector ReflectVelocity(FVector normal)
        {
            // For a bounce effect:
            // return Velocity - 2 * FVector.Dot(Velocity, normal) * normal;

            // For a slide effect:
            return Velocity - FVector.Dot(Velocity, normal) * normal;
        }

        public virtual void AxisInput(AxisMapping axis)
        {
        }

        public virtual void KeyInput(Keys keyVal)
        {
        }
    }
}
