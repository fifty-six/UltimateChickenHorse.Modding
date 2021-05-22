using System.Collections.Generic;
using System.Linq;
using MonoMod;
// ReSharper disable UnusedMember.Global

namespace Modding.Patches
{
    // ReSharper disable once UnusedType.Global
    public class KickTracker : global::KickTracker
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private bool[][] votes;
        
        [MonoModReplace]
        public KickTracker()
        {
            votes = new bool[Constants.PlayerCount][];

            for (int i = 0; i < Constants.PlayerCount; i++)
            {
                votes[i] = new bool[Constants.PlayerCount];
            }
        }

        [MonoModReplace]
        public new void ClearPlayer(int player)
        {
            for (int i = 0; i < Constants.PlayerCount; i++)
            {
                votes[i][player - 1] = false;
                votes[player - 1][i] = false;
            }
        }

        [MonoModReplace]
        public new int CountVotes(int targetPlayer)
        {
            bool[] arr = votes[targetPlayer - 1];

            return arr.Select(x => x).Count();
        }
        
        [MonoModReplace]
        public new IEnumerable<int> VotesFromNetworkNumber(int networkNumber)
        {
            for (int i = 0; i < Constants.PlayerCount; i++)
            {
                if (i != networkNumber - 1 && votes[i][networkNumber - 1])
                {
                    yield return i + 1;
                }
            }
        }
    }
}