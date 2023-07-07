using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;


namespace CaptureTheGem
{
    internal class CTGClasses
    {
        public static void ClassOne(TSPlayer player) // Melee Class 
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

        }

        public static void ClassTwo(TSPlayer player)// Tree Class 
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

        public static void Class3559(TSPlayer player)// Developer Class
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

        public static void Class69(TSPlayer player)// TODO ~ Needs to be added 
        {

        }

        public static void ClassThree(TSPlayer player)// Ancient One Class
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

        public static void ClassFour(TSPlayer player)// Archer Class
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

        public static void ClassFive(TSPlayer player)// Mage Class
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

        public static void ClassSix(TSPlayer player)// Wandering Ninja Class
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

        public static void ClassSeven(TSPlayer player)// Wayne from Brandon Sanderson Class
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


    }
}
