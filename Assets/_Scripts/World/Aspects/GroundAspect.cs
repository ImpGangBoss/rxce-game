using Unity.Entities;
using Unity.Transforms;
using Unity.Physics.Aspects;

namespace RxceGame
{
    public readonly partial struct GroundAspect : IAspect
    {
        private readonly Entity _entity;
        private readonly TransformAspect _transformAspect;
        private readonly RefRW<GroundComponent> _groundComponent;

        public Entity Entity() => _entity;

        public void SetSpawnPosition()
        {
            _transformAspect.WorldPosition = _groundComponent.ValueRO.pos;
            _groundComponent.ValueRW.isPlaced = true;
        }

        public bool IsPlaced() => _groundComponent.ValueRO.isPlaced;
    }
}