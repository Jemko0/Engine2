using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;
using Engine2.Object;
using Engine2.Rendering;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Engine2.World
{
    public class EWorld : ERenderableObject
    {
        public IVector worldSize;
        public Tile[,] tiles;
        public Camera cam;
        private const int TILE_SIZE = 32;
        private const int CHUNK_SIZE = 32;

        private Bitmap[,] chunkBuffers;
        private bool[,] chunkDirty;
        private Rectangle[] chunkBounds;
        private int chunksX, chunksY;
        private object[] chunkLocks;  // Removed readonly
        private float lastZoom = 1.0f;
        private FVector cachedHalfScreen;

        public EWorld(int sx, int sy)
        {
            worldSize = new IVector(sx, sy);
            tiles = new Tile[sx, sy];
            InitializeChunks();
            FillWorldFlatSurface();
            RenderAllChunks();
        }

        // Optimized transform calculation
        private FTransform GetTransformOptimized(int x, int y, Engine e, float halfWidth, float halfHeight)
        {
            float fx = (x * TILE_SIZE - cam.position.x + halfWidth);
            float fy = (cam.position.y - y * TILE_SIZE + halfHeight);
            float scaledX = fx * cam.zoom;
            float scaledY = fy * cam.zoom;
            float scaledSize = TILE_SIZE * cam.zoom;

            return new FTransform(
                new FVector(scaledX, scaledY),
                new FVector(scaledSize, scaledSize)
            );
        }

        private void InitializeChunks()
        {
            chunksX = (worldSize.x + CHUNK_SIZE - 1) / CHUNK_SIZE;
            chunksY = (worldSize.y + CHUNK_SIZE - 1) / CHUNK_SIZE;

            chunkBuffers = new Bitmap[chunksX, chunksY];
            chunkDirty = new bool[chunksX, chunksY];
            chunkBounds = new Rectangle[chunksX * chunksY];
            chunkLocks = new object[chunksX * chunksY];

            for (int cx = 0; cx < chunksX; cx++)
            {
                for (int cy = 0; cy < chunksY; cy++)
                {
                    chunkBuffers[cx, cy] = new Bitmap(CHUNK_SIZE * TILE_SIZE, CHUNK_SIZE * TILE_SIZE, PixelFormat.Format32bppPArgb);
                    chunkDirty[cx, cy] = true;
                    chunkLocks[cx + cy * chunksX] = new object();
                }
            }
        }

        public void FillWorldFlatSurface()
        {
            Parallel.For(0, worldSize.x, i =>
            {
                for (int j = 0; j < worldSize.y; j++)
                {
                    tiles[i, j] = new Tile(TileTypes.Dirt);
                }
            });
        }

        private void RenderChunk(int cx, int cy)
        {
            if (!chunkDirty[cx, cy]) return;

            lock (chunkLocks[cx + cy * chunksX])
            {
                var newBuffer = new Bitmap(CHUNK_SIZE * TILE_SIZE, CHUNK_SIZE * TILE_SIZE, PixelFormat.Format32bppPArgb);

                using (var g = Graphics.FromImage(newBuffer))
                {
                    g.Clear(Color.Transparent);
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = PixelOffsetMode.Half;

                    int startX = cx * CHUNK_SIZE;
                    int startY = cy * CHUNK_SIZE;
                    int endX = Math.Min(startX + CHUNK_SIZE, worldSize.x);
                    int endY = Math.Min(startY + CHUNK_SIZE, worldSize.y);

                    for (int x = startX; x < endX; x++)
                    {
                        for (int y = startY; y < endY; y++)
                        {
                            var tile = tiles[x, y];
                            if (tile.type == Tile.None) continue;

                            var sprite = GlobalLookup.TileLookup.GetTileData(tile.type).tileSprite;
                            g.DrawImage(sprite,
                                (x - startX) * TILE_SIZE,
                                (y - startY) * TILE_SIZE,
                                TILE_SIZE,
                                TILE_SIZE);
                        }
                    }
                }

                var oldBuffer = chunkBuffers[cx, cy];
                chunkBuffers[cx, cy] = newBuffer;
                oldBuffer?.Dispose();
                chunkDirty[cx, cy] = false;
            }
        }

        private void RenderAllChunks()
        {
            for (int cx = 0; cx < chunksX; cx++)
            {
                for (int cy = 0; cy < chunksY; cy++)
                {
                    RenderChunk(cx, cy);
                }
            }
        }

        public override void Render(Graphics g)
        {
            Frame.StartCapture("World Rendering");

            cam = Program.getEngine().Camera;
            var engine = Program.getEngine();

            // Cache half screen dimensions
            float halfWidth = engine.Width / (2 * cam.zoom);
            float halfHeight = engine.Height / (2 * cam.zoom);

            // Check if zoom changed
            if (lastZoom != cam.zoom)
            {
                lastZoom = cam.zoom;
                cachedHalfScreen = new FVector(halfWidth, halfHeight);
            }

            g.CompositingMode = CompositingMode.SourceCopy;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            // Calculate visible range in tile coordinates
            int startX = Math.Max(0, (int)((cam.position.x - halfWidth) / TILE_SIZE));
            int startY = Math.Max(0, (int)((cam.position.y - halfHeight) / TILE_SIZE));
            int endX = Math.Min(worldSize.x, (int)((cam.position.x + halfWidth) / TILE_SIZE) + 1);
            int endY = Math.Min(worldSize.y, (int)((cam.position.y + halfHeight) / TILE_SIZE) + 1);

            // Convert to chunk coordinates
            int startChunkX = startX / CHUNK_SIZE;
            int startChunkY = startY / CHUNK_SIZE;
            int endChunkX = Math.Min(chunksX, (endX / CHUNK_SIZE) + 1);
            int endChunkY = Math.Min(chunksY, (endY / CHUNK_SIZE) + 1);

            for (int cx = startChunkX; cx < endChunkX; cx++)
            {
                for (int cy = startChunkY; cy < endChunkY; cy++)
                {
                    lock (chunkLocks[cx + cy * chunksX])
                    {
                        var buffer = chunkBuffers[cx, cy];
                        if (buffer != null)
                        {
                            var transform = GetTransformOptimized(cx * CHUNK_SIZE, cy * CHUNK_SIZE, engine, halfWidth, halfHeight);
                            g.DrawImage(buffer,
                                transform.Translation.x,
                                transform.Translation.y,
                                transform.Scale.x * CHUNK_SIZE,
                                transform.Scale.y * CHUNK_SIZE);
                        }
                    }
                }
            }

            Frame.EndCapture("World Rendering");
        }

        public void SetTile(int x, int y, TileTypes type)
        {
            tiles[x, y] = new Tile(type);

            int cx = x / CHUNK_SIZE;
            int cy = y / CHUNK_SIZE;
            chunkDirty[cx, cy] = true;

            RenderChunk(cx, cy);
        }

        public void Dispose()
        {
            for (int cx = 0; cx < chunksX; cx++)
            {
                for (int cy = 0; cy < chunksY; cy++)
                {
                    chunkBuffers[cx, cy]?.Dispose();
                }
            }
        }
    }
}