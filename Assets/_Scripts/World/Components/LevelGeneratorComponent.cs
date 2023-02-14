using Unity.Entities;
using Unity.Mathematics;

namespace RxceGame
{
    public struct LevelGeneratorComponent : IComponentData
    {
        public Entity entity;
        public float3 prevSpawnPos;
        public float3 prefabSize;
        public bool isPrefabZeroSpawned;
        public int prefabZeroIndex;

    }
}