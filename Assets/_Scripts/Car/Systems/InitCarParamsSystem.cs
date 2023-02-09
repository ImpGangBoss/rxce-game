using Unity.Entities;
using Unity.Burst;

namespace RxceGame
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(PlayerSpawnSystem))]
    public partial struct InitCarParamsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            foreach (var car in SystemAPI.Query<CarAspect>())
            {
                car.SetCarParamsOnStart();
            }
        }

    }
}