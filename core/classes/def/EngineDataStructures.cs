using Engine2.Entities;
using System.Diagnostics.Tracing;
using System.Numerics;

namespace Engine2.DataStructures
{
    public struct FTransform
    {
        public FVector Translation;
        public float Angle;
        public FVector Scale;

        public FTransform(FVector Translation, FVector Scale)
        {
            this.Translation = Translation;
            this.Scale = Scale;
        }


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

    public struct FVector : IAdditionOperators<FVector, FVector, FVector>, ISubtractionOperators<FVector, FVector, FVector>, IMultiplyOperators<FVector, FVector, FVector>, IMultiplyOperators<FVector, float, FVector>, IDivisionOperators<FVector, float, FVector>, IEquatable<FVector>, IEqualityOperators<FVector, FVector, bool>
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

        public static bool operator ==(FVector left, FVector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FVector left, FVector right)
        {
            return !left.Equals(right);
        }

        public static float Dot(FVector a, FVector b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public float Dot(FVector other)
        {
            return x * other.x + y * other.y;
        }

        public bool Equals(FVector other)
        {
            return x == other.x && y == other.y;
        }

        public static FVector Zero => new FVector(0, 0);
    }

    public struct IVector
    {
        public int x, y;
        public IVector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct DeltaCaptureResult
    {
        public float DeltaMS;
        public double FPS;
    }
    public struct SweptAABBResult
    {
        public bool Collision;      // Whether a collision occurred
        public float CollisionTime; // Time of collision (0-1)
        public FVector Normal;      // Normal of the collision surface
        public EEntity HitEntity;   // The entity we collided with
    }

    public struct AxisMapping : IEquatable<AxisMapping>
    {
        public Keys key;
        public float value;
        
        public AxisMapping(Keys key, float value)
        {
            this.key = key;
            this.value = value;
        }
        #region constants
        public static AxisMapping Empty => new AxisMapping(Keys.None, float.NaN);
        #endregion

        public bool Equals(AxisMapping other)
        {
            return other.key == key && other.value == value;
        }
    }

    #region TILES AND WORLD
    public enum TileTypes
    {
        None = 0,
        Dirt,
        Stone,
    }
    public struct Tile
    {
        public TileTypes type;

        public Tile()
        {

        }

        public Tile(TileTypes type)
        {
            this.type = type;
        }

        public static TileTypes None => TileTypes.None;
    }

    public struct TileData
    {
        public string displayName;
        public bool tileCollide;
        public Image tileSprite;

        public TileData(string name, bool collision, string spriteName)
        {
            displayName = name;
            tileCollide = collision;
            tileSprite = (Bitmap)Properties.Resources.ResourceManager.GetObject(spriteName);
        }
    }
    #endregion

}