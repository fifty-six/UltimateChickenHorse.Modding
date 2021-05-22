using MonoMod;
using Moserware.Skills;

// ReSharper disable NotAccessedField.Local

namespace Modding.Patches
{
    [MonoModPatch("global::LobbySkillTracker")]
    public class LobbySkillTracker : global::LobbySkillTracker
    {
        [MonoModIgnore]
        private Rating[] ratings;

        [MonoModConstructor]
        public LobbySkillTracker()
        {
            ratings = new Rating[Constants.PlayerCount];
        }
    }
}