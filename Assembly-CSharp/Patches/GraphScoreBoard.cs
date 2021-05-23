using System;
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

        public extern void orig_SetPlayerCount(int numberPlayers);

        public new void SetPlayerCount(int numberPlayers)
        {
            // Gets reset after the constructor runs
            if (ScorePositions.Length < Constants.PlayerCount)
            {
                Array.Resize(ref ScorePositions, Constants.PlayerCount);

                for (int i = 4; i < Constants.PlayerCount; i++)
                {
                    // TODO: Actually fix positions...
                    ScorePositions[i] = ScorePositions[i - 1];
                }
            }
            
            orig_SetPlayerCount(numberPlayers);
        }
    }
}