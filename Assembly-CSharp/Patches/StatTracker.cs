using MonoMod;

namespace Modding.Patches
{
    public class StatTracker : global::StatTracker
    {
        [MonoModReplace]
        public new SaveFileData[] saveFiles = new SaveFileData[Constants.PlayerCount];

        [MonoModReplace]
        public new StatTracker.SaveFileStatus[] saveStatuses = new StatTracker.SaveFileStatus[Constants.PlayerCount];
    }
}