using Unity.Entities;
using Unity.Mathematics;

namespace RxceGame
{
    public struct PlayerSpawnData : IComponentData
    {
        public Entity playerCar;
        public float3 spawnPos;
    }
}
