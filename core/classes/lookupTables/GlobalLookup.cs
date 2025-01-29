namespace Engine2.core.classes.lookupTables
{
    public static class GlobalLookup
    {
        public static partial class KeyMappingsLookup
        {
            public static Dictionary<string, Keys> keyMappings = new Dictionary<string, Keys>();

            public static void Init()
            {
                keyMappings.Add("axisForward", Keys.W);
                keyMappings.Add("axisBackward", Keys.S);
                keyMappings.Add("axisLeft", Keys.A);
                keyMappings.Add("axisRight", Keys.D);
            }

            public static Keys GetMappingKey(string mappingName)
            {
                return keyMappings[mappingName];
            }

            public static void SetKeyMapping(string mappingName, Keys newKey)
            {
                if (keyMappings.ContainsKey(mappingName))
                {
                    keyMappings[mappingName] = newKey;
                }
                else
                {
                    keyMappings.Add(mappingName, newKey);
                }
            }
        }
    }
}
