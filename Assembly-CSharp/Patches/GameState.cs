using MonoMod;

// ReSharper disable NotAccessedField.Global

namespace Modding.Patches
{
    [MonoModPatch("global::GameState")]
    public class GameState : global::GameState
    {
        [MonoModConstructor]
        public GameState()
        {
            PlayerScores = new int[Constants.PlayerCount];
        }
    }
}