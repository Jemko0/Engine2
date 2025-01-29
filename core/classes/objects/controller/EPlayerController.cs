using Engine2.core.classes.lookupTables;
using Engine2.Entities;

namespace Engine2.Object
{
    public class EPlayerController : EObject
    {
        int posessedCharacter;

        public EPlayerController() : base()
        {
            //bind to self
            Program.getEngine().KeyDown += PCKeyDown;
        }

        public override void UpdateObject()
        {
            CheckAxisKeys(Program.getEngine().heldKeys);
        }

        public void CheckAxisKeys(List<Keys> currentKeys)
        {
            if(GetPosessedCharacter() != null)
            {
                foreach (Keys key in currentKeys)
                {
                    var axis = GlobalLookup.KeyMappings.GetAxis(key);
                    GetPosessedCharacter().AxisInput(axis);
                }
            }
        }

        private void PCKeyDown(object? sender, KeyEventArgs e)
        {
            if(GetPosessedCharacter() != null)
            {
                GetPosessedCharacter().KeyInput(e.KeyCode);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("NO POSESSED CHARACTER >> PCKeyDown(object? sender, KeyEventArgs e)");
            }
            
        }

        public void Posess(ECharacter c)
        {
            posessedCharacter = c.objectID;
        }

        /// <summary>
        /// Check for null since this can return null if theres no posessed character
        /// </summary>
        /// <returns></returns>
        public ECharacter? GetPosessedCharacter()
        {
            if(ObjectManager.objects[posessedCharacter] != null)
            {
                return ObjectManager.objects[posessedCharacter] as ECharacter;
            }
            return null;
        }
    }
}
