using UnityEngine;

namespace Modding.Patches
{
    public class Character : global::Character
    {
        public new void CreateFlies()
        {
            for (int i = 0; i < Random.Range(1, Constants.PlayerCount); i++)
            {
                Fly fly = Instantiate(zombieFliePrefab, transform.position, Quaternion.identity);
                fly.Initialize(this);
                spawnedFlies.Add(fly);
            }
        }
    }
}