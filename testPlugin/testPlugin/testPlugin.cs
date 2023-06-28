using Terraria;
using System;
using TShockAPI;
using TerrariaApi.Server;
using System.Diagnostics.Contracts;
using TShockAPI.Hooks;
using Terraria.ID;
using System.Drawing;
using IL.Terraria;

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
            Commands.ChatCommands.Add(new Command("tp.ri", randomItem, "ri", "randomitem", "randomi"));
            Commands.ChatCommands.Add(new Command("tp.wh", wormHole, "wh", "wormhole", "Wormhole", "WormHole"));
            Commands.ChatCommands.Add(new Command("tp.nd", nextDay, "nd", "nextd", "nextday", "nextDay"));
            Commands.ChatCommands.Add(new Command("tp.nn", nextNight, "nn", "nextn", "nextnight", "nextNight"));
            Commands.ChatCommands.Add(new Command("tp.an", annoy, "an", "annoy"));
            Commands.ChatCommands.Add(new Command("tp.no", timeChanger, "noon", "no", "nooon"));
            Commands.ChatCommands.Add(new Command("tp.md", midnight, "midnight", "md", "mid"));

        }

        public void randomItem(CommandArgs args)
        {
            if (!args.Player.Active) return;
            var Player = args.Player;
            Random rand = new Random();
            int item = rand.Next(0, 5456);
            int stackSize = rand.Next(0, 300);
            Player.GiveItem(item, stackSize, Terraria.ID.PrefixID.Demonic);
            Player.SendMessage("Gave " + Player.Name +  ": " + stackSize + " " + TShock.Utils.GetItemById(item).Name, Microsoft.Xna.Framework.Color.DarkGoldenrod);
            return;
        }
        public void wormHole(CommandArgs args)
        {
            if(args.Player.Active == false) return;
            var player = args.Player;
            if(player.Team == 0) return;
            else player.GiveItem(2997, 30);
        }
        public void nextDay(CommandArgs args)
        {
            var Player = args.Player;
            if(Player.Active == false) return;

            // Get the server's default group
            Group defaultGroup = TShock.Groups.GetGroupByName("default");

            int[] ownerAdminIndices = GetOwnerAdminIndices();

            // Create a temporary admin-level TSPlayer object
            TSPlayer tempAdminPlayer = new TSPlayer(ownerAdminIndices[0])
            {
                Group = TShock.Groups.GetGroupByName("superadmin")
            };

            TShockAPI.Commands.HandleCommand(tempAdminPlayer, "/time 4:19");
        }
        public void nextNight(CommandArgs args)
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
        
        public void annoy(CommandArgs args)
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
        

        public void noon(CommandArgs args)
        {
            TSPlayer.Server.SetTime(true, 1200);
        }
        public void midnight(CommandArgs args)
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

        private void timeChanger(CommandArgs args)
        {
            Terraria.Main.SkipToTime(1000, true);
        }


        //private void OnPostInitialize(EventArgs args)
        //{
        //    // Get the current time of the server
        //    bool isDaytime = Terraria.Main.dayTime;
        //    double time = Terraria.Main.time;

        //    // Output the current time to console
        //    TShock.Log.ConsoleInfo($"Server time: {(isDaytime ? "Day" : "Night")} - {time} ticks");
        //}


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
