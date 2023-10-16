using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEdit.ViewModel;
using TEdit.Terraria;
using Microsoft.Xna.Framework;
using TEdit.Editor.Tools;

namespace TEdit.Editor.Plugins
{
    internal class TileDel : BasePlugin
    {
        
        private void SetAir(Terraria.Tile curTile)
        {
            curTile.Type = 0;
            curTile.IsActive = false;
            curTile.InActive = false;
            curTile.Actuator = false;
            curTile.BrickStyle = BrickStyle.Full;
            curTile.U = 0;
            curTile.V = 0;
        }

        public TileDel(WorldViewModel worldViewModel) : base(worldViewModel)
        {
            Name = "Delete far Tiles";
        }
        private bool IsNUmber(string str)
        {
            if (str == "") return false;
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str, i)) { return false; }
            }
            return true;
        }
        public override void Execute()
        {
            if (_wvm.CurrentWorld == null)
                return;
            TileRemoverUI view = new TileRemoverUI();
            if (view.ShowDialog() == false) { return; }
            string amount = view.amount;
            int FT = IsNUmber(amount) ? Convert.ToInt32(amount) : 50; ;
            RemoveFarTiles(FT);

        }
        public void RemoveFarTiles(int FarTile)
        {
            int minx, miny, maxx, maxy;
            minx = miny = FarTile;
            maxx = _wvm.CurrentWorld.TilesWide - FarTile < _wvm.CurrentWorld.TilesWide/2 ? _wvm.CurrentWorld.TilesWide / 2 : _wvm.CurrentWorld.TilesWide - FarTile;
            maxy = _wvm.CurrentWorld.TilesHigh - FarTile < _wvm.CurrentWorld.TilesHigh/2 ? _wvm.CurrentWorld.TilesHigh / 2 : _wvm.CurrentWorld.TilesHigh - FarTile;
            #region Tutor1
            /*
             * --------------
             * |            |
             * |            |
             * --------------
             * #xxxxxxxxxxxx#
             * #            #
             * #            #
             * #xxxxxxxxxxxx#
             * 
             * # = remove
             */
            #endregion
            Tile curtile = new Tile();
            List<Rectangle> reclist = new List<Rectangle>();
            for (int y = 0; y < _wvm.CurrentWorld.TilesHigh; y++)
            {
                for (int x = 0; x < minx; x++)
                {
                    //Tile curtile = _wvm.CurrentWorld.Tiles[x, y];
                    SetAir(curtile);
                    _wvm.CurrentWorld.Tiles[x, y] = curtile;
                }

                for (int x = maxx; x < _wvm.CurrentWorld.TilesWide; x++)
                {
                    //Tile curtile = _wvm.CurrentWorld.Tiles[x, y];
                    SetAir(curtile);
                    _wvm.CurrentWorld.Tiles[x, y] = curtile;
                }
            }

            #region Tutor2
            /*
             * --------------
             * |            |
             * |            |
             * --------------
             * x############x
             * x            x
             * x            x
             * x############x
             * 
             * # = remove
             */
            #endregion
            for (int x = minx; x < maxx; x++)
            {
                for (int y = 0; y < miny; y++)
                {
                    //Tile curtile = _wvm.CurrentWorld.Tiles[x, y];
                    SetAir(curtile);
                    _wvm.CurrentWorld.Tiles[x, y] = curtile;
                }

                for (int y = maxy; y < _wvm.CurrentWorld.TilesHigh; y++)
                {
                    //Tile curtile = _wvm.CurrentWorld.Tiles[x, y];
                    SetAir(curtile);
                    _wvm.CurrentWorld.Tiles[x, y] = curtile;

                }
                
            }
            reclist.Add(new Rectangle(0, 0, minx, _wvm.CurrentWorld.TilesHigh));
            reclist.Add(new Rectangle(maxx, 0, _wvm.CurrentWorld.TilesWide, _wvm.CurrentWorld.TilesHigh));
            reclist.Add(new Rectangle(minx, 0, maxx, miny));
            reclist.Add(new Rectangle(minx, maxy, maxx, _wvm.CurrentWorld.TilesHigh));
            foreach (var rectangle in reclist)
            {
                _wvm.UpdateRenderRegion(rectangle);
            }
            _wvm.MinimapImage = Render.RenderMiniMap.Render(_wvm.CurrentWorld); // Update Minimap


        }
    }
}
