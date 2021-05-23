using System;
using System.Collections.Generic;
using System.Linq;
using MonoMod;
using UnityEngine;

// ReSharper disable NotAccessedField.Local
// ReSharper disable AccessToStaticMemberViaDerivedType

namespace Modding.Patches
{
    [MonoModPatch("global::ControllerDisconnect")]
    public class ControllerDisconnect : global::ControllerDisconnect
    {
        [MonoModIgnore]
        private static bool[] showingPrompts;

        [MonoModIgnore]
        private List<InputReceiver>[] orphanedReceivers;

        [MonoModIgnore]
        private Character.Animals[][] orphanedCharacters;

        [MonoModConstructor]
        public ControllerDisconnect()
        {
            showingPrompts = new bool[Constants.PlayerCount];

            orphanedReceivers = Enumerable.Range(0, Constants.PlayerCount).Select(_ => new List<InputReceiver>()).ToArray();

            orphanedCharacters = new Character.Animals[Constants.PlayerCount][];
        }

        private extern void orig_Start();

        private void Start()
        {
            Array.Resize(ref ConnectPrompts, Constants.PlayerCount);

            // TODO: Check if prompts are initialized here and initialize the extras.
            foreach (XboxReconnectPrompt prompt in ConnectPrompts)
            {
                Debug.LogWarning($"ControllerDisconnect ConnectPrompts[n] null?: {prompt == null}");
            }

            for (int i = 4; i < Constants.PlayerCount; i++)
            {
                if (ConnectPrompts[i]) 
                    continue;
                
                ConnectPrompts[i] = Instantiate(ConnectPrompts[i - 1]);
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