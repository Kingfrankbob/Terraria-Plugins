using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;

namespace CaptureTheGem
{
    internal class GameEventHandler
    {
        public static void OnGetPvpToggle(GetDataEventArgs args)
        {
            if (args.MsgID == PacketTypes.TogglePvp)
            {

                var player = TShock.Players[args.Msg.whoAmI];

                if (!player.Group.HasPermission("pvp.toggle"))
                {

                    args.Handled = true;
                    player.SetPvP(true, false);
                    player.SendErrorMessage("You do not have permission to change this setting");

                }

            }

        }
        public static void ForFireWorkLaunchRed(ref GameData CurrentGame)
        {
            var now = DateTime.Now;
            if (CurrentGame.RedLaunch)
            {
                if (CurrentGame.IsTeamGemHeld(1))
                {
                    CurrentGame.RedLaunch = false;
                    CurrentGame.RedTime = now.AddSeconds(5);

                    TSPlayer? player = CurrentGame.RedHolder();
                    if (player == null) return;

                    Terraria.Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(),
                        X: player.X, Y: player.Y, SpeedX: 0f, SpeedY: -8f, Type: 167, Damage: 0, KnockBack: 0, Owner: Terraria.Main.myPlayer);
                }
            }
            else
            {
                if (now >= CurrentGame.RedTime)
                {
                    CurrentGame.RedLaunch = true;
                }
            }
        }

        public static void ForFireWorkLaunchBlue(ref GameData CurrentGame)
        {
            var now = DateTime.Now;
            if (CurrentGame.BlueLaunch)
            {
                if (CurrentGame.IsTeamGemHeld(3))
                {
                    CurrentGame.BlueLaunch = false;
                    CurrentGame.Bluetime = now.AddSeconds(5);

                    TSPlayer? player = CurrentGame.BlueHolder();
                    if (player == null) return;

                    Terraria.Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(),
                        X: player.X, Y: player.Y, SpeedX: 0f, SpeedY: -8f, Type: 169, Damage: 0, KnockBack: 0, Owner: Terraria.Main.myPlayer);
                }
            }
            else
            {
                if (now >= CurrentGame.Bluetime)
                {
                    CurrentGame.BlueLaunch = true;
                }
            }
        }


        public static void OnGameDeathHandler(ref GameData CurrentGame)
        {

            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null || !player.IsLoggedIn || !player.TPlayer.dead)
                    continue;

                if (player.Team == 1)
                {

                    if (CurrentGame.IsTeamGemHeld(3) && player.Name == CurrentGame.BlueHolder().Name)
                    {

                        CurrentGame.SetBlueGemHolder(null);
                        TShock.Utils.Broadcast(player.Name + " has dropped the Blue Gem!", Microsoft.Xna.Framework.Color.Yellow);

                    }

                    Utility.RevivePlayer(player);

                }

                if (player.Team == 3)
                {

                    if (CurrentGame.IsTeamGemHeld(1) && player.Name == CurrentGame.RedHolder().Name)
                    {

                        CurrentGame.SetRedGemHolder(null);
                        TShock.Utils.Broadcast(player.Name + " has dropped the Red Gem!", Microsoft.Xna.Framework.Color.Yellow);

                    }

                    Utility.RevivePlayer(player);
                    
                }

            }
        }


    }
}
