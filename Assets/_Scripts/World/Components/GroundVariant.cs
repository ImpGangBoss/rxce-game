using Unity.Entities;

namespace RxceGame
{
    [InternalBufferCapacity(4)]
    public struct GroundVariant : IBufferElementData
    {
        public static implicit operator Entity(GroundVariant e) { return e.prefab; }
        public static implicit operator GroundVariant(Entity e) { return new GroundVariant { prefab = e }; }
        public Entity prefab;
    }
}