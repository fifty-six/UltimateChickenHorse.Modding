using MonoMod;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable 649
#pragma warning disable 414

namespace Modding.Patches
{
    [MonoModPatch("global::PartyBox")]
    public class PartyBox : global::PartyBox
    {
        [MonoModIgnore]
        private bool partybox_debug;
        
        [MonoModIgnore]
        private PartyPickCursor[] cursors;
        
        [MonoModConstructor]
        public PartyBox()
        {
            partybox_debug = true;
        }

        public extern void orig_handleEvent(GameEvent.GameEvent e);

        public new void handleEvent(GameEvent.GameEvent e)
        {
            Debug.Log($"PartyBox::handleEvent, {cursors.Length}");
            orig_handleEvent(e);
            Debug.Log($"PartyBox::handleEvent, {cursors.Length}");
        }

        public extern void orig_SetPlayerCount(int players);

        public new void SetPlayerCount(int players)
        {
            Debug.Log($"PartyBox::SetPlayerCount, {cursors.Length}");
            orig_SetPlayerCount(players);
            Debug.Log($"PartyBox::SetPlayerCount, {cursors.Length}");
        }

        public extern void orig_ShowBox(bool extraBox);

        public new void ShowBox(bool extraBox)
        {
            Debug.Log($"PartyBox::ShowBox, {cursors.Length}");
            orig_ShowBox(extraBox);
            Debug.Log($"PartyBox::ShowBox, {cursors.Length}");
        }

        public extern PartyPickCursor orig_AddPlayer(int playerNumber, global::Character.Animals animal);
        
        public new PartyPickCursor AddPlayer(int playerNumber, global::Character.Animals animal)
        {
            Debug.Log($"PartyBox::AddPlayer, {cursors.Length}");
            
            PartyPickCursor res = orig_AddPlayer(playerNumber, animal);
            
            Debug.Log($"PartyBox::AddPlayer, {cursors.Length}");

            return res;
        }
    }
}