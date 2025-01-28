using Engine2.core.classes;
using Engine2.DataStructures;
namespace Engine2.Entities
{
    public class ECharacter : EEntity
    {
        public FVector Velocity = new FVector(500, 0);
        public ECharacter()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            Transform.Scale = new FVector(50.0f, 50.0f);
        }

        public override void UpdateObject()
        {
            base.UpdateObject();
            Transform.Translation += Velocity * Frame.deltaTime;
        }
    }
}
