using Engine2.DataStructures;

namespace Engine2.core.classes.lookupTables
{
    public static class GlobalLookup
    {
        public static void InitAll()
        {
            KeyMappings.Init();
            TileLookup.Init();
        }

        public static partial class KeyMappings
        {
            public static Dictionary<string, Keys> keyMappings = new Dictionary<string, Keys>();
            public static Dictionary<string, List<AxisMapping>> axisMappings = new Dictionary<string, List<AxisMapping>>();
            public static void Init()
            {
                AddAxisMapping("axisMoveLR", new AxisMapping(Keys.A, -1.0f));
                AddAxisMapping("axisMoveLR", new AxisMapping(Keys.D, 1.0f));
                AddAxisMapping("axisMoveUD", new AxisMapping(Keys.W, 1.0f));
                AddAxisMapping("axisMoveUD", new AxisMapping(Keys.S, -1.0f));
            }

            public static void AddAxisMapping(string axisName, AxisMapping newMapping)
            {
                if (axisMappings.ContainsKey(axisName))
                {
                    axisMappings[axisName].Add(newMapping);
                }
                else
                {
                    axisMappings.Add(axisName, new List<AxisMapping>(){newMapping});
                }
            }

            public static Keys GetActionKeyMapping(string mappingName)
            {
                return keyMappings[mappingName];
            }

            public static List<AxisMapping> GetAxisMapping(string axisName)
            {
                return axisMappings[axisName];
            }

            public static float GetAxis(string axisName)
            {
                float axisValue = 0f;
                var axisMapping = axisMappings[axisName].ToList();

                for (int i = 0; i < axisMapping.Count; i++)
                {
                    if(Program.getEngine().heldKeys.Contains(axisMapping[i].key))
                    {
                        axisValue += axisMapping[i].value;
                    }
                    
                }
                return axisValue;
            }

            public static AxisMapping GetAxis(Keys key)
            {
                for (int i = 0; i < axisMappings.Values.Count; i++)
                {
                    for(int j = 0; j < axisMappings.Values.ToList()[i].Count; j++)
                    {
                        if (axisMappings.Values.ToList()[i][j].key == key)
                        {
                            return axisMappings.Values.ToList()[i][j];
                        }
                    }
                }
                return AxisMapping.Empty;
            }

            internal static AxisMapping? FindKeyValueInAxis(Keys key, List<AxisMapping> axes)
            {
                for (int i = 0; i < axes.Count; i++)
                {
                    if (axes[i].key == key)
                    {
                        return axes[i];
                    }
                }
                return null;
            }

            public static void SetActionKeyMapping(string mappingName, Keys newKey)
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

        public static class TileLookup
        {
            public static Dictionary<TileTypes, TileData> tileLookup = new Dictionary<TileTypes, TileData>();
            public static TileData GetTileData(TileTypes type)
            {
                return tileLookup[type];
            }
            public static void Init()
            {
                AddTileDef(TileTypes.Dirt, new TileData("Dirt", true, "TileDirt"));
            }

            public static void AddTileDef(TileTypes t, TileData data)
            {
                tileLookup.Add(t, data);
            }
        }
    }
}
