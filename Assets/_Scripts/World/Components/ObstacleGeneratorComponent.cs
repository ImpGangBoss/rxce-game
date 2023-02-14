using Unity.Entities;
using Unity.Mathematics;

namespace RxceGame
{
    public struct ObstacleGeneratorComponent : IComponentData
    {
        public Entity entity;
        public float3 spawnOffset;
        public float spawnChance;
    }
}