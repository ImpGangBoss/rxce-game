using Unity.Entities;

namespace RxceGame
{
    [InternalBufferCapacity(8)]
    public struct Prefab : IBufferElementData
    {
        public static implicit operator Entity(Prefab e) { return e.prefab; }
        public static implicit operator Prefab(Entity e) { return new Prefab { prefab = e }; }
        public Entity prefab;
    }
}