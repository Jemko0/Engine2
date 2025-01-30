using Engine2.core.classes;
using Engine2.core.classes.lookupTables;
using Engine2.DataStructures;
using Engine2.Object;
using Engine2.Entities;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Engine2.World
{
    public class EWorld : ERenderableObject
    {
        public IVector worldSize;
        public Tile[,] tiles;
        public Camera cam;
        private const int TILE_SIZE = 32;
        private const int CHUNK_SIZE = 32;
        private const float CULL_PADDING = 2.0f; // Added culling padding factor - increase to cull chunks earlier
        private const int CHUNK_PADDING = 0; // Added padding constant

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
            float fy = (y * TILE_SIZE + cam.position.y + halfHeight); // Fixed: Changed + to - for y coordinate
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
                    using (Graphics g = Graphics.FromImage(chunkBuffers[cx, cy]))
                    {
                        g.Clear(Color.Transparent);
                    }
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
                using (var g = Graphics.FromImage(chunkBuffers[cx, cy]))
                {
                    g.Clear(Color.Transparent);
                    g.CompositingMode = CompositingMode.SourceOver;
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
                            if (sprite != null)
                            {
                                g.DrawImage(sprite,
                                    (x - startX) * TILE_SIZE,
                                    (y - startY) * TILE_SIZE,
                                    TILE_SIZE,
                                    TILE_SIZE);
                            }
                        }
                    }

                    // Debug: Draw chunk borders
                    using (Pen debugPen = new Pen(Color.Red, 2))
                    {
                        g.DrawRectangle(debugPen, 0, 0, CHUNK_SIZE * TILE_SIZE - 1, CHUNK_SIZE * TILE_SIZE - 1);
                    }
                }
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

            float halfWidth = engine.Width / (2 * cam.zoom);
            float halfHeight = engine.Height / (2 * cam.zoom);

            // Check if zoom changed
            if (lastZoom != cam.zoom)
            {
                lastZoom = cam.zoom;
                cachedHalfScreen = new FVector(halfWidth, halfHeight);
            }

            g.CompositingMode = CompositingMode.SourceOver;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;

            // Added padding to culling calculations
            float paddedHalfWidth = halfWidth / CULL_PADDING;
            float paddedHalfHeight = halfHeight / CULL_PADDING;

            // Fixed culling calculations with padding
            int startX = Math.Max(0, (int)((cam.position.x - paddedHalfWidth) / TILE_SIZE));
            int startY = Math.Max(0, (int)((-cam.position.y - paddedHalfHeight) / TILE_SIZE));
            int endX = Math.Min(worldSize.x, (int)Math.Ceiling((cam.position.x + paddedHalfWidth) / TILE_SIZE));
            int endY = Math.Min(worldSize.y, (int)Math.Ceiling((-cam.position.y + paddedHalfHeight) / TILE_SIZE));

            // Convert to chunk coordinates
            int startChunkX = Math.Max(0, startX / CHUNK_SIZE);
            int startChunkY = Math.Max(0, startY / CHUNK_SIZE);
            int endChunkX = Math.Min(chunksX, (endX + CHUNK_SIZE - 1) / CHUNK_SIZE);
            int endChunkY = Math.Min(chunksY, (endY + CHUNK_SIZE - 1) / CHUNK_SIZE);

            for (int cx = startChunkX; cx < endChunkX; cx++)
            {
                for (int cy = startChunkY; cy < endChunkY; cy++)
                {
                    lock (chunkLocks[cx + cy * chunksX])
                    {
                        if (chunkDirty[cx, cy])
                        {
                            RenderChunk(cx, cy);
                        }

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

        public List<(FVector Position, FVector Scale)> GetCollidingTileData(FVector min, FVector max)
        {
            var collidingTiles = new List<(FVector Position, FVector Scale)>();
            
            // Convert world coordinates to tile coordinates, and clamp to world bounds
            int startX = Math.Max(0, (int)Math.Floor(min.x / TILE_SIZE));
            int startY = Math.Max(0, (int)Math.Floor(-max.y / TILE_SIZE));
            int endX = Math.Min(worldSize.x - 1, (int)Math.Floor(max.x / TILE_SIZE));
            int endY = Math.Min(worldSize.y - 1, (int)Math.Floor(-min.y / TILE_SIZE));

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    if (tiles[x, y].type != TileTypes.None)
                    {
                        collidingTiles.Add((
                            new FVector(x * TILE_SIZE, -y * TILE_SIZE),
                            new FVector(TILE_SIZE, TILE_SIZE)
                        ));
                    }
                }
            }

            return collidingTiles;
        }

        public (int x, int y, bool valid) ScreenToTileIndices(int screenX, int screenY)
        {
            var engine = Program.getEngine();
            
            // Convert screen to world coordinates
            float worldX = (screenX / cam.zoom) + cam.position.x - (engine.Width / (2 * cam.zoom));
            float worldY = (-screenY / cam.zoom) + cam.position.y + (engine.Height / (2 * cam.zoom));
            
            // Convert world coordinates to tile indices
            int tileX = (int)Math.Floor((worldX + TILE_SIZE/2) / TILE_SIZE);
            int tileY = (int)Math.Floor((-worldY + TILE_SIZE/2) / TILE_SIZE);
            
            // Check if the coordinates are within bounds
            bool valid = tileX >= 0 && tileX < worldSize.x && tileY >= 0 && tileY < worldSize.y;
            
            return (tileX, tileY, valid);
        }
    }
}