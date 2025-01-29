using Engine2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.DataStructures
{
    public struct FTransform
    {
        public FVector Translation;
        public float Angle;
        public FVector Scale;

        public void SetTranslation(float x, float y)
        {
            Translation.x = x;
            Translation.y = y;
        }

        public void SetAngle(float a)
        {
            Angle = a;
        }

        public void SetScale(float x, float y)
        {
            Scale.x = x;
            Scale.y = y;
        }

        public void ScaleTransform(float scaleFactor)
        {
            Translation.x *= scaleFactor;
            Translation.y *= scaleFactor;
            Scale.x *= scaleFactor;
            Scale.y *= scaleFactor;
        }
    }

    public struct FVector : IAdditionOperators<FVector, FVector, FVector>, ISubtractionOperators<FVector, FVector, FVector>, IMultiplyOperators<FVector, FVector, FVector>, IMultiplyOperators<FVector, float, FVector>, IDivisionOperators<FVector, float, FVector>
    {
        public float x, y;

        public FVector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        //INTERFACE IMPLEMENTATIONS
        public static FVector operator +(FVector left, FVector right)
        {
            left.x += right.x;
            left.y += right.y;
            return left;
        }

        public static FVector operator -(FVector left, FVector right)
        {
            left.x -= right.x;
            left.y -= right.y;
            return left;
        }

        public static FVector operator *(FVector left, FVector right)
        {
            left.x *= right.x;
            left.y *= right.y;
            return left;
        }

        public static FVector operator *(FVector left, float right)
        {
            left.x *= right;
            left.y *= right;
            return left;
        }

        public static FVector operator *(float left, FVector right)
        {
            right.x *= left;
            right.y *= left;
            return right;
        }

        public static FVector operator /(FVector left, float right)
        {
            left.x /= right;
            left.y /= right;
            return left;
        }

        public static float Dot(FVector a, FVector b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public float Dot(FVector other)
        {
            return x * other.x + y * other.y;
        }
    }

    public struct DeltaCaptureResult
    {
        public float Delta;
        public int FPS;
    }
    public struct SweptAABBResult
    {
        public bool Collision;      // Whether a collision occurred
        public float CollisionTime; // Time of collision (0-1)
        public FVector Normal;      // Normal of the collision surface
        public EEntity HitEntity;   // The entity we collided with
    }

    public struct AxisMapping
    {
        public Keys key;
        public float value;
        
        public AxisMapping(Keys key, float value)
        {
            this.key = key;
            this.value = value;
        }

        public static AxisMapping empty = new AxisMapping(Keys.None, float.NaN);
    }

}