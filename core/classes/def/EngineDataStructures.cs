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

    public struct FVector : IAdditionOperators<FVector, FVector, FVector>, ISubtractionOperators<FVector, FVector, FVector>, IMultiplyOperators<FVector, FVector, FVector>, IMultiplyOperators<FVector, float, FVector>
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
    }

    public struct DeltaCaptureResult
    {
        public float Delta;
        public int FPS;
    }
}