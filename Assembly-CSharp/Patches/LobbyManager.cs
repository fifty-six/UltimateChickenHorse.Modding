using System;
using System.Reflection;
using MonoMod;
using UnityEngine.Networking;

namespace Modding.Patches
{
    [MonoModPatch("global::LobbyManager")]
    public class LobbyManager : global::LobbyManager
    {
        [MonoModConstructor]
        public LobbyManager()
        {
            Array.Resize(ref lobbySlots, Constants.PlayerCount);
            
            FieldInfo fi = typeof(NetworkLobbyManager).GetField("m_MaxPlayers", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fi is null)
                throw new NullReferenceException("Unable to get FieldInfo for field m_MaxPlayers on NetworkLobbyManager (LobbyManager::ctor)");
            
            fi.SetValue(this, Constants.PlayerCount);
        }
    }
}