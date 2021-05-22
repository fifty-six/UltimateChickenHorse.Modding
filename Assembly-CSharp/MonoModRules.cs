using System;
using System.Diagnostics.CodeAnalysis;
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

        private static void ReplaceLdcPlayerCt(string type, string method)
        {
            MethodDefinition md = GetMethod(type, method);
            
            if (!md.HasBody)
            {
                throw new InvalidOperationException($"Method {method} has no body!");
            }

            var cursor = new ILCursor(new ILContext(md));

            while (cursor.TryGotoNext(x => x.MatchLdcI4(4)))
            {
                cursor.Remove();
                cursor.Emit(OpCodes.Ldc_I4, Constants.PlayerCount);
            }
        }

        [UsedImplicitly]
        [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
        static MonoModRules()
        {
            ReplaceLdcPlayerCt(nameof(ChallengeScoreboard), "showResult");

            ReplaceLdcPlayerCt(nameof(ControllerDisconnect), "assignControllerToPlayer");

            {
                var md = GetMethod(nameof(GameControl), nameof(GameControl.ReceiveEvent));

                var cursor = new ILCursor(new ILContext(md));

                cursor.GotoNext(
                    MoveType.After,
                    x => x.MatchLdcI4(4),
                    x => x.MatchStfld<GameControl>("inputPlayerNumber")
                );

                // Instance for stfld.
                cursor.Emit(OpCodes.Ldarg_0);
                
                // InputEvent for delegate
                cursor.Emit(OpCodes.Ldarg_1);
                 
                cursor.EmitDelegate<Func<InputEvent, int>>(e =>
                {
                    int res = 0;
                    
                    for (int i = 1; i <= Constants.PlayerCount; i++)
                    {
                        int pow = 1 << (i - 1);
                        
                        if ((e.PlayerBitMask & pow) == pow)
                        {
                            res = i;
                        }
                    }

                    return res;
                });

                cursor.Emit<GameControl>(OpCodes.Stfld, "inputPlayerNumber");
            }
        }
    }
}