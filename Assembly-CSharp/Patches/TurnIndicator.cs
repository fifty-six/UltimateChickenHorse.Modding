using MonoMod;
using UnityEngine;

namespace Modding.Patches
{
    public class TurnIndicator : global::TurnIndicator
    {
        [MonoModReplace]
        public new Transform[] PortraitPositions = new Transform[Constants.PlayerCount];
        
        [MonoModReplace]
        public new void SetPlayerCount(int players)
        {
            if (players is 0 or > Constants.PlayerCount)
            {
                Debug.LogError("TurnIndicator.SetPlayerCount: invalid number of players: " + players);
                return;
            }
            Portraits = new CharacterPortrait[players];
            for (int num = 0; num != players; num++)
            {
                GameObject go = Instantiate(PortraitPrefab.gameObject, PortraitPositions[num].position, Quaternion.identity);
                go.transform.parent = transform;
                if (num > 0)
                {
                    go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                }
                Portraits[num] = go.GetComponent<CharacterPortrait>();
            }
        }
        
        [MonoModReplace]
        public new void SetPlayerCharacter(int player, global::Character.Animals character)
        {
            if (player is < 0 or > Constants.PlayerCount - 1)
            {
                return;
            }
            int num = character - global::Character.Animals.CHICKEN;
            CharacterPortrait characterPortrait = this.Portraits[player];
            characterPortrait.Icons = new Sprite[2];
            characterPortrait.Icons[0] = this.BuildSprites[num];
            characterPortrait.Icons[1] = this.RunSprites[num];
            characterPortrait.Icon.sprite = characterPortrait.Icons[characterPortrait.CurrentPhase];
            characterPortrait.Name.sprite = this.NameSprites[num];
        }
        
        [MonoModReplace]
        public new void SwapPhase(int player)
        {
            Debug.Log("Swapping phase for " + player);
            if (player is < 0 or > Constants.PlayerCount - 1)
            {
                return;
            }
            this.Portraits[player].SwapPhase();
        }
    }
}