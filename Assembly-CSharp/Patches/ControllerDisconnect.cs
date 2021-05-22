using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable AccessToStaticMemberViaDerivedType

namespace Modding.Patches
{
    public class ControllerDisconnect : global::ControllerDisconnect
    {
        private static bool[] showingPrompts = new bool[Constants.PlayerCount];

        private List<InputReceiver>[] orphanedReceivers = Enumerable.Range(0, Constants.PlayerCount).Select(_ => new List<InputReceiver>()).ToArray();
        
        private Character.Animals[][] orphanedCharacters = new Character.Animals[Constants.PlayerCount][];

        private extern void orig_Start();

        private void Start()
        {
            Array.Resize(ref ConnectPrompts, Constants.PlayerCount);

            // TODO: Check if prompts are initialized here and initialize the extras.
            foreach (XboxReconnectPrompt prompt in ConnectPrompts)
            {
                Debug.LogWarning($"ControllerDisconnect ConnectPrompts[n] null?: {prompt == null}");
            }

            try
            {
                orig_Start();
            }
            catch (Exception e)
            {
                Debug.LogError("\n");
                Debug.LogError("Failure in ControllerDisconnect::orig_Start!");
                Debug.LogError(e);
                Debug.LogError("\n");
            }
        }
    }
}