using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using TEdit.Editor.Clipboard;
using TEdit.Geometry.Primitives;
using TEdit.Terraria;
using TEdit.ViewModel;
using TEdit.Editor.PixelArtTools.Tools;
using TEdit.Editor.Tools;
using Microsoft.Win32;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Security.Policy;
using System.Xml.Serialization;
//using SharpDX.Direct2D1.Effects;

namespace TEdit.Editor.Plugins
{
    
    public class PixelArtCreator : BasePlugin, INotifyPropertyChanged
    {
        
        
        public PixelArtCreator(WorldViewModel worldViewModel) : base(worldViewModel)
        {
            Name = "Pixel-Art's Creator";
            _wvm.Plugins.Add(new PaletteCreator(_wvm));  // my
            _wvm.Plugins.Add(new TileDel(_wvm));  // my
        }

        private ClipboardBuffer _generatedSchematic;
        private void getName(string path)
        {
            string[] words = path.Split('\\');
            ArtData.name = words[words.Length - 2];
        }
        public override async void Execute()
        {
            
            PixelArtCreatorPluginSettings view = new PixelArtCreatorPluginSettings();
            if (view.ShowDialog() == false){ return; }
            string Ppath = view.PhotoPath; // tiles art

            if (!Ppath.Contains(".txt") || Ppath == null) { return; }
            ArtData.Photo_path = Ppath;
            getName(Ppath);
            string TorchsPath = view.TorchPath; // tiles art
            if (TorchsPath.Contains(".txt") && TorchsPath != null) { ArtData.SetTorchs(File.ReadAllLines(TorchsPath)); }
            ArtData.ActiveTile = view.CheckBox3.IsChecked ?? false;
            Task Generation = Task.Factory.StartNew(GenerateArtBlocks);
            await Generation;
            AddSchematic();
        }

        private void SetNecesseryArg(string[] array, int count, out int blockORwall, out int tile, out int paint)
        {
            string[] line = array[count + 3].Split(':');
            blockORwall = Convert.ToInt32(line[0]);
            tile = Convert.ToInt32(line[1]);
            paint = Convert.ToInt32(line[2]);
        }
        private void SetTile(Terraria.Tile curtile,int tile_id, bool isTileActive, int paint_id, int ? wall_id = null)
        {
            curtile.Type = (ushort)tile_id;
            curtile.IsActive = true; // Turn on tile
            curtile.InActive = isTileActive;
            curtile.TileColor = (byte)paint_id;
            if (wall_id != null) { curtile.Wall = (ushort)wall_id; }
        }
        private void SetWall(Terraria.Tile curTile, int tile, int paint)
        {
            SetAir(curTile);
            curTile.Wall = (ushort)tile;
            //curtile.Type = (ushort)erase;
            //WorldViewModel.SetPixel(curile, isErase);
            //curTile.IsActive = true; // Turn on tile
                                     //curtile.InActive = TActive;
            curTile.WallColor = (byte)paint;
            //_wvm.UpdateRenderPixel(new Vector2Int32(x, y)); // Update pixel(show on map)
            
            //_wvm.SetPixelAutomaticLink(curTile, tile: -1, u: 0, v: 0);
            


        }
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
        // /*
        private void GenerateArtBlocks()
        {
            //return;
            string[] torchs = (ArtData.Torchs == null) ? ArtData.DefTorchs.ToArray() : ArtData.Torchs.ToArray(); // Путь к факелам
            string[] art = File.ReadAllLines(ArtData.Photo_path); // Арт - ТАйл путь
            string WstAndWstop = art[1];
            string[] subs = WstAndWstop.Split(':');
            int width = Convert.ToInt32(subs[1]);
            int height = Convert.ToInt32(art[2]);
            int wstart = Convert.ToInt32(subs[0]);
            bool TActive = ArtData.ActiveTile;

            Vector2Int32 _generatedSchematicSize = new Vector2Int32(width-wstart, height);
            _generatedSchematic = new(_generatedSchematicSize, true);

            int count = wstart * height;
            for (int x = wstart; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //SetNecesseryArg(art)
                    SetNecesseryArg(art, count, out int blockORwall, out int tile, out int paint);
                    Tile curtile = _generatedSchematic.Tiles[x - wstart, y];
                    if (blockORwall == 1) // tile
                    {
                        if (!torchs.Contains(Convert.ToString(tile)))
                        {
                            // Set necessary paint
                            SetTile(curtile, tile, TActive, paint);
                        }
                        else
                        {
                            // set wall under the torch
                            SetTile(curtile, tile, TActive, paint, 1);
                        }
                    }
                    else if (blockORwall == 0)//wall
                    {
                        SetWall(curtile, tile, paint);
                    }
                    else
                    {
                        SetAir(curtile);
                    }
                    _generatedSchematic.Tiles[x - wstart, y] = curtile;
                    count += 1;
                }

            }

            

        }/*
// */
        public void AddSchematic()
        {
            _generatedSchematic.Name = ArtData.name;
            _generatedSchematic.RenderBuffer();
            _wvm.Clipboard.LoadedBuffers.Add(_generatedSchematic);
            if (_wvm.CurrentWorld == null)
                return;
            _wvm.ClipboardSetActiveCommand.Execute(_generatedSchematic);
        }
    }

    public class ArtData
    {
        #region MyData
        public static string Photo_path,name;
        public static List<string> DefTorchs { get; private set; } = new List<string> { "4", "136", "557", "429", "424", "423", "420" };
        public static List<string> DefExceptionsTiles { get; private set; } = new List<string> {"3", "5", "10", "11",
                                                                                "12", "13", "14", "15",
                                                                                "16", "17", "18", "20",
                                                                                "21", "24", "26", "27",
                                                                                "28", "29", "31", "33",
                                                                                "34", "35", "36", "42",
                                                                                "49", "50", "55", "61",
                                                                                "71", "72", "73", "74",
                                                                                "77", "78", "79", "81",
                                                                                "82", "83", "84", "85",
                                                                                "86", "87", "88", "89",
                                                                                "90", "91", "92", "93",
                                                                                "94", "95", "96", "97",
                                                                                "98", "99", "100", "191",
                                                                                "102", "103", "104", "105",
                                                                                "106", "110", "113", "114",
                                                                                "125", "126", "128", "129",
                                                                                "132", "133", "134", "135",
                                                                                "137", "138", "139", "141",
                                                                                "142", "143", "144", "149",
                                                                                "165", "171", "172", "173",
                                                                                "174", "178", "184", "185",
                                                                                "186", "187", "201", "207",
                                                                                "209", "210", "212", "215",
                                                                                "216", "217", "218", "219",
                                                                                "220", "227", "228", "231",
                                                                                "233", "235", "236", "237",
                                                                                "238", "239", "240", "241",
                                                                                "242", "243", "244", "245",
                                                                                "246", "247", "254", "269",
                                                                                "270", "271", "275", "276",
                                                                                "277", "278", "279", "280",
                                                                                "281", "282", "283", "285",
                                                                                "286", "287", "288", "289",
                                                                                "290", "291", "292", "293",
                                                                                "294", "295", "296", "297",
                                                                                "298", "299", "300", "301",
                                                                                "302", "303", "304", "305",
                                                                                "306", "307", "308", "309",
                                                                                "310", "314", "316", "317",
                                                                                "318", "319", "320", "323",
                                                                                "324", "334", "335", "337",
                                                                                "338", "339", "349", "354",
                                                                                "355", "356", "358", "359",
                                                                                "360", "361", "362", "363",
                                                                                "364", "372", "373", "374",
                                                                                "375", "376", "377", "378",
                                                                                "380", "386", "387", "388",
                                                                                "389", "390", "391", "392",
                                                                                "393", "394", "395", "405",
                                                                                "406", "410", "411", "412",
                                                                                "413", "414", "419", "425",
                                                                                "527", "428", "440", "441",
                                                                                "442", "443", "444", "452",
                                                                                "453", "454", "455", "456",
                                                                                "457", "461", "462", "463",
                                                                                "464", "465", "466", "467",
                                                                                "468", "469", "470", "471",
                                                                                "475", "476", "480", "484",
                                                                                "485", "486", "487", "488",
                                                                                "489", "490", "491", "493",
                                                                                "494", "497", "499", "505",
                                                                                "506", "509", "510", "511",
                                                                                "518", "519", "520", "521",
                                                                                "522", "523", "524", "525",
                                                                                "526", "527", "529", "530",
                                                                                "531", "532", "533", "538",
                                                                                "542", "543", "544", "545",
                                                                                "547", "548", "549", "550",
                                                                                "551", "552", "553", "554",
                                                                                "555", "556", "558", "559",
                                                                                "560", "564", "565", "567",
                                                                                "568", "569", "570", "571",
                                                                                "572", "573", "579", "580",
                                                                                "581", "582", "583", "584",
                                                                                "585", "586", "587", "588",
                                                                                "589", "590", "591", "592",
                                                                                "593", "594", "595", "596",
                                                                                "597", "598", "599", "600",
                                                                                "601", "602", "603", "604",
                                                                                "605", "606", "607", "608",
                                                                                "609", "610", "611", "612",
                                                                                "613", "614", "615", "616",
                                                                                "617", "619", "620", "621",
                                                                                "622", "623", "624", "629",
                                                                                "630", "631", "632", "634",
                                                                                "637", "639", "640", "642",
                                                                                "643", "644", "645", "646",
                                                                                "647", "648", "649", "650",
                                                                                "651", "652", "653", "654",
                                                                                "656", "657", "658", "660",
                                                                                "663", "664", "665", "127",
                                                                                "52", "53", "112", "116",
                                                                                "234", "224", "123", "330",
                                                                                "331", "332", "333", "51",
                                                                                "52", "62", "115", "205",
                                                                                "382", "528", "636", "638",
                                                                                "32", "69", "352", "655",
                                                                                "80", "101", "124", "179",
                                                                                "180", "181", "182", "183",
                                                                                "366", "381", "449", "450",
                                                                                "451", "481", "482", "483",
                                                                                "504", "512", "513", "514",
                                                                                "515", "516", "517", "546",
                                                                                "574", "575", "576", "577",
                                                                                "578", "56", "495", "692",
                                                                                "160", "627", "628", "541" };
        public static List<string> DefExceptionsWalls { get; private set; } = new List<string> {"168","169"};
        public static class Exceptions
        {
            public static List<string> Tiles, Walls;
        }
        public static List<string> Torchs { get; private set; }
        public static bool ActiveTile { get; set; }
        #endregion
        #region Functions
        public static void InvertExeption(int TileCount, int WallCount)
        {

            string[] ExTiles = ArtData.Exceptions.Tiles.ToArray();
            Array.Sort(ExTiles);
            string[] ExWalls = ArtData.Exceptions.Walls.ToArray();
            Array.Sort(ExWalls);
            List<string> ExTilesReverse = new List<string>();
            List<string> ExWallsReverse = new List<string>();
            for (int i = 0; i < TileCount; i++)
            {
                if (ExTiles.Contains($"{i}"))
                {
                    continue;
                }
                ExTilesReverse.Add($"{i}");
            }
            for (int i = 1; i < WallCount; i++)
            {
                if (ExWalls.Contains($"{i}"))
                {
                    continue;
                }
                ExWallsReverse.Add($"{i}");
            }
            ArtData.Exceptions.Tiles = ExTilesReverse;
            ArtData.Exceptions.Walls = ExWallsReverse;
        }
        public static void AddDefault()
        {
            // If you don't add these exceptions, your application may crash.
            #region Necessarily
            ArtData.Exceptions.Tiles.Add(((int)TileType.Chest).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.Chest2).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.Dresser).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.TrappedChest2).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.TrappedChest).ToString());
            #endregion
            #region Optional
            ArtData.Exceptions.Tiles.Add(((int)TileType.MannequinLegacy).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.WomannequinLegacy).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.FoodPlatter).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.TrainingDummy).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.ItemFrame).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.WeaponRackLegacy).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.WeaponRack).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.HatRack).ToString());
            ArtData.Exceptions.Tiles.Add(((int)TileType.TeleportationPylon).ToString());
            #endregion

            // Distinct 
            ArtData.Exceptions.Tiles.Distinct().ToList();
            ArtData.Exceptions.Walls.Distinct().ToList();
        }
        private bool guessAnActive()
        {
            // MSG Box - Inactive or Active Tiles 
            // Initializes the variables to pass to the MessageBox.Show method.
            string message = "Disabling blocks when creating \"art\" will allow all blocks to be displayed on the map. This is convenient if the \"art\" is large, or if it is created in a new undeveloped area.";
            string caption = "Turn off blocks in art?";
            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
            System.Windows.Forms.DialogResult result;

            // Displays the MessageBox.
            result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string FindFile()
        {
            // OpenFileDialog
            return PathManager.OpenFilePathDialog();
        }
        public static void SetExceptions(string[] NotDefExceptions, bool param = false)
        {
            List<string> Tiles = new List<string>();
            List<string> Walls = new List<string>();
            string[] item;
            foreach (var elem in NotDefExceptions)
            {
                item = elem.Split(':');
                if (item[0] == "1") { Tiles.Add(item[1]); } else { Walls.Add(item[1]); }
            }
            SetExceptionsWall(Walls, param);
            SetExceptionsTile(Tiles, param);
        }
        /// <summary>
        /// NotDefWalls - New ex file from user
        /// if param == false - Combine Exceptions, else Set new
        /// </summary>
        public static void SetExceptionsWall(List<string> NotDefWalls, bool param = false)
        {
            
            if (param == false)
            {
                Exceptions.Walls = DefExceptionsWalls.Concat(NotDefWalls).ToList();
                return;
            }
            Exceptions.Walls = NotDefWalls.ToList();
            return;
        }
        /// <summary>
        /// NotDefTiles - New ex file from user
        /// if param == false - Combine Exceptions, else Set new
        /// </summary>
        public static void SetExceptionsTile(List<string> NotDefTiles, bool param = false)
        {
            
            if (param == false)
            {
                Exceptions.Tiles = DefExceptionsTiles.Concat(NotDefTiles).ToList();
                return;
            }
            Exceptions.Tiles = NotDefTiles.ToList();
            return;
        }
        /// <summary>
        /// NotDefExceptions - New ex file from user
        /// if param == false - Combine Exceptions, else Set new
        /// </summary>
        public static void SetTorchs(string[] UserTorchs, bool param = false)
        {
            
            if (param == false)
            {
                Torchs = DefTorchs.Concat(UserTorchs).ToList();
                return;
            }
            Torchs = UserTorchs.ToList();
            return;
        }

        #endregion
    }
}
