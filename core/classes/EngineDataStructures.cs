using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.core.classes
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
        }

        public struct FVector
        {
            public float x, y;

            public FVector(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }