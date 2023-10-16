using System;
using System.Linq;
using TEdit.ViewModel;
using System.IO;
using System.Windows;
using TEdit.Terraria;
using System.Threading;

namespace TEdit.Editor.Plugins
{
    internal class PaletteCreator : BasePlugin
    {
        public PaletteCreator(WorldViewModel worldViewModel) : base(worldViewModel)
        {
            Name = "Palette Creator";
        }
        public override void Execute()
        {
            if (_wvm.CurrentWorld == null)
                return;
            PaletteCreatorPluginSettings view = new PaletteCreatorPluginSettings();
            if (view.ShowDialog() == false) { return; }
            bool UserDataExc = view.UsersChouseExc ?? false; // Exceptuions
            bool UserDataTrch = view.UsersChouseTrch ?? false; // Torxh
            string Exceptions = view.ExceptionsPath; // Exceptions
            string Torchs = (view.TorchsPath != null) ? view.TorchsPath : ""; // Torch
            string[] TorchsFile = { };
            string[] ExceptionsFile = { };

            //bool InvertTorch = view.InvertTorchs ?? false;
            if (Torchs != null && Torchs.Contains(".txt")) { TorchsFile = File.ReadAllLines(Torchs); }
            if (Exceptions.Contains(".txt") && Exceptions != null) { ExceptionsFile = File.ReadAllLines(Exceptions); }
            ArtData.SetTorchs(TorchsFile, UserDataTrch);
            ArtData.SetExceptions(ExceptionsFile, UserDataExc);
            if (view.InvertExeptions ?? false)
            {
                ArtData.InvertExeption(World.TileCount,World.WallCount);
            }
            ArtData.AddDefault();
            PaletteSetter();
            //_wvm.UpdateRenderRegion(new Microsoft.Xna.Framework.Rectangle(refX2, miny, refX - minx, maxy));
        }

        private void SetAir(Terraria.Tile curTile)
        {
            curTile.Type = 0;
            curTile.Wall = 0;
            curTile.TileColor = 0;
            curTile.WallColor = 0;
            curTile.IsActive = false;
            curTile.InActive = false;
            curTile.Actuator = false;
            curTile.BrickStyle = BrickStyle.Full;
            curTile.U = 0;
            curTile.V = 0;
        }
        public void PaletteSetter()
        {
            // Stage World Vars
            int minx;
            int refX2;
            int refX;
            int miny;
            minx = refX2 = refX = miny =  100;
            int maxx = this._wvm.CurrentWorld.TilesWide - 100;
            int maxy = this._wvm.CurrentWorld.TilesHigh - 100;

            // Reset Vars
            int tile = 0;
            int paint = 0;
            string[] ExTiles = ArtData.Exceptions.Tiles.ToArray();
            string[] ExWalls = ArtData.Exceptions.Walls.ToArray();
            string[] torchs = ArtData.Torchs.ToArray();
            int count = 0;
            for (int x = minx; x < maxx; x++)
            {
                for (int y = miny; y < maxy; y++)
                {
                    if (count >= ((World.TileCount*31 + World.WallCount*31)) * 2) { goto Startup; }
                    if (_wvm.CurrentWorld.Tiles[x, y].IsActive == false && _wvm.CurrentWorld.Tiles[x, y].Wall == 0)
                    {
                        count++;
                        continue;
                    }
                    SetAir(_wvm.CurrentWorld.Tiles[x, y]);
                    
                }
                // Offset Right
                //x++;
                refX = x;
            }
            //_wvm.UpdateRenderRegion(new Microsoft.Xna.Framework.Rectangle(minx, miny, refX - minx, maxy));
        Startup:
            // First Do Tiles
            for (int x = minx; x < maxx; x++)
            {
                for (int y = miny; y < maxy; y++)
                {
                    try
                    {
                        if (!ExTiles.Contains(tile.ToString()))
                        {   
                            if (!torchs.Contains(tile.ToString()))
                            {
                                this._wvm.CurrentWorld.Tiles[x, y].Type = (ushort)tile;
                                this._wvm.CurrentWorld.Tiles[x, y].IsActive = true;
                                this._wvm.CurrentWorld.Tiles[x, y].TileColor = (byte)paint;
                            }
                            else
                            {
                                this._wvm.CurrentWorld.Tiles[x, y].Type = (ushort)tile;
                                this._wvm.CurrentWorld.Tiles[x, y].IsActive = true;
                                this._wvm.CurrentWorld.Tiles[x, y].TileColor = (byte)paint;
                                this._wvm.CurrentWorld.Tiles[x, y].Wall = (ushort)1;
                            }
                            
                            //this._wvm.CurrentWorld.Tiles[x, y].InActive = true;
                            //_wvm.UpdateRenderPixel(x, y);
                        }
                        if (tile == World.TileCount && paint == 30)
                        {
                            // Define New Vars
                            minx = (x + 1);
                            goto LeaveTileLoop;
                        }

                        if (paint == 30)
                        {
                            tile++;
                            paint = 0;
                        }
                        else
                        {
                            paint++;
                        }

                    }
                    catch (Exception)
                    {
                        //System.Windows.Forms.MessageBox.Show($"Tile placement error on ({x},{y}) tile - ({tile})");
                        goto LeaveTileLoop;
                    }
                }

                // Offset Right
                //x++;
            }

        LeaveTileLoop:
            // Reset Vars
            tile = 1;
            paint = 0;

            // Next Do Walls
            for (int x = minx; x < maxx; x++)
            {
                for (int y = miny; y < maxy; y++)
                {
                    if (!ExWalls.Contains(tile.ToString()))
                    {
                        this._wvm.CurrentWorld.Tiles[x, y].Wall = (ushort)tile;
                        this._wvm.CurrentWorld.Tiles[x, y].WallColor = (byte)paint;
                        //_wvm.UpdateRenderPixel(x, y);
                    }
                    if (tile == World.WallCount && paint == 30)
                    {
                        // Define New Vars
                        minx = x;
                        _wvm.MinimapImage = Render.RenderMiniMap.Render(_wvm.CurrentWorld); // Update Minimap
                        _wvm.UpdateRenderRegion(new Microsoft.Xna.Framework.Rectangle(refX2, miny, refX, maxy));
                        return;
                    }
                    if (paint == 30)
                    {
                        tile++;
                        paint = 0;
                    }
                    else
                    {
                        paint++;
                    }
                }
                refX = x;
            }

            //System.Windows.Forms.MessageBox.Show("Finished.");

        }
        // */

    }
}
