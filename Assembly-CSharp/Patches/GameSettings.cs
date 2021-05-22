using MonoMod;

namespace Modding.Patches
{
    public class GameSettings : global::GameSettings
    {
        [MonoModReplace]
        // ReSharper disable once NotAccessedField.Global
        public new int MaxPlayers = Constants.PlayerCount;
    }
}