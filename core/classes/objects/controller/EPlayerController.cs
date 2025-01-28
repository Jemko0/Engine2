using Engine2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.core.classes.objects.controller
{
    public class EPlayerController : EObject
    {
        int posessedCharacter;

        public EPlayerController() : base()
        {
            Program.getEngine().KeyDown += PCKeyDown;
        }

        private void PCKeyDown(object? sender, KeyEventArgs e)
        {
            if(GetPosessedCharacter() != null)
            {
                GetPosessedCharacter().KeyInput(e.KeyCode);
            }
            System.Diagnostics.Debug.WriteLine("NO POSESSED CHARACTER >> PCKeyDown(object? sender, KeyEventArgs e)");
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
