using System.Collections.Generic;
using MonoMod;

namespace Modding.Patches
{
    [MonoModPatch("globa::Tablet")]
    public class Tablet : global::Tablet
    {
        [MonoModIgnore]
        private List<PickCursor> untrackedCursors;

        [MonoModConstructor]
        public Tablet()
        {
            untrackedCursors = new List<PickCursor>(Constants.PlayerCount);
        }
    }
}