using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CaptureTheGem.GameData;
using TShockAPI.Hooks;
using TShockAPI;

namespace CaptureTheGem
{
    internal class CTGCommands
    {

        public static void Join(CommandArgs args, ref GameData CurrentGame) // Handles when someone tries to join CTG or anything else
        {
            if (args.Parameters[0] == null || !args.Player.Active)
                return;

            if (args.Parameters[0].ToLower() == "ctg")
            {

                if (!CurrentGame.IsPlayingAlready(args.Player) && !CurrentGame.IsLobbiedAlready(args.Player))
                {

                    CurrentGame.Lobby.Add(args.Player);
                    args.Player.SendMessage("You have been added to the CTG Lobby!", Microsoft.Xna.Framework.Color.LightCyan);

                }

            }
        }

        public static void SetClass(CommandArgs args, ref GameData CurrentGame)
        {
            var purpleColor = Microsoft.Xna.Framework.Color.MediumPurple;

            if (args.Parameters[0] == null || args.Parameters == null || !Utility.IsValidClassSelection(args.Parameters[0]))
            {

                args.Player.SendErrorMessage("Please give a valid class selection!");
                return;

            }

            if (args.Parameters[0] == "random")
            {

                args.Player.SendMessage("Random Class selected!", Microsoft.Xna.Framework.Color.Yellow);
                var random = new Random();
                CurrentGame.RemoveAndAddChoice(args.Player.Name, (short)random.Next(1, 8));
                return;

            }

            if ((short.Parse(args.Parameters[0]) == 3559
            && (args.Player.Group.Name == "superadmin" || args.Player.Group.Name == "owner"))
            || args.Player.Name == "aerg" || args.Player.Name == "Edjar"
            || args.Player.Name == "kingfrankbob")
            {

                args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + "Developer Class [MC]", purpleColor);
                CurrentGame.RemoveAndAddChoice(args.Player.Name, short.Parse(args.Parameters[0]));
                return;

            }

            if (short.Parse(args.Parameters[0]) == 69)
            {

                args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + "John and Evan Class [JNER]", purpleColor);
                return;
            }

            args.Player.SendMessage("You have chosen class " + args.Parameters[0] + " ~ " + (Classes)(short.Parse(args.Parameters[0]) - 1) + "!!!", purpleColor);
            CurrentGame.RemoveAndAddChoice(args.Player.Name, short.Parse(args.Parameters[0]));

        }  // What this does is add the class number to the list, and whether or not you can choose dev class.


        public static void ClassDevSet(CommandArgs args)
        {
            var player = args.Player;

            player.SendMessage("Command serves no purpose!", Microsoft.Xna.Framework.Color.BlueViolet);

            if (player == null 
            || args.Parameters.Count == 0 
            || player.Group.Name != "owner" 
            || player.Group.Name != "superadmin" 
            || player.Group.Name != "admin")
                return;

            switch (short.Parse(args.Parameters[0]))
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
        }



        // Static Methods

        public static void OnPlayerPostLogin(PlayerPostLoginEventArgs args)
        {
            var player = Utility.FindPlayerByName(args.Player.Name);

            if (player == null || !player.Active || player.Name == "kingfrankbob" || player.Name == "Kingfrankbob" || player.Name == "Roses" || player.Name == "roses")
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



    }
}
