using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Modding.Patches
{
    public class LevelSelectController : global::LevelSelectController
    {
        public playerJoinIndicator[] PlayerJoinIndicators = new playerJoinIndicator[Constants.PlayerCount];

        private extern void orig_Awake();

        private void Awake()
        {
            orig_Awake();

            MaxPlayers = Constants.PlayerCount;

            if (JoinedPlayers.Length == Constants.PlayerCount)
                return;

            Debug.LogWarning($"Joined players length is not {Constants.PlayerCount}! (LevelSelectController::Awake)");
            Array.Resize(ref JoinedPlayers, Constants.PlayerCount);
        }

        private extern IEnumerator orig_createCursorForPlayer(GameObject lobbyPlayerObj, bool showCursor);

        private IEnumerator createCursorForPlayer(GameObject lobbyPlayerObj, bool showCursor)
        {
            LobbyPlayer lobbyPlayer = null;
            if (lobbyPlayerObj == null)
            {
                Debug.LogWarning("Lobby player with netid " + lobbyPlayerObj + " hasn't spawned yet.");
            }
            else
            {
                while (lobbyPlayer == null || lobbyPlayer.localNumber == 0)
                {
                    lobbyPlayer = lobbyPlayerObj.GetComponent<LobbyPlayer>();
                    yield return null;
                }
            }

            if (lobbyPlayer is null)
            {
                Debug.LogError("LobbyPlayer is null! (LevelSelectController::createCursorForPlayer)");
                yield break;
            }

            if (CursorSpawnPoint.Length - 1 < lobbyPlayer.networkNumber - 1)
            {
                Debug.LogError($"CursorSpawnPoint array too small ({CursorSpawnPoint.Length}, resizing!");
                Array.Resize(ref CursorSpawnPoint, CursorSpawnPoint.Length + 1);
                CursorSpawnPoint[CursorSpawnPoint.Length - 1] = CursorSpawnPoint[CursorSpawnPoint.Length - 2];
            }

            Cursor cursor = Instantiate(CursorPrefab, CursorSpawnPoint[lobbyPlayer.networkNumber - 1].position, Quaternion.identity);
            cursor.NetworknetworkNumber = lobbyPlayer.networkNumber;
            cursor.NetworklocalNumber = lobbyPlayer.localNumber;
            cursor.CursorColor = lobbyPlayer.PlayerColor;
            cursor.SetBounds(CursorBounds);
            cursor.UseCamera = MainCamera.GetComponent<Camera>();
            if (!showCursor)
            {
                cursor.Disable(false);
            }
            else
            {
                lobbyPlayer.PlayerStatus = LobbyPlayer.Status.CURSOR;
            }

            NetworkServer.SpawnWithClientAuthority(cursor.gameObject, lobbyPlayer.gameObject);
            AkSoundEngine.PostEvent("UI_Lobby_Cursor_Creation_Poof", gameObject);
            while (cursor.netId.IsEmpty())
            {
                yield return null;
            }

            lobbyPlayer.CallCmdAssignCursor(cursor.gameObject, lobbyPlayer.networkNumber, lobbyPlayer.localNumber);
            if (NetworkServer.SpawnWithClientAuthority(cursor.gameObject, lobbyPlayerObj))
            {
                Debug.Log("Spawning lobby cursor");
            }
            else
            {
                Debug.LogError("Lobby Cursor not spawned!");
            }

            if (lobbyPlayer.IsLocalPlayer && lobbyPlayer.LocalPlayer != null && lobbyPlayer.LocalPlayer.UseController != null)
            {
                cursor.SetLocalController(lobbyPlayer.LocalPlayer.UseController);
            }
        }
    }
}