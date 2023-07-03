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
        public Dictionary<string, int> PlayerChoiceList = new Dictionary<string, int>();

        public List<TSPlayer> Lobby = new List<TSPlayer>();
        public List<TSPlayer> GamePlayerList = new List<TSPlayer>();
        public List<TSPlayer> RedTeamPlayers = new List<TSPlayer>();
        public List<TSPlayer> BlueTeamPlayers = new List<TSPlayer>();

        public DateTime RedTime;
        public DateTime Bluetime;
        public DateTime TimeEvery5Min;

        public (string, bool)? RedGemHolder = null;
        public (string, bool)? BlueGemHolder = null;

        public bool ChangeTime = false;
        public bool RedLaunch = false;
        public bool BlueLaunch = false;

        public enum Classes { Warrior, Tree, Ancient_One, Archer, Mage, Wandering, Wayne };

        public CTG(Terraria.Main game) : base(game)
        {

        }

        public override void Initialize()
        {
            GetDataHandlers.PlayerTeam += playerTeamChangeHandler;
            PlayerHooks.PlayerPostLogin += onPlayerPostLogin;

            ServerApi.Hooks.GameUpdate.Register(this, OnGameDeathHandler);
            ServerApi.Hooks.GameUpdate.Register(this, gemUpdateHandler);
            ServerApi.Hooks.GameUpdate.Register(this, forFireWorkLaunchRed);
            ServerApi.Hooks.GameUpdate.Register(this, forFireWorkLaunchBlue);
            ServerApi.Hooks.GameUpdate.Register(this, timeChanger);
            ServerApi.Hooks.GameInitialize.Register(this, WorldLoaded);
            ServerApi.Hooks.NetGetData.Register(this, OnGetPvpToggle);
        }

        private void WorldLoaded(EventArgs args)
        {
            Commands.ChatCommands.Add(new Command("CTG.j", Join, "j", "join", "J"));
            Commands.ChatCommands.Add(new Command("CTG.start", Start, "s", "start", "Start"));
            Commands.ChatCommands.Add(new Command("CTG.cl", SetClass, "class"));
            Commands.ChatCommands.Add(new Command("CTG.co", classDevSet, "classone", "co"));
        }

        private async void OnGameDeathHandler(EventArgs args)
        {
            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null)
                    continue;
                if (player.IsLoggedIn && player.TPlayer.dead)
                {
                    if (player.Team == 1)
                    {
                        player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                        if (BlueGemHolder != null)
                        {
                            if (player.Name == BlueGemHolder.Value.Item1)
                            {
                                BlueGemHolder = null;
                                TShock.Utils.Broadcast(player.Name + " has dropped the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                            }
                        }
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        player.Teleport((float)36853.426, 4982, 2);
                    }
                    else if (player.Team == 3)
                    {
                        if (RedGemHolder != null)
                        {
                            if (player.Name == RedGemHolder.Value.Item1)
                            {
                                RedGemHolder = null;
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

        public void Join(CommandArgs args) // Handles when someone tries to join CTG or anything else
        {
            if (args.Parameters[0] == null)
                return;
            else if (args.Parameters[0] == "CTG" || args.Parameters[0] == "ctg" || args.Parameters[0] == "Ctg")
            {
                TSPlayer player = args.Player;
                if (player != null)
                {
                    if (!GamePlayerList.Contains(player))
                        Lobby.Add(player);
                }
            }
            else
            {
                args.Player.SendErrorMessage("Invalid Option, please enter a valid option!");
            }
        }

        private void onPlayerPostLogin(PlayerPostLoginEventArgs args)
        {
            var player = findPlayerByName(args.Player.Name);

            if (player == null || !player.Active) return;
            if (player.Name == "kingfrankbob" || player.Name == "Kingfrankbob" || player.Name == "Roses" || player.Name == "roses")
                return;
            if (player != null)
            {
                player.SendInfoMessage("Welcome back to the server!");
                player.SendInfoMessage("Please enjoy your stay! Find games to play, and give feedback!");
                player.Group = TShock.Groups.GetGroupByName("default");
                Utility.ClearInventory(player);
                Utility.UpdateSlot(player, 62, 4954);
                Utility.UpdateSlot(player, 63, 4989);
                Utility.UpdateSlot(player, 0, 4344, true, 20);
                Utility.UpdateSlot(player, 1, 1000, true, 20);
            }
        }

        public void SetClass(CommandArgs args)
        {
            var purpleColor = Microsoft.Xna.Framework.Color.MediumPurple;
            if (PlayerChoiceList.ContainsKey(args.Player.Name))
                PlayerChoiceList.Remove(args.Player.Name);

            if (args.Parameters[0] == null || args.Parameters == null)
            {
                args.Player.SendErrorMessage("Please give a class!");
                return;
            }
            if (args.Parameters[0] == "random")
            {
                args.Player.SendMessage("Random Class selected!", Microsoft.Xna.Framework.Color.Yellow);
                var random = new Random();
                PlayerChoiceList.Add(args.Player.Name, random.Next(1, 8));
            }
            else if (short.Parse(args.Parameters[0]) > 0 && short.Parse(args.Parameters[0]) < 8 || short.Parse(args.Parameters[0]) == 3559)
            {
                if (short.Parse(args.Parameters[0]) != 3559 || short.Parse(args.Parameters[0]) != 69)
                {
                    args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + (Classes)(short.Parse(args.Parameters[0]) - 1) + "!!!", purpleColor);
                    PlayerChoiceList.Add(args.Player.Name, short.Parse(args.Parameters[0]));
                }
                else if ((short.Parse(args.Parameters[0]) == 3559
                    && (args.Player.Group.Name == "superadmin" || args.Player.Group.Name == "owner"))
                    || args.Player.Name == "aerg" || args.Player.Name == "Edjar"
                    || args.Player.Name == "kingfrankbob")
                {
                    args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + "Developer Class [MC]", purpleColor);
                    PlayerChoiceList.Add(args.Player.Name, short.Parse(args.Parameters[0]));
                }
                else if (short.Parse(args.Parameters[0]) == 3559
                     && (args.Player.Group.Name == "superadmin" || args.Player.Group.Name == "owner"))
                {
                    args.Player.SendMessage("You have not chosen class " + args.Parameters[0] + " ~ " + "Developer Class [MC]", purpleColor);
                }
                else if (short.Parse(args.Parameters[0]) == 69)
                {
                    args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + "John and Evan Class [JNER]", purpleColor);
                }
            }
            else
            {
                args.Player.SendErrorMessage("Invalid Syntax, please try: /class <number> between 1 and 7");
            }
        }  // What this does is add the class number to the list, and whether or not you can choose dev class.

        private TSPlayer? findPlayerByName(string playerName)
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

        public async void Start(CommandArgs args)
        {
            if (Lobby.Count < 2)
            {
                TShock.Utils.Broadcast("Not enough players!", Microsoft.Xna.Framework.Color.Yellow);
                return;
            }

            PlayerChoiceList.Clear();
            var playerCount = 0;

            foreach (TSPlayer player in Lobby)
            {
                GamePlayerList.Add(player);
                playerCount++;
            }

            Lobby.Clear();

            Utility.UpdateArena();
            var randomPlayers = randomizePlayersTeams(playerCount);
            playerCount = 0;
            var counter = 0;

            foreach (TSPlayer player in TShock.Players)
            {
                try
                {
                    if (player == null && !player.Active) continue;

                    if (GamePlayerList.Contains(player) && randomPlayers.Contains(counter))
                    {
                        playerCount++;
                        setPlayerRed(player);
                    }
                    else if (GamePlayerList.Contains(player))
                    {
                        setPlayerBlue(player);
                    }

                }
                catch (Exception)
                {
                    // It should not throw one, but incase it does, it is not important!
                }
                counter++;
            }

            TShock.Utils.Broadcast("You have 15 Seconds to choose your class!", Microsoft.Xna.Framework.Color.Yellow);
            TShock.Utils.Broadcast("Good Luck!", Microsoft.Xna.Framework.Color.Green);

            TShock.Groups.GetGroupByName("default").AddPermission("CTG.cl");
            await Task.Delay(TimeSpan.FromSeconds(15));
            TShock.Groups.GetGroupByName("default").RemovePermission("CTG.cl");

            TShock.Utils.Broadcast("GamePlayerList Starting, Good Luck", Microsoft.Xna.Framework.Color.Yellow);


            // After everyone sets the class
            // or gets assigned a random class
            // This handles it for each individual team
            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null) continue;

                if (GamePlayerList.Contains(player) && PlayerChoiceList.ContainsKey(player.Name))
                {
                    player.SendMessage("You have chosen class " + PlayerChoiceList[player.Name] + "!", Microsoft.Xna.Framework.Color.BlueViolet);
                    setClassItem(player);

                    if (player.Team == 1)
                    {
                        player.Teleport((float)36853.426, 4982, 2); // Red Spawn
                    }
                    else if (player.Team == 3)
                    {
                        player.Teleport((float)30359, 4982, 2); // Blue Spawn
                    }
                }
            }
        }

        private void setPlayerBlue(TSPlayer player)
        {
            BlueTeamPlayers.Add(player);
            player.SetTeam(3);
            player.SetPvP(true);
            player.Teleport((float)30131.447, 10534, 2);
            Utility.ClearInventory(player);
        }

        private void setPlayerRed(TSPlayer player)
        {
            RedTeamPlayers.Add(player);
            player.SetTeam(1);
            player.SetPvP(true);
            player.Teleport((float)30741.84, 10534, 3);
            Utility.ClearInventory(player);
        }

        private List<int> randomizePlayersTeams(int playerCount)
        {
            var perTeam = playerCount / 2;
            var randomPlayers = new List<int>();
            var random = new Random();

            do
            {
                var next = random.Next(perTeam);
                if (randomPlayers.Contains(next)) continue;
                randomPlayers.Add(next);
            } while (randomPlayers.Count < perTeam);

            return randomPlayers;
        }

        private void setClassItem(TSPlayer player)
        {
            if (player == null)
                return;

            if (!PlayerChoiceList.ContainsKey(player.Name))
            {
                var random = new Random();
                PlayerChoiceList.Add(player.Name, random.Next(1, 7));
            }

            switch (PlayerChoiceList[player.Name])
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
        }

        private void classOne(TSPlayer player) // Melee Class 
        {
            Utility.ClearInventory(player);
            Utility.ApplyDye(player);

            player.TPlayer.statLifeMax = 200;
            player.TPlayer.statLife = 200;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.UpdateSlot(player, 59, ItemID.CrimsonHelmet);
            Utility.UpdateSlot(player, 60, ItemID.CrimsonScalemail);
            Utility.UpdateSlot(player, 61, ItemID.CrimsonGreaves);
            Utility.UpdateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 42);
            Utility.UpdateSlot(player, 63, ItemID.PaintSprayer, prefix: 42);
            Utility.UpdateSlot(player, 64, ItemID.FrogLeg, prefix: 72);
            Utility.UpdateSlot(player, 65, ItemID.CobaltShield, prefix: 72);
            Utility.UpdateSlot(player, 66, ItemID.SweetheartNecklace, prefix: 72);
            Utility.UpdateSlot(player, 0, 5294);
            Utility.UpdateSlot(player, 1, 1320);
            Utility.UpdateSlot(player, 2, ItemID.WhoopieCushion);

            return;
        }

        private void classTwo(TSPlayer player)// Tree Class 
        {
            player.TPlayer.statLifeMax = 160;
            player.TPlayer.statLife = 160;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.ClearInventory(player);
            Utility.ApplyDye(player);

            Utility.UpdateSlot(player, 0, ItemID.WandofSparking);
            Utility.UpdateSlot(player, 1, 1320);
            Utility.UpdateSlot(player, 2, ItemID.WhoopieCushion);
            Utility.UpdateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            Utility.UpdateSlot(player, 59, ItemID.WoodHelmet);
            Utility.UpdateSlot(player, 60, ItemID.WoodBreastplate);
            Utility.UpdateSlot(player, 61, ItemID.WoodGreaves);
            Utility.UpdateSlot(player, 62, ItemID.LuckyHorseshoe);
            Utility.UpdateSlot(player, 63, ItemID.PaintSprayer);
            Utility.UpdateSlot(player, 64, ItemID.FrogLeg);
            Utility.UpdateSlot(player, 65, ItemID.CharmofMyths);
            Utility.UpdateSlot(player, 66, ItemID.SorcererEmblem);
            Utility.UpdateSlot(player, 69, ItemID.TreeMask);
            Utility.UpdateSlot(player, 70, ItemID.TreeShirt);
            Utility.UpdateSlot(player, 71, ItemID.TreeTrunks);
            Utility.UpdateSlot(player, 93, ItemID.WebSlinger);
            Utility.UpdateSlot(player, 90, 4813);

        }

        private void class3559(TSPlayer player)// Developer Class
        {
            player.TPlayer.statLifeMax = 500;
            player.TPlayer.statLife = 500;
            Utility.UpdateSlot(player, 0, 4956); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.ClearInventory(player);

            Utility.UpdateSlot(player, 1, ItemID.ShroomiteDiggingClaw);
            Utility.UpdateSlot(player, 2, ItemID.WhoopieCushion);
            Utility.UpdateSlot(player, 3, ItemID.LihzahrdBrick, true, 9999);
            Utility.UpdateSlot(player, 4, ItemID.RodOfHarmony);
            Utility.UpdateSlot(player, 5, ItemID.DrillContainmentUnit);
            Utility.UpdateSlot(player, 59, ItemID.SolarFlareHelmet);
            Utility.UpdateSlot(player, 60, ItemID.SolarFlareBreastplate);
            Utility.UpdateSlot(player, 61, ItemID.SolarFlareLeggings);
            Utility.UpdateSlot(player, 62, 4954);
            Utility.UpdateSlot(player, 63, 4989);
            Utility.UpdateSlot(player, 64, ItemID.BerserkerGlove);
            Utility.UpdateSlot(player, 65, ItemID.TerrasparkBoots);
            Utility.UpdateSlot(player, 66, ItemID.CelestialShell);
            Utility.UpdateSlot(player, 93, ItemID.LunarHook);
            Utility.UpdateSlot(player, 90, 4813);
        }

        private void class69(TSPlayer player)// TODO ~ Needs to be added 
        {

        }

        private void classThree(TSPlayer player)// Ancient One Class
        {
            player.TPlayer.statLife = 120;
            player.TPlayer.statLifeMax = 120;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.ClearInventory(player);
            Utility.ApplyDye(player);

            Utility.UpdateSlot(player, 0, 4913);
            Utility.UpdateSlot(player, 1, 1320, prefix: 15);
            Utility.UpdateSlot(player, 2, ItemID.WhoopieCushion);
            Utility.UpdateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            Utility.UpdateSlot(player, 59, ItemID.BeeHeadgear);
            Utility.UpdateSlot(player, 60, ItemID.BeeBreastplate);
            Utility.UpdateSlot(player, 61, ItemID.BeeGreaves);
            Utility.UpdateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 76);
            Utility.UpdateSlot(player, 63, ItemID.PaintSprayer, prefix: 76);
            Utility.UpdateSlot(player, 64, ItemID.FrogLeg, prefix: 68);
            Utility.UpdateSlot(player, 65, 3097, prefix: 80);
            Utility.UpdateSlot(player, 66, ItemID.BlizzardinaBottle);
            Utility.UpdateSlot(player, 69, 3773);
            Utility.UpdateSlot(player, 70, 3774);
            Utility.UpdateSlot(player, 71, 3775);
            Utility.UpdateSlot(player, 90, ItemID.ExoticEasternChewToy);
        }

        private void classFour(TSPlayer player)// Archer Class
        {
            player.TPlayer.statLife = 180;
            player.TPlayer.statLifeMax = 180;
            Utility.UpdateSlot(player, 0, ItemID.DemonBow); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.ClearInventory(player);
            Utility.ApplyDye(player);

            Utility.UpdateSlot(player, 1, 1320);
            Utility.UpdateSlot(player, 2, ItemID.WhoopieCushion);
            Utility.UpdateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            Utility.UpdateSlot(player, 59, ItemID.NecroHelmet);
            Utility.UpdateSlot(player, 60, ItemID.NecroBreastplate);
            Utility.UpdateSlot(player, 61, ItemID.NecroGreaves);
            Utility.UpdateSlot(player, 62, ItemID.LuckyHorseshoe);
            Utility.UpdateSlot(player, 63, ItemID.PaintSprayer);
            Utility.UpdateSlot(player, 64, ItemID.FrogLeg);
            Utility.UpdateSlot(player, 65, ItemID.CloudinaBottle);
            Utility.UpdateSlot(player, 54, ItemID.HellfireArrow, true, 9999);
            Utility.UpdateSlot(player, 69, ItemID.HerosHat);
            Utility.UpdateSlot(player, 70, ItemID.HerosShirt);
            Utility.UpdateSlot(player, 71, ItemID.HerosPants);
            Utility.UpdateSlot(player, 90, ItemID.FairyBell);
        }

        private void classFive(TSPlayer player)// Mage Class
        {
            player.TPlayer.statLife = 120;
            player.TPlayer.statLifeMax = 120;
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);

            Utility.ClearInventory(player);
            Utility.ApplyDye(player);

            Utility.UpdateSlot(player, 0, ItemID.Vilethorn);
            Utility.UpdateSlot(player, 1, ItemID.IceRod);
            Utility.UpdateSlot(player, 2, 1320);
            Utility.UpdateSlot(player, 4, ItemID.DirtBlock, true, 9999);
            Utility.UpdateSlot(player, 3, ItemID.WhoopieCushion);
            Utility.UpdateSlot(player, 59, ItemID.WizardHat);
            Utility.UpdateSlot(player, 60, ItemID.AmberRobe);
            Utility.UpdateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 65);
            Utility.UpdateSlot(player, 63, ItemID.PaintSprayer, prefix: 65);
            Utility.UpdateSlot(player, 64, ItemID.FrogLeg, prefix: 74);
            Utility.UpdateSlot(player, 65, ItemID.FrozenShield, prefix: 68);
            Utility.UpdateSlot(player, 66, ItemID.CelestialStone, prefix: 68);
            Utility.UpdateSlot(player, 69, ItemID.RuneHat);
            Utility.UpdateSlot(player, 70, ItemID.RuneRobe);
            Utility.UpdateSlot(player, 90, 4809);
        }

        private void classSix(TSPlayer player)// Wandering Ninja Class
        {
            player.TPlayer.statLife = 200;
            player.TPlayer.statLifeMax = 200;
            Utility.UpdateSlot(player, 0, ItemID.Katana); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.ClearInventory(player);
            Utility.ApplyDye(player);

            Utility.UpdateSlot(player, 1, 1320);
            Utility.UpdateSlot(player, 2, ItemID.WhoopieCushion);
            Utility.UpdateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            Utility.UpdateSlot(player, 59, ItemID.AncientShadowHelmet);
            Utility.UpdateSlot(player, 60, ItemID.AncientShadowScalemail);
            Utility.UpdateSlot(player, 61, ItemID.AncientShadowGreaves);
            Utility.UpdateSlot(player, 62, ItemID.LuckyHorseshoe);
            Utility.UpdateSlot(player, 63, ItemID.PaintSprayer);
            Utility.UpdateSlot(player, 64, ItemID.FrogLeg);
            Utility.UpdateSlot(player, 65, ItemID.SharkToothNecklace);
            Utility.UpdateSlot(player, 66, ItemID.FleshKnuckles);
            Utility.UpdateSlot(player, 69, 5048);
            Utility.UpdateSlot(player, 70, 5049);
            Utility.UpdateSlot(player, 71, 5050);
            Utility.UpdateSlot(player, 90, ItemID.BambooLeaf);
        }

        private void classSeven(TSPlayer player)// Wayne from Brandon Sanderson Class
        {
            player.TPlayer.statLife = 220;
            player.TPlayer.statLifeMax = 220;
            Utility.UpdateSlot(player, 0, 3351, prefix: 81); TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.ClearInventory(player);
            Utility.ApplyDye(player);

            Utility.UpdateSlot(player, 1, 1320);
            Utility.UpdateSlot(player, 2, ItemID.WhoopieCushion);
            Utility.UpdateSlot(player, 3, ItemID.DirtBlock, true, 9999);
            Utility.UpdateSlot(player, 59, ItemID.MagicHat);
            Utility.UpdateSlot(player, 60, ItemID.Gi);
            Utility.UpdateSlot(player, 61, ItemID.OrichalcumLeggings);
            Utility.UpdateSlot(player, 62, ItemID.LuckyHorseshoe, prefix: 68);
            Utility.UpdateSlot(player, 63, ItemID.PaintSprayer, prefix: 68);
            Utility.UpdateSlot(player, 64, ItemID.FrogLeg, prefix: 68);
            Utility.UpdateSlot(player, 65, ItemID.ShinyStone, prefix: 68);
            Utility.UpdateSlot(player, 66, ItemID.BandofRegeneration, prefix: 68);
            Utility.UpdateSlot(player, 69, ItemID.BallaHat);
            Utility.UpdateSlot(player, 70, 4686);
            Utility.UpdateSlot(player, 79, ItemID.OrangeDye);
            Utility.UpdateSlot(player, 80, ItemID.OrangeDye);
            Utility.UpdateSlot(player, 90, ItemID.ParrotCracker);
        }

        private void classDevSet(CommandArgs args)
        {
            var player = args.Player;
            player.SendMessage("Command serves no purpose!", Microsoft.Xna.Framework.Color.BlueViolet);

            if (player == null || args.Parameters.Count == 0)
                return;

            switch (short.Parse(args.Parameters[0]))
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
        }

        private void OnGetPvpToggle(GetDataEventArgs args)
        {
            if (!args.Handled && (args.MsgID == PacketTypes.TogglePvp || args.MsgID == PacketTypes.ToggleParty))
            {
                var player = TShock.Players[args.Msg.whoAmI];

                if (!player.Group.HasPermission("pvp.toggle") || player.Group.HasPermission("tshock.party.toggle"))
                {
                    args.Handled = true;
                    player.SetPvP(true, false);
                    player.SendErrorMessage("You do not have permission to change this setting");
                }
            }
        }

        private void playerTeamChangeHandler(object? sender, GetDataHandlers.PlayerTeamEventArgs eArgs)
        {
            TSPlayer player = TShock.Players[eArgs.PlayerId];
            if (player != null && player.Active)
            {
                if (BlueTeamPlayers.Contains(player)) player.SetTeam(3);
                if (RedTeamPlayers.Contains(player)) player.SetTeam(1);
                eArgs.Handled = true;
                player.SendErrorMessage("You cannot change this setting");
            }
        }

        private void gemUpdateHandler(EventArgs args)// Handles pickup and dropping of the gems
        {
            try
            {
                foreach (TSPlayer player in TShock.Players)
                {
                    if (player == null || !player.Active) continue;

                    var currentRegion = player.CurrentRegion;

                    if (!GamePlayerList.Contains(player)
                    || player.Group == null
                    || currentRegion == null) continue;

                    if (player.Team == 1 && currentRegion.Name == "BlueGemHolder" && BlueGemHolder == null)
                    {
                        TShock.Utils.Broadcast(player.Name + " has picked up the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                        BlueGemHolder = (player.Name, true);
                        return;
                    }
                    else if (player.Team == 3 && currentRegion.Name == "RedGemHolder" && RedGemHolder == null)
                    {
                        TShock.Utils.Broadcast(player.Name + " has picked up the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);
                        RedGemHolder = (player.Name, true);
                        return;
                    }

                    if (BlueGemHolder != null)
                        if (player.Team == 1 && currentRegion.Name == "RedGemHolder" && BlueGemHolder.Value.Item1 == player.Name)
                        {
                            TShock.Utils.Broadcast(player.Name + " has captured the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                            TShock.Utils.Broadcast("Congratulations Red Team", Microsoft.Xna.Framework.Color.Red);
                            BlueGemHolder = null;
                            winGame(1);
                        }

                    if (RedGemHolder != null)
                        if (player.Team == 3 && currentRegion.Name == "BlueGemHolder" && RedGemHolder.Value.Item1 == player.Name)
                        {
                            TShock.Utils.Broadcast(player.Name + " has captured the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);
                            TShock.Utils.Broadcast("Congratulations Blue Team", Microsoft.Xna.Framework.Color.Blue);
                            RedGemHolder = null;
                            winGame(3);
                        }
                }
            }
            catch (Exception) { }
        }

        private void forFireWorkLaunchRed(EventArgs args)
        {
            var now = DateTime.Now;
            if (RedLaunch)
            {
                if (RedGemHolder != null)
                {
                    RedLaunch = false;
                    var player = findPlayerByName(RedGemHolder.Value.Item1);
                    Terraria.Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(),
                        X: player.X, Y: player.Y, SpeedX: 0f, SpeedY: -8f, Type: 167, Damage: 0, KnockBack: 0, Owner: Terraria.Main.myPlayer);
                }
                RedTime = now.AddSeconds(5);
            }
            else
            {
                if (now >= RedTime)
                {
                    RedLaunch = true;
                }
            }
        }

        private void forFireWorkLaunchBlue(EventArgs args)
        {
            var now = DateTime.Now;
            if (BlueLaunch)
            {
                if (BlueGemHolder != null)
                {
                    BlueLaunch = false;
                    var player = findPlayerByName(BlueGemHolder.Value.Item1);
                    Terraria.Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(),
                        X: player.X, Y: player.Y, SpeedX: 0f, SpeedY: -8f, Type: 169, Damage: 0, KnockBack: 0, Owner: Terraria.Main.myPlayer);
                }
                Bluetime = now.AddSeconds(5);
            }
            else
            {
                if (now >= Bluetime)
                {
                    BlueLaunch = true;
                }
            }
        }

        private void winGame(int winTeam)
        {
            var loseTeam = (winTeam == 1) ? 3 : 1;
            foreach (TSPlayer player in TShock.Players)
            {
                if (!player.Active || player == null) continue;

                if (player.Team == 1 || player.Team == 3)
                {
                    if (player.Team == winTeam)
                    {
                        Utility.UpdateSlot(player, 0, 1000, true, 20);
                    }
                    if (player.Team == loseTeam)
                    {
                        Utility.ClearInventory(player);
                        player.KillPlayer();
                        player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                    }
                }
            }
            cleanGameEverything();
        }

        private void cleanGameEverything()
        {
            RedGemHolder = null;
            BlueGemHolder = null;
            RedTeamPlayers.Clear();
            BlueTeamPlayers.Clear();
            GamePlayerList.Clear();
            Lobby.Clear();

            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null || !player.Active)
                    continue;

                if ((player.Team == 1 || player.Team == 3) && player.Active)
                {
                    player.SetPvP(false);
                    player.SetTeam(0);
                }
            }

            return;
        }

        private void timeChanger(EventArgs args)
        {
            DateTime now = DateTime.Now;

            if (ChangeTime)
            {
                Terraria.Main.SkipToTime(36000, true);
                TimeEvery5Min = TimeEvery5Min.AddMinutes(5);
                ChangeTime = false;
            }
            else
            {
                if (now >= TimeEvery5Min)
                    ChangeTime = true;
            }
        }

        public override Version Version => new Version(3, 0, 0);

        public override string Author => "Michael Cragun";

        public override string Description => "A simple plugin for my home server, with the CTG implementation";

        public override string Name => "Capture The Gem plugin!!!";

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

    }
}
