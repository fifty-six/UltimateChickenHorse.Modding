using System;
using MonoMod;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
// ReSharper disable MemberCanBePrivate.Global

namespace Modding.Patches
{
    [MonoModPatch("global::LobbyManager")]
    public class LobbyManager : global::LobbyManager
    {
        [MonoModConstructor]
        public LobbyManager()
        {
            Array.Resize(ref lobbySlots, Constants.PlayerCount);

            maxPlayers = Constants.PlayerCount;
        }

        public extern void orig_OnServerAddPlayer(NetworkConnection conn, short playerControllerId);

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            // Gets overwritten by serialization or something, gotta set it again.
            maxPlayers = Constants.PlayerCount;

            // Same deal.
            if (lobbySlots.Length != Constants.PlayerCount)
            {
                Array.Resize(ref lobbySlots, Constants.PlayerCount);
            }

            orig_OnServerAddPlayer(conn, playerControllerId);
        }

        public extern GameObject orig_OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId);

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            var settings = global::GameSettings.GetInstance();
            
            if (settings.PlayerColors.Length == Constants.PlayerCount) 
                return orig_OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
            
            Array.Resize(ref settings.PlayerColors, Constants.PlayerCount);
            
            for (int i = 4; i < Constants.PlayerCount; i++)
            {
                settings.PlayerColors[i] = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            }

            return orig_OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
        }

    }
}