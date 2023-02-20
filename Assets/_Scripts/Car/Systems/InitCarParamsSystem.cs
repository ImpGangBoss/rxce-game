using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;

namespace RxceGame
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(PlayerSpawnSystem))]
    public partial struct InitCarParamsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var car in SystemAPI.Query<CarAspect>())
            {
                //TODO: change to real position
                float3 spawnPos = float3.zero;
                if (SystemAPI.HasComponent<PlayerTag>(car.Entity()))
                    spawnPos = SystemAPI.GetSingleton<PlayerSpawnData>().spawnPos;
                car.SetCarParamsOnStart(spawnPos);
            }
        }
    }
}