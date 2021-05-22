using MonoMod;
using UnityEngine;

// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modding.Patches
{
    [MonoModPatch("global::Scoreboard")]
    public class Scoreboard : global::Scoreboard
    {
        public Scoreboard()
        {
            ScorePositions = new Transform[Constants.PlayerCount];
        }
    }
}