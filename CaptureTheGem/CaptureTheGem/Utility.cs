using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;

namespace CaptureTheGem
{
    internal class Utility
    {

        public static void ClearInventory(TSPlayer Player, int[]? exclusions = null)// Clear Players Inventory
        {
            for (int i = 0; i < 98; ++i)
            {

                if (exclusions != null)
                    if (exclusions.Contains(i)) continue;

                UpdateSlot(Player, i, ItemID.None);
            }

            Player.TPlayer.trashItem.stack = 0;

            Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, Terraria.Localization.NetworkText.FromLiteral(Player.TPlayer.trashItem.Name), Player.Index, 179);
            Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, Player.Index, -1, Terraria.Localization.NetworkText.FromLiteral(Player.TPlayer.trashItem.Name), Player.Index, 179);

            foreach (Int32 buff in Player.TPlayer.buffType) { Player.TPlayer.ClearBuff(buff); }
        }

        public static void ApplyDye(TSPlayer player)// Set Team Dyes
        {
            int teamDye = 0;
            if (player.Team == 1) teamDye = ItemID.FlameDye;
            else teamDye = ItemID.BlueFlameDye;

            UpdateSlot(player, 79, teamDye);
            UpdateSlot(player, 80, teamDye);
            UpdateSlot(player, 81, teamDye);
            UpdateSlot(player, 82, teamDye);
            UpdateSlot(player, 83, teamDye);
            UpdateSlot(player, 84, teamDye);
            UpdateSlot(player, 85, teamDye);
            UpdateSlot(player, 86, teamDye);
            UpdateSlot(player, 87, teamDye);
            UpdateSlot(player, 94, teamDye);
            UpdateSlot(player, 95, teamDye);
            UpdateSlot(player, 96, teamDye);
            UpdateSlot(player, 97, teamDye);
            UpdateSlot(player, 98, teamDye);

            if (player.Team == 1)
                UpdateSlot(player, 49, ItemID.DeepRedPaint, true, 999);
            else if (player.Team == 3)
                UpdateSlot(player, 49, ItemID.BluePaint, true, 999);
            else
                UpdateSlot(player, 49, ItemID.WhitePaint, true, 999);
        }

        public static void UpdateSlot(TSPlayer player, int slot, int itemn, bool stackable = false, int stacksize = 0, byte prefix = 0) // Change Inv. Slot to item
        {
            int index;
            var item = TShock.Utils.GetItemById(itemn);
            item.prefix = prefix;

            if (slot < NetItem.InventorySlots)
            {
                index = slot;
                player.TPlayer.inventory[slot] = item;
                if (stackable)
                    player.TPlayer.inventory[slot].stack = stacksize;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.inventory[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.inventory[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.inventory[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.inventory[index].prefix);
            }
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots) //Armor & Accessory slots
            {
                index = slot - NetItem.InventorySlots;
                player.TPlayer.armor[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.armor[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.armor[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.armor[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.armor[index].prefix);
            }
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots) //Dye slots
            {
                index = slot - (NetItem.InventorySlots + NetItem.ArmorSlots);
                player.TPlayer.dye[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.dye[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.dye[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.dye[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.dye[index].prefix);
            }
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots) //Misc Equipment slots
            {
                index = slot - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots);
                player.TPlayer.miscEquips[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscEquips[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscEquips[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscEquips[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscEquips[index].prefix);
            }
            else if (slot < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots) //Misc Dyes slots
            {
                index = slot - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots);
                player.TPlayer.miscDyes[index] = item;

                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscDyes[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscDyes[index].prefix);
                Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, new Terraria.Localization.NetworkText(player.TPlayer.miscDyes[index].Name, Terraria.Localization.NetworkText.Mode.Literal), player.Index, slot, player.TPlayer.miscDyes[index].prefix);
            }
        }

        public static void GetPos(CommandArgs args)// Prints X & Y of player in the console
        {
            var Player = args.Player;
            Console.WriteLine("X: " + Player.X + " Y:" + Player.Y + " Player: " + Player.Name);
            Console.WriteLine("Tile X: " + Player.X / 16 + " Y:" + Player.Y / 16 + " Player: " + Player.Name);
        }

        public static void UpdateArena()
        {
            int sourceX = 161, sourceY = 315, destX = 1949, destY = 288, width = 304, height = 41;

            for (int x = sourceX; x < sourceX + width; x++)
            {
                for (int y = sourceY; y < sourceY + height; y++)
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

            TSPlayer.All.SendTileRect((short)destX, (short)destY, 100, 41);
            TSPlayer.All.SendTileRect((short)(destX + 100), (short)(destY), 100, 41);
            TSPlayer.All.SendTileRect((short)(destX + 200), (short)(destY), 100, 41);
            TSPlayer.All.SendTileRect((short)(destX + 300), (short)(destY), 4, 41);
        }

        public static bool IsValidClassSelection(string number)
        {
            if (number.ToLower() == "random") return true;

            Int16.TryParse(number, out var resultNumber);

            bool isValid = false;

            if (resultNumber > 0 && resultNumber < 8) { isValid = true; }
            if (resultNumber == 3559 || resultNumber == 69) { isValid = true; }

            return isValid;
        }

        public static TSPlayer? FindPlayerByName(string playerName)
        {
            return TShock.Players.FirstOrDefault(player => player?.Name == playerName);
        }

        public static void WinGame(int winTeam)
        {
            var loseTeam = (winTeam == 1) ? 3 : 1;
            
            foreach (TSPlayer player in TShock.Players)
            {

                if (!player.Active || player == null || player.Team == 0) continue;

                if (player.Team == winTeam)
                {
                    Utility.UpdateSlot(player, 0, 1000, true, 20);
                    player.SetPvP(false);
                    player.SetTeam(0);
                }

                if (player.Team == loseTeam)
                {
                    Utility.ClearInventory(player);
                    player.KillPlayer();
                    player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                    player.SetPvP(false);
                    player.SetTeam(0);
                }

            }
            return;
        }

        public static async void RevivePlayer(TSPlayer player)
        {
            if (player.Team == 1)
            {
                player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                await Task.Delay(TimeSpan.FromSeconds(1));
                player.Teleport((float)36853.426, 4982, 2);
            }
            if (player.Team == 3)
            {
                player.Spawn(PlayerSpawnContext.ReviveFromDeath);
                await Task.Delay(TimeSpan.FromSeconds(1));
                player.Teleport((float)30359, 4982, 2);
            }
        }
    }
}

