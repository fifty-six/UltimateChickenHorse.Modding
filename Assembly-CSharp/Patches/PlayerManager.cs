using MonoMod;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable UnusedMember.Local

namespace Modding.Patches
{
    // Can't inherit as it has a private constructor.
    [MonoModPatch("global::PlayerManager")]
    public class PlayerManager
    {
        [MonoModIgnore]
        public static int maxPlayers;

        [MonoModIgnore]
        private Player[] playerList;

        [MonoModConstructor]
        static PlayerManager()
        {
            maxPlayers = Constants.PlayerCount;
        }

        [MonoModConstructor]
        private extern void orig_ctor();

        [MonoModConstructor]
        private void ctor()
        {
            orig_ctor();

            maxPlayers = Constants.PlayerCount;

            playerList = new Player[Constants.PlayerCount];
        }
    }
}