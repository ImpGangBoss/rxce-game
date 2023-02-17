using Unity.Entities;
using Unity.Mathematics;

namespace RxceGame
{
    public struct PlayerSpawnData : IComponentData
    {
        public float3 spawnPos;
    }
}
