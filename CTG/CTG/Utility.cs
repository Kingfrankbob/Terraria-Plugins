using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;



namespace CTG
{
    internal class Utility
    {
        public static void ClearInventory(TSPlayer Player, int[]? exclusions  = null)// Clear Players Inventory
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
            foreach (var buff in Player.TPlayer.buffType) { Player.TPlayer.ClearBuff(buff); }
        } // Finished Cleaning

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
        } // Finished Cleaning

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
        } // Finished Cleaning

        public static void GetPos(CommandArgs args)// Prints X & Y of player in the console
        {
            var Player = args.Player;
            Console.WriteLine("X: " + Player.X + " Y:" + Player.Y + " Player: " + Player.Name);
            Console.WriteLine("Tile X: " + Player.X / 16 + " Y:" + Player.Y / 16 + " Player: " + Player.Name);
        }  // Finsihed Cleaning

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
    }
}
