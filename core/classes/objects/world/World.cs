using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;
using Engine2.Object;
using Engine2.Rendering;
using System.CodeDom.Compiler;
using System.Security.Cryptography.Xml;

namespace Engine2.World
{
    public class EWorld : ERenderableObject
    {
        public IVector worldSize;
        public Tile[,] tiles;
        public Pen pen = new Pen(Color.LightPink);
        public EWorld(int sx, int sy)
        {
            worldSize = new IVector(sx, sy);
            tiles = new Tile[sx, sy];
            FillWorldFlatSurface();
            //System.Diagnostics.Debug.WriteLine(tiles);
        }

        public void FillWorldFlatSurface()
        {
            for(int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = new Tile(TileTypes.Dirt);
                }
            }
        }

        public override void Render(Graphics g)
        {
            Frame.StartCapture("World Rendering");
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j].type == Tile.None) continue;

                    FTransform transform = URenderer.GetTransformedTile(i, j);
                    if (Program.getEngine().Camera.IsRectInView(new FVector(transform.Translation.x, transform.Translation.y), new FVector(32, 32)))
                    {
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.DrawImage(GlobalLookup.TileLookup.GetTileData(tiles[i, j].type).tileSprite, transform.Translation.x, transform.Translation.y, transform.Scale.x, transform.Scale.y);
                    }
                }
            }
            Frame.EndCapture("World Rendering");
        }
    }
}