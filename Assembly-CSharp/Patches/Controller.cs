using MonoMod;
using UnityEngine;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Modding.Patches
{
    public abstract class Controller : global::Controller
    {
        protected new Character.Animals[] associatedChars = new Character.Animals[Constants.PlayerCount];

        private int _mask = (1 << Constants.PlayerCount + 1) - 1;

        [MonoModReplace]
        public new void AddPlayer(int player)
        {
            if (player is < 1 or > Constants.PlayerCount)
            {
                Debug.LogWarning($"Tried to add player greater than {Constants.PlayerCount}!");
                return;
            }

            Player |= 1 << (player - 1 & _mask);
        }

        [MonoModReplace]
        public new int GetLastPlayerNumber()
        {
            for (int i = Constants.PlayerCount - 1; i >= 0; i--)
            {
                if ((Player & 1 << (i & _mask)) > 0)
                    return i + 1;
            }

            return 0;
        }
        
        [MonoModReplace]
        public new int GetLastPlayerNumberAfter(int lastPlayerNumber)
        {
            bool found = false;
            
            for (int i = Constants.PlayerCount - 1; i >= 0; i--)
            {
                if ((Player & 1 << (i & _mask)) <= 0 || (!found && lastPlayerNumber != i + 1)) 
                    continue;
                
                if (found)
                    return i + 1;

                found = true;
            }

            return 0;
        }
        
        [MonoModReplace]
        public new void RemovePlayer(int player)
        {
            Player &= ((1 << Constants.PlayerCount) - 1) ^ 1 << (player - 1 & _mask);

            associatedChars[player - 1] = Character.Animals.NONE;

            if (Player == 0)
                PossibleNetWorkNumber = 0;
        }
    }
}