using System;
using MonoMod;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable NotAccessedField.Global

namespace Modding.Patches
{
    [MonoModPatch("global::GameSettings")]
    public class GameSettings : global::GameSettings
    {
        [MonoModConstructor]
        public GameSettings()
        {
            MaxPlayers = Constants.PlayerCount;

            Array.Resize(ref PlayerColors, Constants.PlayerCount);

            for (int i = 4; i < Constants.PlayerCount; i++)
            {
                PlayerColors[i] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            }
        }
    }
}