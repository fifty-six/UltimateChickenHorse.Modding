using System;
using MonoMod;
using UnityEngine;

namespace Modding.Patches
{
    [MonoModPatch("global::LobbyManagerManager")]
    public class LobbyManagerManager : global::LobbyManagerManager
    {
        public static extern void orig_AbortGameInProgressGracefully(string abortReason);
        
        public new static void AbortGameInProgressGracefully(string abortReason)
        {
            Debug.LogError(Environment.StackTrace);
            
            // FIXME / TODO: Re-enable
            // orig_AbortGameInProgressGracefully(abortReason);
        }
    }
}