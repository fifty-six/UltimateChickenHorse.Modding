using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Modding;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.InlineRT;
using MonoMod.Utils;

// ReSharper disable once CheckNamespace
namespace MonoMod
{
    [UsedImplicitly]
    public static class MonoModRules
    {
        private static MethodDefinition GetMethod(string type, string method)
        {
            TypeDefinition td = MonoModRule.Modder.FindType(type)?.Resolve();

            if (td is null)
                throw new ArgumentNullException($"Unable to resolve type {type}!");

            MethodDefinition md = td.FindMethod(method);

            if (md is null)
                throw new ArgumentNullException($"Unable to resolve method {method} on type {type}!");

            return md;
        }

        private static void ReplaceLdcPlayerCt
        (
            string type,
            string method,
            int input = 4,
            int result = Constants.PlayerCount
        )
        {
            MethodDefinition md = GetMethod(type, method);

            if (!md.HasBody)
            {
                throw new InvalidOperationException($"Method {method} has no body!");
            }

            var cursor = new ILCursor(new ILContext(md));

            while (cursor.TryGotoNext(x => x.MatchLdcI4(input)))
            {
                cursor.Remove();
                cursor.Emit(OpCodes.Ldc_I4, result);
            }
        }

        private static void ReplaceFirstLdcPlayerCt
        (
            string type,
            string method,
            int input = 4,
            int result = Constants.PlayerCount
        )
        {
            MethodDefinition md = GetMethod(type, method);

            if (!md.HasBody)
            {
                throw new InvalidOperationException($"Method {method} has no body!");
            }

            var cursor = new ILCursor(new ILContext(md));

            cursor.GotoNext(x => x.MatchLdcI4(input));
            cursor.Remove();
            cursor.Emit(OpCodes.Ldc_I4, result);
        }

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
        static MonoModRules()
        {
            ReplaceLdcPlayerCt(nameof(UnityMatchmaker), "onLobbyJoined");
            ReplaceLdcPlayerCt(nameof(UnityMatchmaker), "CheckHostConnectivity");

            ReplaceLdcPlayerCt(nameof(SteamMatchmaker), "createSocialLobby");

            ReplaceLdcPlayerCt(nameof(ChallengeScoreboard), "showResult");

            ReplaceLdcPlayerCt(nameof(ControllerDisconnect), "assignControllerToPlayer");

            ReplaceLdcPlayerCt(nameof(LobbyPointCounter), nameof(LobbyPointCounter.handleEvent));

            // Can't replace all - we index into an array at predefined index [4] in array of length 8, so array[8] throws.
            ReplaceFirstLdcPlayerCt(nameof(PartyBox), nameof(PartyBox.AddPlayer));
            ReplaceLdcPlayerCt(nameof(PartyBox), nameof(PartyBox.SetPlayerCount));

            ReplaceLdcPlayerCt(nameof(Scoreboard), nameof(Scoreboard.GetPlayerScore), 3);
            ReplaceLdcPlayerCt(nameof(Scoreboard), nameof(Scoreboard.IncrementPlayerScore), 3);
            ReplaceLdcPlayerCt(nameof(Scoreboard), nameof(Scoreboard.SetPlayerScore), 3);
            ReplaceLdcPlayerCt(nameof(Scoreboard), nameof(Scoreboard.SetPlayerCount));
            ReplaceLdcPlayerCt(nameof(Scoreboard), nameof(Scoreboard.SetPlayerCharacter), 3);

            ReplaceLdcPlayerCt(nameof(TurnIndicator), nameof(TurnIndicator.SetPlayerCount));
            ReplaceLdcPlayerCt(nameof(TurnIndicator), nameof(TurnIndicator.SetPlayerCharacter), 3);
            ReplaceLdcPlayerCt(nameof(TurnIndicator), nameof(TurnIndicator.SwapPhase), 3);

            var modder = MonoModRule.Modder;

            // We need to handle some stuff in post as we add new members which won't get recognized here.
            modder.PostProcessors += PostProcess;

            foreach (MethodDefinition method in modder.Module.Types.SelectMany(type => type.Methods))
            {
                method.FixShortLongOps();
            }
        }

        private static void PostProcess(MonoModder modder)
        {
            // GameControl.ReceiveEvent
            {
                MethodDefinition md = GetMethod(nameof(GameControl), nameof(GameControl.ReceiveEvent));

                var cursor = new ILCursor(new ILContext(md));

                cursor.GotoNext
                (
                    MoveType.After,
                    x => x.MatchLdcI4(4),
                    x => x.MatchStfld<GameControl>("inputPlayerNumber")
                );

                // Move the cursor out of the else if, one instruction into the sequence after
                // This way we're after the ldarg.1 (e) in `e.Key`, so the branches of our if
                // has it point at our code first.
                cursor.Index += 1;

                // Instance for stfld.
                cursor.Emit(OpCodes.Ldarg_0);

                // InputEvent for delegate
                cursor.Emit(OpCodes.Ldarg_1);

                cursor.Emit(OpCodes.Call, GetMethod("GameControl", "GetPlayerNumber"));

                cursor.Emit<GameControl>(OpCodes.Stfld, "inputPlayerNumber");
            }
        }
    }
}