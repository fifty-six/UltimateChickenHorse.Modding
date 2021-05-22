using MonoMod;
using UnityEngine;

namespace Modding.Patches
{
    [MonoModPatch("global::TurnIndicator")]
    public class TurnIndicator : global::TurnIndicator
    {
        [MonoModConstructor]
        public TurnIndicator()
        {
            PortraitPositions = new Transform[Constants.PlayerCount];
        }
    }
}