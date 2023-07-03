using TShockAPI;

namespace HideNSeek
{
    internal class Utility
    {
        public static void UpdateSlot(TSPlayer player, int slot, int itemn, bool stackable = false, int stacksize = 0, byte prefix = 0) // CHANGE ITEM TO GIVEN
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

        public static void ClearInventory(TSPlayer Player, int[] exclusions = null)
        {
            for (int i = 0; i < 98; ++i)
            {
                if (exclusions != null)
                    if (exclusions.Contains(i)) continue;
                Utility.UpdateSlot(Player, i, 0); // ItemID.None
            }
            Player.TPlayer.trashItem.stack = 0;
            Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, Terraria.Localization.NetworkText.FromLiteral(Player.TPlayer.trashItem.Name), Player.Index, 179);
            Terraria.NetMessage.SendData((int)PacketTypes.PlayerSlot, Player.Index, -1, Terraria.Localization.NetworkText.FromLiteral(Player.TPlayer.trashItem.Name), Player.Index, 179);
            foreach (var buff in Player.TPlayer.buffType) { Player.TPlayer.ClearBuff(buff); }
        }


    }
}
