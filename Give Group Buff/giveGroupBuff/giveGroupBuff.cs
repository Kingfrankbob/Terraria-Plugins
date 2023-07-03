using TShockAPI;
using TerrariaApi.Server;

namespace giveGroupBuff
{
    [ApiVersion(2, 1)]
    public class giveGroupBuff : TerrariaPlugin
    {
        public override string Name => "¡Predeterminado dando el beneficio a un grupo determinado!";
        public giveGroupBuff(Terraria.Main game) : base(game)
        {
        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameUpdate.Register(this, onGameUpdate);
        }
        private void onGameUpdate(EventArgs args)
        {
            try
            {
                foreach (var player in TShock.Players)
                {
                    if (player == null || !player.Active) continue;

                    switch (player.Group.Name)
                    {
                        case "héroe":
                            player.SetBuff(113, 999999999);
                            player.SetBuff(257, 999999999);
                            player.SetBuff(48, 999999999);
                            player.SetBuff(3, 999999999);
                            player.SetBuff(206, 999999999);
                            break;

                        case "default":
                            player.SetBuff(87, 999999999);
                            break;

                        case "magma":
                            player.SetBuff(5, 999999999);
                            player.SetBuff(74, 999999999);
                            player.SetBuff(115, 999999999);
                            break;

                        case "veterano":
                            player.SetBuff(5, 999999999);
                            player.SetBuff(2, 999999999);
                            break;

                        case "tétrico":
                            player.SetBuff(73, 999999999);
                            player.SetBuff(114, 999999999);
                            player.SetBuff(165, 999999999);
                            player.SetBuff(12, 999999999);
                            break;

                        case "nigromancia":
                            player.SetBuff(151, 999999999);
                            player.SetBuff(150, 999999999);
                            player.SetBuff(165, 999999999);
                            player.SetBuff(120, 999999999);
                            player.SetBuff(26, 999999999); break;

                        case "lunar":
                            player.SetBuff(176, 999999999);
                            player.SetBuff(174, 999999999);
                            player.SetBuff(165, 999999999);
                            player.SetBuff(257, 999999999);
                            player.SetBuff(207, 999999999);
                            player.SetBuff(113, 999999999); break;

                        case "constructor":
                            player.SetBuff(107, 999999999);
                            break;
                        default: // Do Nothing
                            break;

                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public override Version Version => new Version(1, 0, 0);
        public override string Author => "Zum";
        public override string Description => "A simple plugin for someone :)";
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }

    }
}
