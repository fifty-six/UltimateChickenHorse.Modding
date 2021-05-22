using MonoMod;

namespace Modding.Patches
{
    [MonoModPatch("global::StatTracker")]
    public class StatTracker : global::StatTracker
    {
        [MonoModConstructor]
        public StatTracker()
        {
            saveFiles = new global::SaveFileData[Constants.PlayerCount];
            saveStatuses = new SaveFileStatus[Constants.PlayerCount];
        }
    }
}