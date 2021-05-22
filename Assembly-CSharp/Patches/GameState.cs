using MonoMod;

// ReSharper disable NotAccessedField.Global

namespace Modding.Patches
{
    [MonoModPatch("global::GameState")]
    public class GameState : global::GameState
    {
        [MonoModIgnore]
        public new int[] PlayerScores;

        [MonoModConstructor]
        public GameState()
        {
            PlayerScores = new int[Constants.PlayerCount];
        }
    }
}