using Engine2.core.classes.lookupTables;
using Engine2.Entities;

namespace Engine2.Object
{
    public class EPlayerController : EObject
    {
        int posessedCharacter;
        List<Keys> launchedKeys = new List<Keys>();
        public EPlayerController() : base()
        {
            Program.getEngine().KeyUp += PCKeyUp;
        }

        private void PCKeyUp(object? sender, KeyEventArgs e)
        {
            launchedKeys.Remove(e.KeyCode);
        }

        public override void UpdateObject()
        {
            CheckAxisKeys(Program.getEngine().heldKeys);
            CheckInputKeys(Program.getEngine().heldKeys);
        }

        public void CheckInputKeys(List<Keys> keys)
        {
            if (GetPosessedCharacter() != null)
            {
                foreach (Keys key in keys)
                {
                    if(!launchedKeys.Contains(key))
                    {
                        launchedKeys.Add(key);
                        GetPosessedCharacter().KeyInput(key);
                    }
                }
            }
            //System.Diagnostics.Debug.WriteLine(string.Join(':', launchedKeys));
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
