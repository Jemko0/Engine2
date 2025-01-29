using Engine2.DataStructures;
using Engine2.Entities;

namespace Engine2.core.classes.objects
{
    public class Camera : EObject
    {
        public EEntity followTarget;
        public FVector position;
        public float zoom = 1.0f;
        public override void UpdateObject()
        {
            if(followTarget != null)
            {
                position = followTarget.Transform.Translation;
            }
        }

        public void SetTarget(EEntity target)
        {
            followTarget = target;
        }
    }
}
