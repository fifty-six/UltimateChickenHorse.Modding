using MonoMod;
using Steamworks;
using UnityEngine;

namespace Modding.Patches
{
    public class SteamMatchmaker : global::SteamMatchmaker
    {
        [MonoModIgnore]
        private CallResult<LobbyCreated_t> lobbyCreatedResult;
        
        protected override void createSocialLobby()
        {
            base.createSocialLobby();
            ELobbyType eLobbyType;
            switch (global::GameSettings.GetInstance().lobbyPrivacy)
            {
                case MatchmakingLobby.Visibility.PUBLIC:
                    eLobbyType = ELobbyType.k_ELobbyTypePublic;
                    break;
                case MatchmakingLobby.Visibility.FRIENDS:
                    eLobbyType = ELobbyType.k_ELobbyTypeFriendsOnly;
                    break;
                case MatchmakingLobby.Visibility.PRIVATE:
                    eLobbyType = ELobbyType.k_ELobbyTypePrivate;
                    break;
                default:
                    eLobbyType = ELobbyType.k_ELobbyTypePublic;
                    break;
            }
            Debug.Log("[Net] Starting " + GameSettings.GetInstance().lobbyPrivacy + " Steam lobby.");
            SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby(eLobbyType, Constants.PlayerCount);
            this.lobbyCreatedResult.Set(hAPICall, null);
        }
    }
}