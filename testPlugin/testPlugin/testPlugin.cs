using TShockAPI;
using TerrariaApi.Server;

namespace testPlugin
{
    [ApiVersion(2,1)]
    public class testPlugin : TerrariaPlugin
    {
        
        public override string Name => "Test Plugin";
        public testPlugin(Terraria.Main game) : base(game)
        { 

        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, WorldLoaded);       }

        private void WorldLoaded(EventArgs args)
        {
            Commands.ChatCommands.Add(new Command("tp.ri", RandomItem, "ri", "randomitem", "randomi"));
            Commands.ChatCommands.Add(new Command("tp.wh", WormHole, "wh", "wormhole", "Wormhole", "WormHole"));
            Commands.ChatCommands.Add(new Command("tp.nd", NextDay, "nd", "nextd", "nextday", "nextDay"));
            Commands.ChatCommands.Add(new Command("tp.nn", NextNight, "nn", "nextn", "nextnight", "nextNight"));
            Commands.ChatCommands.Add(new Command("tp.an", Annoy, "an", "annoy"));
            Commands.ChatCommands.Add(new Command("tp.no", Noon, "noon", "no", "nooon"));
            Commands.ChatCommands.Add(new Command("tp.md", Midnight, "midnight", "md", "mid"));

        }

        public void RandomItem(CommandArgs args)
        {
            if (!args.Player.Active) return;

            var Player = args.Player;
            Random rand = new Random();

            int item = rand.Next(0, 5456);
            int stackSize = rand.Next(0, 300);

            Player.GiveItem(item, stackSize, Terraria.ID.PrefixID.Demonic);
            Player.SendMessage("Gave " + Player.Name +  ": " + stackSize + " " + TShock.Utils.GetItemById(item).Name, Microsoft.Xna.Framework.Color.DarkGoldenrod);
        }

        public void WormHole(CommandArgs args)// Gives Players a stack of wormhole potions
        {
            if(args.Player.Active == false) return;
            var player = args.Player;
            if(player.Team == 0) return;
            if(!HasWormholePotion(player)) player.GiveItem(2997, 30);
        }

        private bool HasWormholePotion(TSPlayer player)
        {
            foreach (var item in player.TPlayer.inventory)
            {
                if (item.type == Terraria.ID.ItemID.WormholePotion)
                {
                    return true;
                }
            }
            return false;
        }

        public void NextDay(CommandArgs args)// Old Method but works!
        {
            var Player = args.Player;
            if(Player.Active == false) return;

            Group defaultGroup = TShock.Groups.GetGroupByName("default");

            int[] ownerAdminIndices = GetOwnerAdminIndices();

            TSPlayer tempAdminPlayer = new TSPlayer(ownerAdminIndices[0])
            {
                Group = TShock.Groups.GetGroupByName("superadmin")
            };
            TShockAPI.Commands.HandleCommand(tempAdminPlayer, "/time 4:19");
        }

        public void NextNight(CommandArgs args)// Old Method but works!
        {
            if (!args.Player.Active) return;
            var player = args.Player;


            // Get the server's default group
            Group defaultGroup = TShock.Groups.GetGroupByName("default");

            int[] ownerAdminIndices = GetOwnerAdminIndices();

            // Create a temporary admin-level TSPlayer object
            TSPlayer tempAdminPlayer = new TSPlayer(ownerAdminIndices[0])
            {
                Group = TShock.Groups.GetGroupByName("superadmin")
            };

            TShockAPI.Commands.HandleCommand(tempAdminPlayer, "/time 20:00");
        }
        
        public void Annoy(CommandArgs args)// Spawns a rainbow slime and Zombie on them! Trolls!
        {
            if (!args.Player.Active) return;
            var playerName = args.Parameters[0].ToString();

            TSPlayer? player = FindPlayerByName(playerName); // Replace 0 with the desired player index

            if(player == null) return;

            int npcID = Terraria.NPC.NewNPC(source: Terraria.NPC.GetSpawnSourceForNaturalSpawn(), X: player.TileX * 16, Y: player.TileY * 16, Type: Terraria.ID.NPCID.Zombie);
            int npcID2 = Terraria.NPC.NewNPC(source: Terraria.NPC.GetSpawnSourceForNaturalSpawn(), X: player.TileX * 16, Y: player.TileY * 16, Type: Terraria.ID.NPCID.RainbowSlime);
            Terraria.NetMessage.SendData(Terraria.ID.MessageID.SyncNPC, -1, -1, null, npcID);
            Terraria.NetMessage.SendData(Terraria.ID.MessageID.SyncNPC, -1, -1, null, npcID2);

        }

        private TSPlayer? FindPlayerByName(string playername)
        {
            if (string.IsNullOrEmpty(playername)) return null;
            foreach(var player in TShock.Players)
                if(player?.Name == playername) return player;
            return null;
        }
        
        public void Noon(CommandArgs args)
        {
            TSPlayer.Server.SetTime(true, 1200);
        }

        public void Midnight(CommandArgs args)
        {
            if (!args.Player.Active) return;
            var player = args.Player;

            Group defaultGroup = TShock.Groups.GetGroupByName("default");

            int[] ownerAdminIndices = GetOwnerAdminIndices();

            // Create a temporary admin-level TSPlayer object
            TSPlayer tempAdminPlayer = new TSPlayer(ownerAdminIndices[0])
            {
                Group = TShock.Groups.GetGroupByName("superadmin")
            };

            TShockAPI.Commands.HandleCommand(tempAdminPlayer, "/time 23:59");
        }

        private int[] GetOwnerAdminIndices()
        {
            Group ownerGroup = TShock.Groups.GetGroupByName("owner");
            Group adminGroup = TShock.Groups.GetGroupByName("admin");

            int[] ownerAdminIndices = new int[TShock.Players.Length];
            int count = 0;

            for (int i = 0; i < TShock.Players.Length; i++)
            {
                if (TShock.Players[i] != null &&
                    (TShock.Players[i].Group == ownerGroup || TShock.Players[i].Group == adminGroup))
                {
                    ownerAdminIndices[count] = i;
                    count++;
                }
            }

            // Resize the array to the actual count of owner/admin indices
            System.Array.Resize(ref ownerAdminIndices, count);

            return ownerAdminIndices;
        }

        public override Version Version => new Version(1,0);
        public override string Author => "Michael Cragun";
        public override string Description => "A simple plugin for my home server";

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                    
            }
            base.Dispose(disposing);
        }

    }
}
