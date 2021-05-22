using System.Collections.Generic;
using MonoMod;

namespace Modding.Patches
{
    public class Tablet : global::Tablet

    {
        [MonoModReplace]
        private List<PickCursor> untrackedCursors = new List<PickCursor>(Constants.PlayerCount);
    }
}