using MonoMod;

// ReSharper disable all

namespace Modding.Patches
{
    [MonoModPatch("global::SaveFileData")]
    public class SaveFileData : global::SaveFileData
    {
        [MonoModReplace]
        public bool get_IsCheater => false;
    }
}