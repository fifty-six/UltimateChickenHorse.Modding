namespace Modding.Patches
{
    public class GameState : global::GameState
    {
        // ReSharper disable once NotAccessedField.Global
        public new int[] PlayerScores = new int[Constants.PlayerCount];
    }
}