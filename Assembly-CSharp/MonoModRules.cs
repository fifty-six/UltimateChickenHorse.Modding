using System;
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
        private static void ReplaceLdcPlayerCt(string type, string method)
        {
            TypeDefinition td = MonoModRule.Modder.FindType(type)?.Resolve();

            if (td is null)
                throw new ArgumentNullException($"Unable to resolve type {type}!");

            MethodDefinition md = td.FindMethod(method);

            if (md is not { HasBody: true })
            {
                if (md is null)
                    throw new ArgumentNullException($"Unable to resolve method {method} on type {type}!");

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
        static MonoModRules()
        {
            MonoModder modder = MonoModRule.Modder;
            
            TypeDefinition ChallengeScoreboard = modder.FindType("ChallengeScoreboard")?.Resolve();

            MethodDefinition csb_showRes;

            ReplaceLdcPlayerCt(nameof(ControllerDisconnect), "assignControllerToPlayer");
        }
    }
}