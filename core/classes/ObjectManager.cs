﻿using Engine2.core.interfaces;

namespace Engine2.core.classes
{

    public class ObjectManager
    {
        public static List<EObject> objects;
        public static List<EObject> renderingObjects;
        public ObjectManager()
        {
            objects = new List<EObject>();
            renderingObjects = new List<EObject>();
        }

        public static int FindObject(EObject o)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == o)
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool DoesImplementInterface(EObject o)
        {
            return o is IObjectRenderInterface;
        }

        public static int RegisterObject(EObject o)
        {
            objects.Add(o);
            if(DoesImplementInterface(o))
            {
                renderingObjects.Add(o);
            }
            return FindObject(o);
        }

        public void UpdateObjects()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].UpdateObject();
            }
        }
    }

    public static class EInstance
    {
        public static void New(EObject newObject)
        {
            ObjectManager.RegisterObject(newObject);
        }
    }
}
