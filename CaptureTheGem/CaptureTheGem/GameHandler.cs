using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;
using Org.BouncyCastle.Asn1.Mozilla;

namespace CaptureTheGem
{
    internal class GameHandler
    {

        public GameData CurrentGame;

        public bool ChangeTime = false;
        public bool CheckForPlayers = true;

        public DateTime ChangeTimeEvery5Min;
        public DateTime CheckNext;

        public GameHandler() 
        {
            CurrentGame = new GameData();
        }


        public async void Start()
        {

            CurrentGame.GameOn = true;
            CurrentGame.PlayerChoiceList.Clear();
            var playerCount = 0;

            foreach (TSPlayer player in CurrentGame.Lobby)
            {
                CurrentGame.GamePlayerList.Add(player);
                playerCount++;
            }

            CurrentGame.Lobby.Clear();

            Utility.UpdateArena();

            var randomPlayers = RandomizePlayersTeams(playerCount);
            playerCount = 0;
            var counter = 0;

            foreach (TSPlayer player in TShock.Players)
            {
                try
                {
                    if (player == null || !player.Active) continue;

                    if (CurrentGame.GamePlayerList.Contains(player) && randomPlayers.Contains(counter))
                    {
                        playerCount++;
                        SetPlayerRed(player);
                    }
                    else if (CurrentGame.GamePlayerList.Contains(player))
                    {
                        SetPlayerBlue(player);
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

            TShock.Utils.Broadcast("Game Starting, Good Luck", Microsoft.Xna.Framework.Color.Yellow);



            // After everyone sets the class
            // or gets assigned a random class
            // This handles it for each individual team
            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null || !player.Active) continue;


                if (CurrentGame.GamePlayerList.Contains(player))
                {

                    if (!CurrentGame.PlayerChoiceList.ContainsKey(player.Name))
                    {
                        var random = new Random();
                        CurrentGame.PlayerChoiceList.Add(player.Name, random.Next(1, 7));
                    }

                    player.SendMessage("You have chosen class " + CurrentGame.PlayerChoiceList[player.Name] + "!", Microsoft.Xna.Framework.Color.BlueViolet);
                    SetClassItem(player);

                    switch (player.Team)
                    {

                        case 1: player.Teleport((float)36853.426, 4982, 2); break; // Red Spawn 

                        case 3: player.Teleport((float)30359, 4982, 2); break; // Blue Spawn

                    }
                }
            }
        }

        public void SetClassItem(TSPlayer player)
        {
            if (player == null || !player.Active)
                return;

            switch (CurrentGame.PlayerChoiceList[player.Name])
            {
                case 1:
                    CTGClasses.ClassOne(player);
                    break;
                case 2:
                    CTGClasses.ClassTwo(player);
                    break;
                case 3:
                    CTGClasses.ClassThree(player);
                    break;
                case 4:
                    CTGClasses.ClassFour(player);
                    break;
                case 5:
                    CTGClasses.ClassFive(player);
                    break;
                case 6:
                    CTGClasses.ClassSix(player);
                    break;
                case 7:
                    CTGClasses.ClassSeven(player);
                    break;
                case 3559:
                    CTGClasses.Class3559(player);
                    break;
                case 69:
                    CTGClasses.Class69(player);
                    break;
            }
        } // Adapted

        public void SetPlayerBlue(TSPlayer player)
        {
            CurrentGame.AddBluePlayer(player);
            player.SetTeam(3);
            player.SetPvP(true);
            player.Teleport((float)30131.447, 10534, 2);
            Utility.ClearInventory(player);
        }

        public void SetPlayerRed(TSPlayer player)
        {
            CurrentGame.AddRedPlayer(player);
            player.SetTeam(1);
            player.SetPvP(true);
            player.Teleport((float)30741.84, 10534, 3);
            Utility.ClearInventory(player);
        }

        public List<int> RandomizePlayersTeams(int playerCount)
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


        // In this section all of the event handlers are done!


        public void GemUpdateHandler(EventArgs args)// Handles pickup and dropping of the gems
        {
            
            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null || !player.Active) continue;

                var currentRegion = player.CurrentRegion;

                if (!CurrentGame.IsPlaying(player)
                || player.Group == null
                || currentRegion == null) continue;


                if (player.Team == 1 && currentRegion.Name == "blueGem" && !CurrentGame.IsTeamGemHeld(3))
                {
                    TShock.Utils.Broadcast(player.Name + " has picked up the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                    CurrentGame.SetBlueGemHolder(player);
                }

                if (player.Team == 3 && currentRegion.Name == "redGem" && !CurrentGame.IsTeamGemHeld(1))
                {
                    TShock.Utils.Broadcast(player.Name + " has picked up the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);
                    CurrentGame.SetRedGemHolder(player);
                }

                if (CurrentGame.IsTeamGemHeld(3)) // Null warning is ignorable because this statement checks to see if it could be null
                    if (player.Team == 1 && currentRegion.Name == "redGem" && CurrentGame.BlueHolder().Name == player.Name)
                    {
                        TShock.Utils.Broadcast(player.Name + " has captured the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);
                        TShock.Utils.Broadcast("Congratulations Red Team", Microsoft.Xna.Framework.Color.Red);

                        CurrentGame = new GameData();
                        Utility.WinGame(1);

                    }

                if (CurrentGame.IsTeamGemHeld(1)) // Null warning is ignorable because this statement checks to see if it could be null
                    if (player.Team == 3 && currentRegion.Name == "blueGem" && CurrentGame.RedHolder().Name == player.Name)
                    {
                        TShock.Utils.Broadcast(player.Name + " has captured the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);
                        TShock.Utils.Broadcast("Congratulations Blue Team", Microsoft.Xna.Framework.Color.Blue);

                        CurrentGame = new GameData();
                        Utility.WinGame(3);

                    }

            }

        }

        public void PlayerTeamChangeHandler(object? sender, GetDataHandlers.PlayerTeamEventArgs eArgs)
        {
            TSPlayer player = TShock.Players[eArgs.PlayerId];
            if (player != null && player.Active)
            {
                if (CurrentGame.IsPlayingBlue(player)) player.SetTeam(3);
                if (CurrentGame.IsPlayingRed(player)) player.SetTeam(1);
                eArgs.Handled = true;
                player.SendErrorMessage("You cannot change this setting");
            }
        }

        public void HandleGameEvents(EventArgs args)
        {
            GameEventHandler.ForFireWorkLaunchRed(ref CurrentGame); 
            GameEventHandler.ForFireWorkLaunchBlue(ref CurrentGame);
            GameEventHandler.OnGameDeathHandler(ref CurrentGame);
        }

        public void TimeChanger(EventArgs args)
        {
            DateTime now = DateTime.Now;

            if (ChangeTime)
            {
                Terraria.Main.SkipToTime(36000, true);
                ChangeTimeEvery5Min = ChangeTimeEvery5Min.AddMinutes(5);
                ChangeTime = false;
            }
            else
            {
                if (now >= ChangeTimeEvery5Min)
                    ChangeTime = true;
            }

        }

        public void AutoStart(EventArgs args)
        {
            var Now = DateTime.Now;
            if (CheckForPlayers)
            {
                if (CurrentGame.Lobby.Count > 1)
                {
                    Start();
                }
                else
                {
                    if (!CurrentGame.GameOn)
                        TShock.Utils.Broadcast("More players Needed to start, you have: " + CurrentGame.Lobby.Count, Microsoft.Xna.Framework.Color.OrangeRed);
                }
                CheckForPlayers = false;
                CheckNext = Now.AddSeconds(20);
            }
            else
            {
                if (Now >= CheckNext)
                {
                    CheckForPlayers = true;
                }
            }
        }


        //Command Handlers

        public void Join(CommandArgs args)
        {
            CTGCommands.Join(args, ref CurrentGame);
        }

        public void SetClass(CommandArgs args)
        {
            CTGCommands.SetClass(args, ref CurrentGame);
        }


    }
}
