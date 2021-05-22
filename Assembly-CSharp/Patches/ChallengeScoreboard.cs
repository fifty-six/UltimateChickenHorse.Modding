using MonoMod;
using UnityEngine.UI;
// ReSharper disable NotAccessedField.Global
// ReSharper disable UnusedMember.Local

namespace Modding.Patches
{
    public class ChallengeScoreboard : global::ChallengeScoreboard
    {
        [MonoModReplace]
        public new Image[] CharacterSprites = new Image[Constants.PlayerCount];
        
        [MonoModReplace]
        public new Image[] CharacterSpritesBG = new Image[Constants.PlayerCount];

        private ChallengePlayer[] players = new ChallengePlayer[Constants.PlayerCount];
    }
}