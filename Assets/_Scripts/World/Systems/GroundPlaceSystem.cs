using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Transforms;

namespace RxceGame
{
    [BurstCompile]
    public partial struct GroundPlaceSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GroundComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var groundAspect in SystemAPI.Query<GroundAspect>())
                if (!groundAspect.IsPlaced())
                    groundAspect.SetSpawnPosition();
        }
    }
}