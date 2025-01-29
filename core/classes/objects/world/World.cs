using Engine2.DataStructures;
using Engine2.Object;

namespace Engine2.World
{
    public class EWorld : ERenderableObject
    {
        public IVector worldSize;
        public Tile[][] tiles;
        public EWorld(int sz)
        {
            worldSize = new IVector(sz, sz);
            tiles = new Tile[sz][];
            System.Diagnostics.Debug.WriteLine(tiles);
        }

        public override void Render(Graphics g)
        {
            
        }
    }
}
