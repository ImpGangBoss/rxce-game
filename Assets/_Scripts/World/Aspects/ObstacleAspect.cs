using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace RxceGame
{
    public readonly partial struct ObstacleAspect : IAspect
    {
        private readonly Entity _entity;
        private readonly TransformAspect _transformAspect;
        private readonly RefRW<ObstacleComponent> _obstacleComponent;

        public Entity Entity() => _entity;
        public void SetSpawnPos(float3 pos)
        {
            _transformAspect.WorldPosition = pos;
            _obstacleComponent.ValueRW.isSpawned = true;
        }

        public bool IsSpawned() => _obstacleComponent.ValueRO.isSpawned;

    }
}