using MonoMod;
using UnityEngine;

// ReSharper disable once NotAccessedField.Global

namespace Modding.Patches
{
    [MonoModPatch("global::GraphScoreBoard")]
    public class GraphScoreBoard : global::GraphScoreBoard
    {
        [MonoModIgnore]
        public new RectTransform[] ScorePositions;

        [MonoModConstructor]
        public GraphScoreBoard()
        {
            ScorePositions = new RectTransform[Constants.PlayerCount];
        }
    }
}