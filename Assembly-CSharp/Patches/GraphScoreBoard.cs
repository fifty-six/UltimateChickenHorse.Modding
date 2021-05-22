using UnityEngine;

namespace Modding.Patches
{
    public class GraphScoreBoard : global::GraphScoreBoard
    {
        public new RectTransform[] ScorePositions = new RectTransform[Constants.PlayerCount];
    }
}