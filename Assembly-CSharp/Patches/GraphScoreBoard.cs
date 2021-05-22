using MonoMod;
using UnityEngine;

// ReSharper disable once NotAccessedField.Global

namespace Modding.Patches
{
    [MonoModPatch("global::GraphScoreBoard")]
    public class GraphScoreBoard : global::GraphScoreBoard
    {
        [MonoModConstructor]
        public GraphScoreBoard()
        {
            ScorePositions = new RectTransform[Constants.PlayerCount];
        }
    }
}