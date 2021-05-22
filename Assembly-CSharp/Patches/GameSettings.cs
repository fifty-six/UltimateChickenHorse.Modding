using MonoMod;

// ReSharper disable NotAccessedField.Global

namespace Modding.Patches
{
    [MonoModPatch("global::GameSettings")]
    public class GameSettings : global::GameSettings
    {
        [MonoModIgnore]
        public new int MaxPlayers;

        [MonoModConstructor]
        public GameSettings()
        {
            MaxPlayers = Constants.PlayerCount;
        }
    }
}