using System;
using MonoMod;
using UnityEngine.UI;

// ReSharper disable NotAccessedField.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local

namespace Modding.Patches
{
    [MonoModPatch("global::ChallengeScoreboard")]
    public class ChallengeScoreboard : global::ChallengeScoreboard
    {
        [MonoModIgnore]
        private ChallengePlayer[] players;

        [MonoModConstructor]
        public ChallengeScoreboard()
        {
            CharacterSprites = new Image[Constants.PlayerCount];
            CharacterSpritesBG = new Image[Constants.PlayerCount];
            players = new ChallengePlayer[Constants.PlayerCount];

            Console.WriteLine("sonia play uch");
        }
    }
}