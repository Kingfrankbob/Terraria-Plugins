using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using TShockAPI.Hooks;
using Terraria.ID;

namespace CaptureTheGem
{
    internal class GameData
    {
        public Dictionary<string, int> PlayerChoiceList = new Dictionary<string, int>();

        public List<TSPlayer> GamePlayerList = new List<TSPlayer>();
        public List<TSPlayer> Lobby = new List<TSPlayer>();
        private List<TSPlayer> _redTeamPlayers = new List<TSPlayer>();
        private List<TSPlayer> _blueTeamPlayers = new List<TSPlayer>();

        public DateTime RedTime;
        public DateTime Bluetime;

        public TSPlayer? _redGemHolder = null;
        public TSPlayer? _blueGemHolder = null;

        public bool RedLaunch = false;
        public bool BlueLaunch = false;
        public bool GameOn = false;

        public enum Classes { Warrior, Tree, Ancient_One, Archer, Mage, Wandering, Wayne };

        public void AddRedPlayer(TSPlayer player)
        {
            _redTeamPlayers.Add(player);
        }

        public void AddBluePlayer(TSPlayer player)
        {
            _blueTeamPlayers.Add(player);
        }

        public bool IsPlaying(TSPlayer player)
        {
            return GamePlayerList.Contains(player);
        }

        public bool IsPlayingRed(TSPlayer player)
        {
            return _redTeamPlayers.Contains(player);
        }

        public bool IsPlayingBlue(TSPlayer player)
        {
            return _blueTeamPlayers.Contains(player);
        }

        public bool IsTeamGemHeld(int team)
        {
            switch (team)
            {

                case 1:

                    if (_redGemHolder == null) return false;
                    else return true;

                case 3:

                    if (_blueGemHolder == null) return false;
                    else return true;

            }

            return false;
        }

        public void SetBlueGemHolder(TSPlayer player)
        {
            _blueGemHolder = player;
        }

        public void SetRedGemHolder(TSPlayer player)
        {
            _redGemHolder = player;
        }

        public TSPlayer? RedHolder()
        {
            return _redGemHolder;
        }

        public TSPlayer? BlueHolder()
        {
            return _blueGemHolder;
        }

        public bool IsPlayingAlready(TSPlayer player)
        {
            return GamePlayerList.Contains(player);
        }

        public bool IsLobbiedAlready(TSPlayer player)
        {
            return Lobby.Contains(player);
        }

        public void RemoveAndAddChoice(string name, Int16 choice)
        {
            if (PlayerChoiceList.ContainsKey(name))
                PlayerChoiceList.Remove(name);

            PlayerChoiceList.Add(name, choice);
        }

        public void Clear()
        {
            PlayerChoiceList = new Dictionary<string, int>();

            GamePlayerList = new List<TSPlayer>();
            Lobby = new List<TSPlayer>();
            _redTeamPlayers = new List<TSPlayer>();
            _blueTeamPlayers = new List<TSPlayer>();

            _redGemHolder = null;
            _blueGemHolder = null;

            RedLaunch = false;
            BlueLaunch = false;
            GameOn = false;
        }

    }
}
