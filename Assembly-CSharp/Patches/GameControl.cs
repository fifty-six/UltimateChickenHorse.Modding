using MonoMod;

namespace Modding.Patches
{
    [MonoModPatch("global::GameControl")]
    public class GameControl : global::GameControl
    {
        public static int GetPlayerNumber(InputEvent e)
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
        }
    }
}