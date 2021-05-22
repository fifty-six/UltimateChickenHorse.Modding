using System;
using MonoMod;
using UnityEngine;
using UnityEngine.Networking.Match;

namespace Modding.Patches
{
    public class UnityMatchMaker : global::UnityMatchmaker

    {
        [MonoModReplace]
        protected new void CreateUnityMatch()
        {
            this.unityLobbyname = "UCH Lobby" + UnityEngine.Random.Range(0, 1000).ToString();
            this.networkMatch.baseUri = new Uri(GameSettings.GetRelayURIRegionString(this.SessionLockedUnityRelayRegion));
            Debug.Log("U: Trying to create Unity lobby " + this.unityLobbyname + " - in server region: " + GameSettings.GetUnityServerRegionName(this.SessionLockedUnityRelayRegion));
            this.networkMatch.CreateMatch(this.unityLobbyname, Constants.PlayerCount, true, string.Empty, string.Empty, string.Empty, 0, 1, new NetworkMatch.DataResponseDelegate<MatchInfo>(this.onUnityMatchCreate));
        }
        
    }
}