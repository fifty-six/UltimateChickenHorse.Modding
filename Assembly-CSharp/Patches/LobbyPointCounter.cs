using MonoMod;
// ReSharper disable NotAccessedField.Local

namespace Modding.Patches
{
    [MonoModPatch("global::LobbyPointCounter")]
    public class LobbyPointCounter : global::LobbyPointCounter
    {
        [MonoModIgnore]
        private bool[] playerJoinedGame;

        [MonoModIgnore]
        private bool[] playerPlayedGame;

        [MonoModIgnore]
        private bool[] playerAFK;

        public LobbyPointCounter()
        {
            playerJoinedGame = new bool[Constants.PlayerCount];
            playerPlayedGame = new bool[Constants.PlayerCount];
            playerAFK = new bool[Constants.PlayerCount];
        }
    }
}