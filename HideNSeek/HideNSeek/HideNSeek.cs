using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;
using System.Diagnostics;

namespace HideNSeek
{
    [ApiVersion(2, 1)]
    public class HideNSeek : TerrariaPlugin
    {
        public override string Name => "HIDE N SEEK BOYZ!";

        public List<string> Lobby = new List<string>();
        public List<string> GameA = new List<string>();
        public List<string> Alive = new List<string>();
        public List<string> beenIT = new List<string>();
        public List<string> canBe = new List<string>();

        public DateTime nextCatch = DateTime.Now;

        bool check = false, gameOn = false;

        public Stopwatch time = new Stopwatch();


        public string previousIt = "";
        public HideNSeek(Terraria.Main game) : base(game)
        {

        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, WorldLoaded);
            ServerApi.Hooks.GameUpdate.Register(this, autoStart);
            ServerApi.Hooks.GameUpdate.Register(this, OnGameUpdate);

        }
        private void WorldLoaded(EventArgs args)
        {
            Commands.ChatCommands.Add(new Command("HNS.j", Join, "j", "join", "J"));
        }
        private void OnGameUpdate(EventArgs args)// Checks for Death, and finishes Gane if needed every tick
        {
            if (gameOn)
            {
                foreach (var player in TShock.Players)
                {
                    if (player == null) continue;
                    if (player.IsLoggedIn && player.TPlayer.dead)
                    {
                        if (Alive.Contains(player.Name))
                        {
                            Alive.Remove(player.Name);
                        }
                    }
                }
                if (Alive.Count == 0)
                {
                    time.Stop();
                    var timeTook = time.Elapsed.Subtract(new TimeSpan(0, 0, 15));
                    foreach (var player in GameA)
                    {
                        var Player = FindPlayerByName(player);
                        Player.SetPvP(false);
                        Player.SetTeam(0);
                        Player.SetBuff(1, 0);
                        Player.SetBuff(10, 0);
                        Player.SetBuff(15, 0);
                        Utility.ClearInventory(Player);
                        Player.KillPlayer();
                        Player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                    }
                    var secondss = false;
                    var timee = timeTook.TotalMinutes;
                    if (timee < 1)
                    {
                        secondss = true;
                        timee = timeTook.TotalSeconds;
                    }
                    TShock.Utils.Broadcast(previousIt + " Has won the game in: " + (int)timee + (secondss ? " seconds " : " minutes ") + "killing " + (GameA.Count - 1) + (((GameA.Count - 1) != 1) ? " souls!" : " soul!"), Microsoft.Xna.Framework.Color.DarkOrange);
                    GameA.Clear();
                    time.Reset();
                    gameOn = false;
                }
            }
        }

        private void autoStart(EventArgs args)
        {
            var Now = DateTime.Now;
            if (check)
            {
                if (Lobby.Count > 1)
                {
                    Start();
                    Lobby.Clear();
                }
                else
                {
                    if (!gameOn)
                        TShock.Utils.Broadcast("More players Needed to start, you have: " + Lobby.Count + ", compared to what you need: 2", Microsoft.Xna.Framework.Color.OrangeRed);
                }
                check = false;
                nextCatch = Now.AddSeconds(20);
            }
            else
            {
                if (Now >= nextCatch)
                {
                    check = true;
                }
            }
        }

        private void Join(CommandArgs args)
        {
            var player = args.Player;
            if (args.Parameters == null || args.Parameters.Count == 0 || args.Parameters[0] == "")
            {
                player.SendErrorMessage("Please try again! Syntax: /join <name>");
                return;
            }
            if (args.Parameters[0].ToLower() == "hns" || args.Parameters[0].ToLower() == "hidenseek")
            {
                if (!Lobby.Contains(player.Name))
                {
                    Lobby.Add(player.Name);
                    player.SendMessage("You have been added to the Hide N' Seek Lobby!", Microsoft.Xna.Framework.Color.Cyan);
                }
            }
            else
            {
                player.SendErrorMessage("Please try entering a Valid game! hns or hidenseek");
            }
        }

        private void Start()
        {
            GameA.Clear();
            foreach (var person in Lobby)
            {
                if (!beenIT.Contains(person)) canBe.Add(person);
                GameA.Add(person);
                Utility.ClearInventory(FindPlayerByName(person));
            }
            Lobby.Clear();
            Alive.Clear();
            var iT = "";

            do
            {
                var random = new Random();
                var next = random.Next(0, GameA.Count);
                iT = GameA[next];
                if (!beenIT.Contains(iT) && canBe.Contains(iT))
                { beenIT.Add(iT); canBe.Remove(iT); break; }
            } while (true);
            TShock.Utils.Broadcast("Hide N' Seek is starting! ", Microsoft.Xna.Framework.Color.Cyan);

            previousIt = iT;
            setSeeker(FindPlayerByName(iT));
            FindPlayerByName(iT).Teleport((float)30359, 4982, 2);
            foreach (var person in GameA)
            {
                if (person == iT) continue;
                TShock.Utils.Broadcast(person, Microsoft.Xna.Framework.Color.Cyan);

                setEveryone(FindPlayerByName(person));
                FindPlayerByName(person).Teleport((float)36853.426, 4982, 2);
                Alive.Add(person);
            }
            gameOn = true;
            time.Start();
        }

        private TSPlayer FindPlayerByName(string playerName)
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

        private void setSeeker(TSPlayer player)
        {
            player.TPlayer.statLife = 500;
            player.TPlayer.statLifeMax = 500;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.UpdateSlot(player, 0, ItemID.Meowmere);
            Utility.UpdateSlot(player, 1, 5335);
            Utility.UpdateSlot(player, 63, 4989);
            Utility.UpdateSlot(player, 64, 4954);
            Utility.UpdateSlot(player, 74, ItemID.RedsWings);

            player.SetBuff(1, 999999999);
            player.SetBuff(10, 999999999);
            player.SetBuff(15, 999999999);
            player.SetBuff(47, 300);
            player.SetBuff(149, 300);
            player.SetPvP(true);
            player.SetTeam(2);
        }

        private void setEveryone(TSPlayer player)
        {
            player.TPlayer.statLife = 20;
            player.TPlayer.statLifeMax = 20;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, number: player.Index);
            NetMessage.SendData((int)PacketTypes.PlayerHp, number: player.Index);

            Utility.UpdateSlot(player, 63, 4989);
            Utility.UpdateSlot(player, 64, 4954);
            Utility.UpdateSlot(player, 74, ItemID.RedsWings);
            Utility.UpdateSlot(player, 0, 5335);

            player.SetBuff(1, 999999999);
            player.SetBuff(10, 999999999);
            player.SetBuff(15, 999999999);
            player.SetPvP(true);
            player.SetTeam(5);
        }

        public override Version Version => new Version(2, 8, 12);

        public override string Author => "Michael Cragun";

        public override string Description => "Hide n' Seek with John!!!";

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

    }
}