using UnityEngine;
using Unity.Entities;

namespace RxceGame
{
    public class PlayerSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform spawnPos;

        public Vector3 SpawnPos { get => spawnPos.position; }
    }

    public class PlayerSpawnBaker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            AddComponent(new PlayerSpawnData
            {
                spawnPos = authoring.SpawnPos
            });
        }
    }
}
