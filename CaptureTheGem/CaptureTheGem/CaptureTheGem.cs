using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;


namespace CaptureTheGem
{
    [ApiVersion(2, 1)]
    public class CaptureTheGem : TerrariaPlugin
    {
        private GameHandler CurrentHandler = new GameHandler();
        public CaptureTheGem(Terraria.Main game) : base(game)
        {
            
        }

        public override void Initialize()
        {

            GetDataHandlers.PlayerTeam += CurrentHandler.PlayerTeamChangeHandler;
            PlayerHooks.PlayerPostLogin += CTGCommands.OnPlayerPostLogin;

            ServerApi.Hooks.GameUpdate.Register(this, CurrentHandler.GemUpdateHandler);
            ServerApi.Hooks.GameUpdate.Register(this, CurrentHandler.HandleGameEvents);
            ServerApi.Hooks.GameUpdate.Register(this, CurrentHandler.TimeChanger);
            ServerApi.Hooks.GameUpdate.Register(this, CurrentHandler.AutoStart);

            ServerApi.Hooks.NetGetData.Register(this, GameEventHandler.OnGetPvpToggle);

            Commands.ChatCommands.Add(new Command("CTG.j", CurrentHandler.Join, "j", "join", "J"));
            Commands.ChatCommands.Add(new Command("CTG.cl", CurrentHandler.SetClass, "class"));
            Commands.ChatCommands.Add(new Command("CTG.co", CTGCommands.ClassDevSet, "classone", "co"));
        }


        public override Version Version => new Version(1, 0, 0);

        public override string Author => "Michael Cragun";

        public override string Description => "A simple re-written version that hopefully is more clear";

        public override string Name => "CTG Plugin";

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}