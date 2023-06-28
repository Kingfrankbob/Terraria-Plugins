using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;


namespace CTG
{
    [ApiVersion(2, 1)]
    public class CTG : TerrariaPlugin
    {
        public override string Name => "Capture The Gem plugin!!!";
        public Dictionary<string, int> playerChoice = new Dictionary<string, int>();
        public List<TSPlayer> Lobby = new List<TSPlayer>();
        public List<TSPlayer> GameA = new List<TSPlayer>();
        public List<TSPlayer> Red = new List<TSPlayer>();
        public List<TSPlayer> Blue = new List<TSPlayer>();
        public (string, bool)? redGem = null;
        public (string, bool)? blueGem = null;
        public bool redLaunch = false;
        public bool blueLaunch = false;
        public DateTime time;
        public DateTime bluetime;
        public DateTime every5;
        public bool changeTime = false;
        public enum classes { Warrior, Tree, Ancient_One, Archer, Mage, Wandering, Wayne };
        public CTG(Terraria.Main game) : base(game)
        {

        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, WorldLoaded);
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
            GetDataHandlers.PlayerTeam += PTCH;
            PlayerHooks.PlayerPostLogin += OnPlayerPostLogin;
            ServerApi.Hooks.GameUpdate.Register(this, OnGameAUpdate);
            ServerApi.Hooks.GameUpdate.Register(this, OnGameAUpdateTwice);
            ServerApi.Hooks.GameUpdate.Register(this, forFireWorkLaunchRed);
            ServerApi.Hooks.GameUpdate.Register(this, forFireWorkLaunchBlue);
            ServerApi.Hooks.GameUpdate.Register(this, timeChanger);
        }
        private void WorldLoaded(EventArgs args)
        {
            Commands.ChatCommands.Add(new Command("CTG.j", join, "j", "join", "J"));
            Commands.ChatCommands.Add(new Command("CTG.start", start, "s", "start", "Start"));
            //Commands.ChatCommands.Add(new Command("CTG.gp", getPos, "gp", "g", "getpos"));
            Commands.ChatCommands.Add(new Command("CTG.cl", setClass, "class"));
            //Commands.ChatCommands.Add(new Command("CTG.rt", reset, "reset", "rs", "rt"));
            Commands.ChatCommands.Add(new Command("CTG.co", classOnea, "classone", "co"));
            //Commands.ChatCommands.Add(new Command("CTG.cf", clearFiftyeight, "cf"));
            //Commands.ChatCommands.Add(new Command("CTG.gi", giveItem, "givei", "gi", "giveitem"));
        }
        //private void giveItem(CommandArgs args)
        //{
        //    if (args.Parameters.Count > 1)
        //    {
        //        args.Player.SendErrorMessage(
        //            "Invalid syntax. Proper syntax: {0}give <item type/id> [item amount] [prefix id/name]");
        //        return;
        //    }
        //    if (args.Parameters[0].Length == 0)
        //    {
        //        args.Player.SendErrorMessage( ("Missing item name/id."));
        //        return;
        //    }
        //    int itemAmount = 0;
        //    int prefix = 0;
        //    var items = TShock.Utils.GetItemByIdOrName(args.Parameters[0]);
        //    args.Parameters.RemoveAt(0);
        //    string plStr = args.Player.Name;
        //    if (args.Parameters.Count == 1)
        //        int.TryParse(args.Parameters[0], out itemAmount);
        //    if (items.Count == 0)
        //    {
        //        args.Player.SendErrorMessage( ("Invalid item type!"));
        //    }
        //    else if (items.Count > 1)
        //    {
        //        args.Player.SendMultipleMatchError(items.Select(i => $"{i.Name}({i.netID})"));
        //    }
        //    else
        //    {
        //        var item = items[0];

        //        if (args.Parameters.Count == 2)
        //        {
        //            int.TryParse(args.Parameters[0], out itemAmount);
        //            var prefixIds = TShock.Utils.GetPrefixByIdOrName(args.Parameters[1]);
        //            if (item.accessory && prefixIds.Contains(PrefixID.Quick))
        //            {
        //                prefixIds.Remove(PrefixID.Quick);
        //                prefixIds.Remove(PrefixID.Quick2);
        //                prefixIds.Add(PrefixID.Quick2);
        //            }
        //            else if (!item.accessory && prefixIds.Contains(PrefixID.Quick))
        //                prefixIds.Remove(PrefixID.Quick2);
        //            if (prefixIds.Count == 1)
        //                prefix = prefixIds[0];
        //        }

        //        if (item.type >= 1 && item.type < Terraria.ID.ItemID.Count)
        //        {
        //            var players = TSPlayer.FindByNameOrID(plStr);
        //            if (players.Count == 0)
        //            {
        //                args.Player.SendErrorMessage( ("Invalid player!"));
        //            }
        //            else if (players.Count > 1)
        //            {
        //                args.Player.SendMultipleMatchError(players.Select(p => p.Name));
        //            }
        //            else
        //            {
        //                var plr = players[0];
        //                if (plr.InventorySlotAvailable || (item.type > 70 && item.type < 75) || item.ammo > 0 || item.type == 58 || item.type == 184)
        //                {
        //                    if (itemAmount == 0 || itemAmount > item.maxStack)
        //                        itemAmount = item.maxStack;
        //                    //plr.GiveItemCheck(item.type, EnglishLanguage.GetItemNameById(item.type), itemAmount, prefix);

        //                    args.Player.GiveItem(item.type, itemAmount, prefix);


        //                        args.Player.SendSuccessMessage("Gave " + itemAmount +  + ' ' + item.Name + " to " + plr.Name);
        //                }
        //                else
        //                {
        //                    args.Player.SendErrorMessage( ("Player does not have free slots!"));
        //                }
        //            }
        //        }
        //        else
        //        {
        //            args.Player.SendErrorMessage( ("Invalid item type!"));
        //        }
        //    }
        //}
        private async void OnGameAUpdate(EventArgs args)
        {
            foreach (var player in TShock.Players)
            {
                if (player == null) continue;
                if (player.IsLoggedIn && player.TPlayer.dead)
                {
                    if (player.Team == 1)
                    {
                        player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                        if (blueGem != null)
                        {
                            if (player.Name == blueGem.Value.Item1)
                            {
                                blueGem = null;
                                TShock.Utils.Broadcast(player.Name + " has dropped the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                            }
                        }
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        player.Teleport((float)36853.426, 4982, 2);
                    }
                    else if (player.Team == 3)
                    {
                        if (redGem != null)
                        {
                            if (player.Name == redGem.Value.Item1)
                            {
                                redGem = null;
                                TShock.Utils.Broadcast(player.Name + " has dropped the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);
                            }
                        }
                        player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        player.Teleport((float)30359, 4982, 2);
                    }
                }
            }
        }
        public void join(CommandArgs args)
        {
            if (args.Parameters[0] == null) return;
            else if (args.Parameters[0] == "CTG" || args.Parameters[0] == "ctg" || args.Parameters[0] == "Ctg")
            {
                TSPlayer player = args.Player;
                if (player != null)
                {
                    //var group = TShock.Groups.GetGroupByName("CTGLOB");
                    //if (group != null)
                    //{
                    //    // Add the player to the temporary group
                    //    player.Group = group;
                    //}
                    if (!GameA.Contains(player))  Lobby.Add(player); 
                    
                }
            }
            else
            {
                args.Player.SendErrorMessage("Invalid Option, please enter a valid option!");
            }
        }
        private void OnPlayerPostLogin(PlayerPostLoginEventArgs args)
        {
            var player = FindPlayerByName(args.Player.Name);
            if (player.Name == "kingfrankbob" || player.Name == "Kingfrankbob" || player.Name == "Roses" || player.Name == "roses") return;
            if (player != null)
            {
                player.SendInfoMessage("Welcome back to the server!");
                player.SendInfoMessage("Please enjoy your stay! Find games to play, and give feedback!");
                player.Group = TShock.Groups.GetGroupByName("default");
                clearInventory(player);
                updateSlot(player, 62, 4954);
                updateSlot(player, 63, 4989);
                updateSlot(player, 0, 4344, true, 20);
                updateSlot(player, 1, 1000, true, 20);
            }
        }
        public void setClass(CommandArgs args) //CLASS SELECTIONS, 'random' Working
        {
            if (playerChoice.ContainsKey(args.Player.Name))
                playerChoice.Remove(args.Player.Name);
            if (args.Parameters[0] == null || args.Parameters == null)
            {
                args.Player.SendErrorMessage("Please give a class!");
                return;
            }
            if (args.Parameters[0] == "random")
            {
                args.Player.SendMessage("Random Class selected!", Microsoft.Xna.Framework.Color.Yellow);
                var random = new Random();
                playerChoice.Add(args.Player.Name, random.Next(1, 8));
            }
            else if (Int16.Parse(args.Parameters[0]) > 0 && Int16.Parse(args.Parameters[0]) < 8 || Int16.Parse(args.Parameters[0]) == 3559)
            {

                if (Int16.Parse(args.Parameters[0]) != 3559 || Int16.Parse(args.Parameters[0]) != 69) { args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + (classes)(Int16.Parse(args.Parameters[0]) - 1) + "!!!", Microsoft.Xna.Framework.Color.MediumPurple); playerChoice.Add(args.Player.Name, Int16.Parse(args.Parameters[0])); }
                else if (Int16.Parse(args.Parameters[0]) == 3559 && (args.Player.Group.Name == "superadmin" || args.Player.Group.Name == "owner") || args.Player.Name == "aerg" || args.Player.Name == "Edjar" || args.Player.Name == "kingfrankbob") { args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + "Developer Class [MC]", Microsoft.Xna.Framework.Color.MediumPurple); playerChoice.Add(args.Player.Name, Int16.Parse(args.Parameters[0])); }
                else if (Int16.Parse(args.Parameters[0]) == 3559 && !(args.Player.Group.Name == "superadmin" || args.Player.Group.Name == "owner")) args.Player.SendMessage("You have not chosen class " + args.Parameters[0] + " ~ " + "Developer Class [MC]", Microsoft.Xna.Framework.Color.MediumPurple);
                else if (Int16.Parse(args.Parameters[0]) == 69) args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + "John and Evan Class [JNER]", Microsoft.Xna.Framework.Color.MediumPurple);
            }
            else
            {
                args.Player.SendErrorMessage("Invalid Syntax, please try: /class <number> between 1 and 7");
            }
        }
        private TSPlayer FindPlayerByName(string playerName)//FIND PLAYER BY NAME, DONT KNOW HOW ELSE TO EXPLAIN XD
        {
            foreach (TSPlayer player in TShock.Players)
            {
                if (player?.Name == playerName)
                {
                    return player;
                }
            }
            return null;
        }
        public async void start(CommandArgs args)
        {
            if (Lobby.Count < 2)
            {
                TShock.Utils.Broadcast("Not enough players!", Microsoft.Xna.Framework.Color.Yellow);
                return;
            }
            playerChoice.Clear();
            var playercount = 0;
            foreach(var player in Lobby)
            {
                GameA.Add(player);
                ++playercount;
            }
            Lobby.Clear();


            int sourceX = 161, sourceY = 315, destX = 1949, destY = 288, width = 304, height = 41;
            for (int x = sourceX; x < sourceX + width; ++x)
            {
                for (int y = sourceY; y < sourceY + height; ++y)
                {
                    var sourceTile = Terraria.Main.tile[x, y];
                    if (sourceTile.type == 54)
                        sourceTile = new Terraria.Tile();

                    var destTile = new Terraria.Tile();

                    destTile.CopyFrom(sourceTile);
                    destTile.CopyPaintAndCoating(sourceTile);

                    Terraria.Main.tile[destX + (x - sourceX), destY + (y - sourceY)].CopyFrom(destTile);
                    Terraria.Main.tile[destX + (x - sourceX), destY + (y - sourceY)].CopyPaintAndCoating(destTile);

                }
            }
            // 1949, 328 start   41 TALL  304 WIDE  TO PASTE
            //copy from 161,355   Same Dimensions
            TSPlayer.All.SendTileRect((short)destX, (short)destY, 100, 41);
            TSPlayer.All.SendTileRect((short)(destX + 100), (short)(destY), 100, 41);
            TSPlayer.All.SendTileRect((short)(destX + 200), (short)(destY), 100, 41);
            TSPlayer.All.SendTileRect((short)(destX + 300), (short)(destY), 4, 41);

            var perteam = (int)playercount / 2;
            var randomPlayers = new List<int>();
            var random = new Random();

            do
            {
                var next = random.Next(perteam);
                if (randomPlayers.Contains(next)) continue;
                randomPlayers.Add(next);

            } while (randomPlayers.Count < perteam);

            playercount = 0;
            var counter = 0;
            foreach (var player in TShock.Players)
            {
                try
                {
                    if (player != null && player.Active)
                    {
                        if (GameA.Contains(player) && randomPlayers.Contains(counter))
                        {
                            Red.Add(player);
                            player.SetTeam(1);
                            player.SetPvP(true);
                            ++playercount;
                            player.Teleport((float)30741.84, 10534, 3);
                            clearInventory(player);
                        }
                        else if (GameA.Contains(player))
                        {
                            Blue.Add(player);
                            player.SetTeam(3);
                            player.SetPvP(true);
                            player.Teleport((float)30131.447, 10534, 2);
                            clearInventory(player);
                        }
                        //RED POS CLASS: X:30769.568  Y:10230
                        //BLUE POS CLASS: X:30131.447 Y:10534
                    }
                }
                catch (Exception) { }
                ++counter;
            }

            TShock.Utils.Broadcast("You have 15 Seconds to choose your class!", Microsoft.Xna.Framework.Color.Yellow);
            TShock.Utils.Broadcast("Good Luck!", Microsoft.Xna.Framework.Color.Green);
            TShock.Groups.GetGroupByName("default").AddPermission("CTG.cl");
            await Task.Delay(TimeSpan.FromSeconds(15));
            TShock.Groups.GetGroupByName("default").RemovePermission("CTG.cl");
            TShock.Utils.Broadcast("GameA Starting, Good Luck", Microsoft.Xna.Framework.Color.Yellow);
            foreach (var player in TShock.Players)
            {
                if (player == null) continue;
                if (GameA.Contains(player) && playerChoice.ContainsKey(player.Name))
                {
                    player.SendMessage("You have chosen class " + playerChoice[player.Name] + "!", Microsoft.Xna.Framework.Color.BlueViolet);
                    setClassItem(player);
                    if (player.Team == 1)
                    {
                        player.Teleport((float)36853.426, 4982, 2);
                        player.sX = (int)(36853.426 / 16);
                        player.sY = (4982 / 16);
                    }
                    if (player.Team == 3)
                    {

                        player.Teleport((float)30359, 4982, 2);
                        player.sX = (int)(30359.117 / 16);
                        player.sY = (4982 / 16);
                    }
                }
                else if (GameA.Contains(player))
                {
                    var choice = random.Next(1, 7);
                    player.SendMessage("You have chosen class " + choice + "!", Microsoft.Xna.Framework.Color.BlueViolet);
                    setClassItem(player);
                    if (player.Team == 1)
                    {
                        player.Teleport((float)36853.426, 4982, 2);
                        player.sX = (int)(36853.426 / 16);
                        player.sY = (4982 / 16);
                    }
                    if (player.Team == 3)
                    {
                        player.Teleport((float)30359, 4982, 2);
                        player.sX = (int)(30359.117 / 16);
                        player.sY = (4982 / 16);
                    }
                }
                //RED SPAWN: X: 36853.426  Y: 4982
                //BLUE SPAWN X: 30359.117  Y: 4982
                else
                {
                    await Console.Out.WriteLineAsync("Not able to happen?!");
                }
            }


        }
        private void setClassItem(TSPlayer player)
        {
            if (player == null) return;
            if (!playerChoice.ContainsKey(player.Name))
            {
                var random = new Random();
                playerChoice.Add(player.Name, random.Next(1, 7));
            }
            switch (playerChoice[player.Name])
            {
                case 1:
                    classOne(player);
                    break;
                case 2:
                    classTwo(player);
                    break;
                case 3:
                    classThree(player);
                    break;
                case 4:
                    classFour(player);
                    break;
                case 5:
                    classFive(player);
                    break;
                case 6:
                    classSix(player);
                    break;
                case 7:
                    classSeven(player);
                    break;
                case 3559:
                    class3559(player);
                    break;
                case 69:
                    class69(player);
                    break;
            }
          return;
        }
        private void classOne(TSPlayer player, bool ondeath = false) // CLASS ONE MELEE, CRIMSON ARMOR, LEAD BROADSWORD 
        {
            for (int i = 0; i < 80; i++)
            {
                updateSlot(player, i, ItemID.None);
            }
            player.TPlayer.statLifeMax = 200;
            player.TPlayer.statLife = 200;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            updateSlot(player, 59, ItemID.CrimsonHelmet);
            updateSlot(player, 60, ItemID.CrimsonScalemail);
            updateSlot(player, 61, ItemID.CrimsonGreaves);
            updateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 42);
            updateSlot(player, 63, ItemID.PaintSprayer, prefix: 42);
            updateSlot(player, 64, ItemID.FrogLeg, prefix: 72);
            updateSlot(player, 65, ItemID.CobaltShield, prefix: 72);
            updateSlot(player, 66, ItemID.SweetheartNecklace, prefix: 72);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            updateSlot(player, 0, 5294);
            updateSlot(player, 1, 1320);
            updateSlot(player, 2, ItemID.WhoopieCushion);
            if (!ondeath) updateSlot(player, 3, ItemID.DirtBlock, true, 999);

            if (player.Team == 1)
                updateSlot(player, 49, ItemID.DeepRedPaint, true, 999);
            else if (player.Team == 3)
                updateSlot(player, 49, ItemID.BluePaint, true, 999);
            else
                updateSlot(player, 49, ItemID.WhitePaint, true, 999);

            return;
        }
        private void classTwo(TSPlayer player, bool ondeath = false)// TREE CLASS
        {
            player.TPlayer.statLifeMax = 160;
            player.TPlayer.statLife = 160;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            clearInventory(player);
            applyDye(player);
            updateSlot(player, 0, ItemID.WandofSparking);
            updateSlot(player, 1, 1320);
            updateSlot(player, 2, ItemID.WhoopieCushion);
            updateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            updateSlot(player, 59, ItemID.WoodHelmet);
            updateSlot(player, 60, ItemID.WoodBreastplate);
            updateSlot(player, 61, ItemID.WoodGreaves);
            updateSlot(player, 62, ItemID.LuckyHorseshoe);
            updateSlot(player, 63, ItemID.PaintSprayer);
            updateSlot(player, 64, ItemID.FrogLeg);
            updateSlot(player, 65, ItemID.CharmofMyths);
            updateSlot(player, 66, ItemID.SorcererEmblem);
            updateSlot(player, 69, ItemID.TreeMask);
            updateSlot(player, 70, ItemID.TreeShirt);
            updateSlot(player, 71, ItemID.TreeTrunks);
            updateSlot(player, 93, ItemID.WebSlinger);
            updateSlot(player, 90, 4813);
        }
        private void class3559(TSPlayer player, bool ondeath = false)// TREE CLASS
        {
            clearInventory(player);
            player.TPlayer.statLifeMax = 500;
            player.TPlayer.statLife = 500;
            updateSlot(player, 0, 4956); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            updateSlot(player, 1, ItemID.ShroomiteDiggingClaw);
            updateSlot(player, 2, ItemID.WhoopieCushion);
            updateSlot(player, 3, ItemID.LihzahrdBrick, true, 9999);
            updateSlot(player, 4, ItemID.RodOfHarmony);
            updateSlot(player, 5, ItemID.DrillContainmentUnit);
            updateSlot(player, 59, ItemID.SolarFlareHelmet);
            updateSlot(player, 60, ItemID.SolarFlareBreastplate);
            updateSlot(player, 61, ItemID.SolarFlareLeggings);
            updateSlot(player, 62, 4954);
            updateSlot(player, 63, 4989);
            updateSlot(player, 64, ItemID.BerserkerGlove);
            updateSlot(player, 65, ItemID.TerrasparkBoots);
            updateSlot(player, 66, ItemID.CelestialShell);
            updateSlot(player, 93, ItemID.LunarHook);
            updateSlot(player, 90, 4813);
        }
        private void class69(TSPlayer player)
        {

        }
        private void classThree(TSPlayer player, bool ondeath = false)// ANCIENT CLASS
        {
            player.TPlayer.statLife = 120;
            player.TPlayer.statLifeMax = 120;
            clearInventory(player); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            applyDye(player);
            updateSlot(player, 0, 4913);
            updateSlot(player, 1, 1320, prefix: 15);
            updateSlot(player, 2, ItemID.WhoopieCushion);
            updateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            updateSlot(player, 59, ItemID.BeeHeadgear);
            updateSlot(player, 60, ItemID.BeeBreastplate);
            updateSlot(player, 61, ItemID.BeeGreaves);
            updateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 76);
            updateSlot(player, 63, ItemID.PaintSprayer, prefix: 76);
            updateSlot(player, 64, ItemID.FrogLeg, prefix: 68);
            updateSlot(player, 65, 3097, prefix: 80);
            updateSlot(player, 66, ItemID.BlizzardinaBottle);
            updateSlot(player, 69, 3773);
            updateSlot(player, 70, 3774);
            updateSlot(player, 71, 3775);
            updateSlot(player, 90, ItemID.ExoticEasternChewToy);
        }
        private void classFour(TSPlayer player, bool ondeath = false)// ARCHER CLASS
        {
            clearInventory(player);
            applyDye(player);
            player.TPlayer.statLife = 180;
            player.TPlayer.statLifeMax = 180;
            updateSlot(player, 0, ItemID.DemonBow); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            updateSlot(player, 1, 1320);
            updateSlot(player, 2, ItemID.WhoopieCushion);
            updateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            updateSlot(player, 59, ItemID.NecroHelmet);
            updateSlot(player, 60, ItemID.NecroBreastplate);
            updateSlot(player, 61, ItemID.NecroGreaves);
            updateSlot(player, 62, ItemID.LuckyHorseshoe);
            updateSlot(player, 63, ItemID.PaintSprayer);
            updateSlot(player, 64, ItemID.FrogLeg);
            updateSlot(player, 65, ItemID.CloudinaBottle);
            updateSlot(player, 54, ItemID.HellfireArrow, true, 9999);
            updateSlot(player, 69, ItemID.HerosHat);
            updateSlot(player, 70, ItemID.HerosShirt);
            updateSlot(player, 71, ItemID.HerosPants);
            updateSlot(player, 90, ItemID.FairyBell);
        }
        private void classFive(TSPlayer player, bool ondeath = false)// MAGE CLASS
        {
            clearInventory(player);
            applyDye(player);
            player.TPlayer.statLife = 120;
            player.TPlayer.statLifeMax = 120;
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            updateSlot(player, 0, ItemID.Vilethorn);
            updateSlot(player, 1, ItemID.IceRod);
            updateSlot(player, 2, 1320);
            updateSlot(player, 4, ItemID.DirtBlock, true, 9999);
            updateSlot(player, 3, ItemID.WhoopieCushion);
            updateSlot(player, 59, ItemID.WizardHat);
            updateSlot(player, 60, ItemID.AmberRobe);
            updateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 65);
            updateSlot(player, 63, ItemID.PaintSprayer, prefix: 65);
            updateSlot(player, 64, ItemID.FrogLeg, prefix: 74);
            updateSlot(player, 65, ItemID.FrozenShield, prefix: 68);
            updateSlot(player, 66, ItemID.CelestialStone, prefix: 68);
            updateSlot(player, 69, ItemID.RuneHat);
            updateSlot(player, 70, ItemID.RuneRobe);
            updateSlot(player, 90, 4809);
        }
        private void classSix(TSPlayer player, bool ondeath = false)// WANDERING NINJA CLASS
        {
            clearInventory(player);
            applyDye(player);
            player.TPlayer.statLife = 200;
            player.TPlayer.statLifeMax = 200;
            updateSlot(player, 0, ItemID.Katana); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            updateSlot(player, 1, 1320);
            updateSlot(player, 2, ItemID.WhoopieCushion);
            updateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            updateSlot(player, 59, ItemID.AncientShadowHelmet);
            updateSlot(player, 60, ItemID.AncientShadowScalemail);
            updateSlot(player, 61, ItemID.AncientShadowGreaves);
            updateSlot(player, 62, ItemID.LuckyHorseshoe);
            updateSlot(player, 63, ItemID.PaintSprayer);
            updateSlot(player, 64, ItemID.FrogLeg);
            updateSlot(player, 65, ItemID.SharkToothNecklace);
            updateSlot(player, 66, ItemID.FleshKnuckles);
            updateSlot(player, 69, 5048);
            updateSlot(player, 70, 5049);
            updateSlot(player, 71, 5050);
            updateSlot(player, 90, ItemID.BambooLeaf);
        }
        private void classSeven(TSPlayer player, bool ondeath = false)// WAYNE CLASS
        {
            clearInventory(player);
            applyDye(player);
            player.TPlayer.statLife = 220;
            player.TPlayer.statLifeMax = 220;
            updateSlot(player, 0, 3351, prefix: 81); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            updateSlot(player, 1, 1320);
            updateSlot(player, 2, ItemID.WhoopieCushion);
            updateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            updateSlot(player, 59, ItemID.MagicHat);
            updateSlot(player, 60, ItemID.Gi);
            updateSlot(player, 61, ItemID.OrichalcumLeggings);
            updateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 68);
            updateSlot(player, 63, ItemID.PaintSprayer, prefix: 68);
            updateSlot(player, 64, ItemID.FrogLeg, prefix: 68);
            updateSlot(player, 65, ItemID.ShinyStone, prefix: 68);
            updateSlot(player, 66, ItemID.BandofRegeneration, prefix: 68);
            updateSlot(player, 69, ItemID.BallaHat);
            updateSlot(player, 70, 4686);
            updateSlot(player, 79, ItemID.OrangeDye);
            updateSlot(player, 80, ItemID.OrangeDye);
            updateSlot(player, 90, ItemID.ParrotCracker);
        }
        private void classOnea(CommandArgs args)//TESTING PURPOSES ONLY

        {
            var player = args.Player;
            player.SendMessage("Command serves no purpose!", Microsoft.Xna.Framework.Color.BlueViolet);
            if (player == null) return;
            if (args.Parameters[0] == null) return;
            switch (Int16.Parse(args.Parameters[0]))
            {
                case 1:
                    classOne(player);
                    break;
                case 2:
                    classTwo(player);
                    break;
                case 3:
                    classThree(player);
                    break;
                case 4:
                    classFour(player);
                    break;
                case 5:
                    classFive(player);
                    break;
                case 6:
                    classSix(player);
                    break;
                case 7:
                    classSeven(player);
                    break;
                case 3559:
                    class3559(player);
                    break;
                case 69:
                    class69(player);
                    break;
            }
            return;
        }
        public void getPos(CommandArgs args)// GET X AND Y OF GIVEN PLAYER
        {
            var Player = args.Player;
            Console.WriteLine("X: " + Player.X + " Y:" + Player.Y + " Player: " + Player.Name);
            Console.WriteLine("Tile X: " + Player.X / 16 + " Y:" + Player.Y / 16 + " Player: " + Player.Name);
        }
        private void OnGetData(GetDataEventArgs args)// MAKE PLAYERS SO THEY CANNOT TOGGLE PVP
        {
            if (!args.Handled && (args.MsgID == PacketTypes.TogglePvp || args.MsgID == PacketTypes.ToggleParty))
            {
                var Player = TShock.Players[args.Msg.whoAmI];

                if (!Player.Group.HasPermission("pvp.toggle") || Player.Group.HasPermission("tshock.party.toggle"))
                {
                    args.Handled = true;
                    Player.SetPvP(true, false);
                    Player.SendErrorMessage("You do not have permission to change this setting");
                }
            }
        }
        public void reset(CommandArgs args)
        {
            foreach (var player in TShock.Players)
            {
                if (player == null || !player.Active) continue;
                player.Group = TShock.Groups.GetGroupByName("default");
            }
        } // CHANGE GROUP BACK TO DEFAULT
        private void PTCH(object sender, GetDataHandlers.PlayerTeamEventArgs e) // PLAYER TEAM CHANGE HANDLER
        {
            TSPlayer player = TShock.Players[e.PlayerId];
            if (player != null && player.Active)
            {
                if (Blue.Contains(player)) player.SetTeam(3);
                if (Red.Contains(player)) player.SetTeam(1);
                e.Handled = true;
                player.SendErrorMessage("You cannot change this setting");
                return;
            }
        }
        private void updateSlot(TSPlayer player, int slot, int itemn, bool stackable = false, int stacksize = 0, byte prefix = 0) // CHANGE ITEM TO GIVEN
        {
            int index; //A variable that will be used to find the index for the needed array (inventory[], armor[], dye[], etc.)
            var item = TShock.Utils.GetItemById(itemn);
            item.prefix = prefix;
            if (slot < NetItem.InventorySlots)
            {
                index = slot;
                player.TPlayer.inventory[slot] = item;
                if (stackable) player.TPlayer.inventory[slot].stack = stacksize;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.inventory[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.inventory[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.inventory[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.inventory[index].prefix);
            }

            //Armor & Accessory slots
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots)
            {
                index = slot - NetItem.InventorySlots;
                player.TPlayer.armor[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.armor[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.armor[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.armor[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.armor[index].prefix);
            }

            //Dye slots
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots)
            {
                index = slot - (NetItem.InventorySlots + NetItem.ArmorSlots);
                player.TPlayer.dye[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.dye[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.dye[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.dye[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.dye[index].prefix);
            }

            //Misc Equipment slots
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots)
            {
                index = slot - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots);
                player.TPlayer.miscEquips[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscEquips[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscEquips[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscEquips[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscEquips[index].prefix);
            }

            //Misc Dyes slots
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots)
            {
                index = slot - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots);
                player.TPlayer.miscDyes[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscDyes[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscDyes[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscDyes[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscDyes[index].prefix);
            }
        }
        private void OnGameAUpdateTwice(EventArgs args)// HANDLE GEM DROP AND CAPTURE
        {
            try
            {
                foreach (TSPlayer player in TShock.Players)
                {
                    if (player == null) continue;

                    if (player?.Active == true)
                    {
                        var currentRegion = player.CurrentRegion;
                        if (currentRegion != null)
                        {
                            if (player.Group != null)
                                if (GameA.Contains(player))
                                {
                                    if (player.Team == 1 && currentRegion.Name == "blueGem" && blueGem == null)
                                    {
                                        TShock.Utils.Broadcast(player.Name + " has picked up the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                                        blueGem = (player.Name, true);
                                        return;
                                    }
                                    else if (player.Team == 3 && currentRegion.Name == "redGem" && redGem == null)
                                    {
                                        TShock.Utils.Broadcast(player.Name + " has picked up the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);
                                        redGem = (player.Name, true);
                                        return;
                                    }
                                    if (blueGem != null || redGem != null)
                                    {
                                        if (player.Team == 1 && currentRegion.Name == "redGem" && blueGem.Value.Item1 == player.Name)
                                        {
                                            TShock.Utils.Broadcast(player.Name + " has captured the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                                            TShock.Utils.Broadcast("Congratulations Red Team", Microsoft.Xna.Framework.Color.Red);
                                            blueGem = null;
                                            winGameA(1, 3);
                                            cleanGameA();
                                        }
                                        else if (player.Team == 3 && currentRegion.Name == "blueGem" && redGem.Value.Item1 == player.Name)
                                        {
                                            TShock.Utils.Broadcast(player.Name + " has captured the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);
                                            TShock.Utils.Broadcast("Congratulations Blue Team", Microsoft.Xna.Framework.Color.Blue);
                                            redGem = null;
                                            winGameA(3, 1);
                                            cleanGameA();
                                        }
                                    }
                                }
                        }
                    }
                }
            }
            catch(Exception) { }
        }
        private void forFireWorkLaunchRed(EventArgs args)
        {
            var Now = DateTime.Now;
            if (redLaunch)
            {
                if (redGem != null)
                {
                    redLaunch = false;
                    var player = FindPlayerByName(redGem.Value.Item1);
                    Terraria.Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(), X: player.X, Y: player.Y, SpeedX: 0f, SpeedY: -8f, Type: 167, Damage: 0, KnockBack: 0, Owner: Terraria.Main.myPlayer);
                }
                time = Now.AddSeconds(5);
            }
            else
            {
                if (Now >= time)
                {
                    redLaunch = true;
                }
            }
        }
        private void forFireWorkLaunchBlue(EventArgs args)
        {
            var Now = DateTime.Now;
            if (blueLaunch)
            {
                if (blueGem != null)
                {
                    blueLaunch = false;
                    var player = FindPlayerByName(blueGem.Value.Item1);
                    Terraria.Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(), X: player.X, Y: player.Y, SpeedX: 0f, SpeedY: -8f, Type: 169, Damage: 0, KnockBack: 0, Owner: Terraria.Main.myPlayer);
                }
                bluetime = Now.AddSeconds(5);
            }
            else
            {
                if (Now >= bluetime)
                {
                    blueLaunch = true;
                }
            }
        }
        private void winGameA(int winTeam, int loseTeam)
        {
            foreach (var player in TShock.Players)
            {
                if (!player.Active || player == null) continue;
                if (player.Team == 1 || player.Team == 3)
                {
                    if (player.Team == winTeam)
                    {
                        updateSlot(player, 0, 1000, true, 20);
                    }
                    if (player.Team == loseTeam)
                    {
                        clearInventory(player);
                        player.KillPlayer();
                        player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                    }
                }
                Red.Clear();
                Blue.Clear();
                GameA.Clear();
                Lobby.Clear();
            }
            redGem = null;
            blueGem = null;
            Red.Clear();
            Blue.Clear();
            GameA.Clear();
            Lobby.Clear();
            cleanGameA();

        }
        private void clearFiftyeight(CommandArgs args)
        {
            updateSlot(args.Player, 179, 0);
        }
        private void clearInventory(TSPlayer Player, int[] exclusions = null)
        {
            for (int i = 0; i < 98; ++i)
            {
                if (exclusions != null)
                    if (exclusions.Contains(i)) continue;
                updateSlot(Player, i, ItemID.None);
            }
            Player.TPlayer.trashItem.stack = 0;
            Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, Terraria.Localization.NetworkText.FromLiteral(Player.TPlayer.trashItem.Name), Player.Index, 179);
            Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, Player.Index, -1, Terraria.Localization.NetworkText.FromLiteral(Player.TPlayer.trashItem.Name), Player.Index, 179);
            foreach (var buff in Player.TPlayer.buffType) { Player.TPlayer.ClearBuff(buff); }
        }
        private void applyDye(TSPlayer player)
        {
            int teamDye = 0;
            if (player.Team == 1) teamDye = ItemID.FlameDye;
            else teamDye = ItemID.BlueFlameDye;
            updateSlot(player, 79, teamDye);
            updateSlot(player, 80, teamDye);
            updateSlot(player, 81, teamDye);
            updateSlot(player, 82, teamDye);
            updateSlot(player, 83, teamDye);
            updateSlot(player, 84, teamDye);
            updateSlot(player, 85, teamDye);
            updateSlot(player, 86, teamDye);
            updateSlot(player, 87, teamDye);
            updateSlot(player, 94, teamDye);
            updateSlot(player, 95, teamDye);
            updateSlot(player, 96, teamDye);
            updateSlot(player, 97, teamDye);
            updateSlot(player, 98, teamDye);
            if (player.Team == 1)
                updateSlot(player, 49, ItemID.DeepRedPaint, true, 999);
            else if (player.Team == 3)
                updateSlot(player, 49, ItemID.BluePaint, true, 999);
            else
                updateSlot(player, 49, ItemID.WhitePaint, true, 999);
        }
        private void timeChanger(EventArgs args)
        {
            DateTime now = DateTime.Now;
            if (changeTime)
            {
                Terraria.Main.SkipToTime(36000, true);
                every5.AddMinutes(5);
                changeTime = false;
            }
            else
            {
                if (now >= every5)
                    changeTime = true;
            }
        }
        private void cleanGameA()
        {
            foreach (var player in TShock.Players)
            {
                if (player == null || !player.Active) continue;
                if ((player.Team == 1 || player.Team == 3) && player.Active)
                {
                    player.SetPvP(false);
                    player.SetTeam(0);
                }
            }
            redGem = null;
            blueGem = null;
            Red.Clear();
            Blue.Clear();
            GameA.Clear();
            Lobby.Clear();
            return;
        }
        public override Version Version => new Version(2, 8, 12);
        public override string Author => "Michael Cragun";
        public override string Description => "A simple plugin for my home server, with the CTG implementation";

        private void buffGroup(EventArgs args)
        {
            foreach (var player in TShock.Players)
            {
                if (player == null || !player.Active) continue;
                if(player.Group.Name == "YOUR NAME HERE")
                {
                    player.TPlayer.AddBuff(BuffID.Swiftness, -1); // PLUG IN BUFFS ACCORDINGLY
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

    }
}
