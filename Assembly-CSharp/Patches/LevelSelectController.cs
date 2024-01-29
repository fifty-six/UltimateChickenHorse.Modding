using System;
using System.Collections;
using MonoMod;
using UnityEngine;
using UnityEngine.Networking;
// ReSharper disable UnusedMember.Local

namespace Modding.Patches
{
    [MonoModPatch("global::LevelSelectController")]
    public class LevelSelectController : global::LevelSelectController
    {
        [MonoModConstructor]
        public LevelSelectController()
        {
            PlayerJoinIndicators = new playerJoinIndicator[Constants.PlayerCount];
            JoinedPlayers = new LobbyPlayer[Constants.PlayerCount];
        }

        private extern void orig_Awake();

        private void Awake()
        {
            if (snapshotPortals.Length != Constants.PlayerCount)
            {
                Array.Resize(ref snapshotPortals, Constants.PlayerCount);
                for (int i = 4; i < Constants.PlayerCount; i++)
                {
                    Debug.LogWarning($"snapShotPortals[{i}] is null. Instantiating clone - is this valid(?).");
                    snapshotPortals[i] = Instantiate(snapshotPortals[i - 1]);
                }
            }
            
            orig_Awake();

            MaxPlayers = Constants.PlayerCount;

            if (JoinedPlayers.Length != Constants.PlayerCount)
            {
                Debug.LogWarning($"Joined players length is not {Constants.PlayerCount}! (LevelSelectController::Awake)");
                Array.Resize(ref JoinedPlayers, Constants.PlayerCount);
                return;
            }

            for (int i = 4; i < Constants.PlayerCount; i++)
            {
                if (JoinedPlayers[i] != null)
                    continue;

                Debug.LogWarning($"JoinedPlayers[{i}] is null. Instantiating clone.");

                JoinedPlayers[i] = Instantiate(JoinedPlayers[i - 1]);
            }
        }

        private extern void orig_TryAddLocalPlayer(Controller sender);

        private void TryAddLocalPlayer(Controller sender)
        {
            if (PlayerJoinIndicators.Length != Constants.PlayerCount)
            {
                Array.Resize(ref PlayerJoinIndicators, Constants.PlayerCount);

                for (int i = 4; i < Constants.PlayerCount; i++)
                {
                    PlayerJoinIndicators[i] = Instantiate(PlayerJoinIndicators[i - 1]);
                }
            }

            orig_TryAddLocalPlayer(sender);
        }
        
        [MonoModReplace]
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

        // Method used to throw, patch in some logs
        [MonoModReplace]
        private IEnumerator setupLobbyCursor(GameObject lobbyCursorObj)
        {
            LobbyCursor lobbyCursor;
            
            do
            {
                lobbyCursor = lobbyCursorObj.GetComponent<LobbyCursor>();
                yield return null;
            }
            while (lobbyCursor == null || lobbyCursor.AssociatedLobbyPlayer == null);

            lobbyCursor.SetBounds(CursorBounds);
            
            Debug.LogError("Before Instantiate (LevelSelectController::setupLobbyCursor)");
            
            var componentInChildren = Instantiate
                (
                    magicSmoke,
                    CursorSpawnPoint[lobbyCursor.AssociatedLobbyPlayer.networkNumber - 1].position,
                    Quaternion.identity
                )
                .GetComponentInChildren<SpriteRenderer>();
            
            Debug.LogError("After Instantiate (LevelSelectController::setupLobbyCursor)");
            
            componentInChildren.color = lobbyCursor.AssociatedLobbyPlayer.PlayerColor;
            componentInChildren.gameObject.layer = LayerMask.NameToLayer("LobbyCursors");
            
            try
            {
                PlayerJoinIndicators[lobbyCursor.networkNumber - 1].setTintColor(lobbyCursor.AssociatedLobbyPlayer.PlayerColor);
                PlayerJoinIndicators[lobbyCursor.networkNumber - 1].ChooseCharacterEnabled();
            }
            catch (NullReferenceException)
            {
                Debug.LogError("Player join indicator is null!");
            }

            lobbyCursor.AssociatedLobbyPlayer.RunAfterInitialized
            (
                () =>
                {
                    if (!lobbyCursor.AssociatedLobbyPlayer.IsLocalPlayer)
                        return;

                    Player player = global::PlayerManager.GetInstance().GetPlayer(lobbyCursor.localNumber);
                    lobbyCursor.LocalPlayer = player;
                    lobbyCursor.UseCamera = MainCamera.GetComponent<Camera>();
                    player.PlayerCursor = lobbyCursor;
                }
            );

            MainCamera.AddTarget(lobbyCursor);
        }
    }
}