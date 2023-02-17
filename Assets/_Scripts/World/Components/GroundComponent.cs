using Unity.Entities;
using Unity.Mathematics;

namespace RxceGame
{
    public struct GroundComponent : IComponentData
    {
        public float3 pos;
        public bool isPlaced;
    }
}