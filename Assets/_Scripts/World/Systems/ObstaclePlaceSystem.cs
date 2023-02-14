using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;

namespace RxceGame
{
    [BurstCompile]
    public partial struct ObstaclePlaceSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ObstacleComponent>();
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var obstacle in SystemAPI.Query<ObstacleAspect>())
            {
                if (obstacle.IsSpawned())
                    return;

                var playerPos = float3.zero;

                foreach (var (playerCar, car) in
                SystemAPI.Query<PlayerTag, CarAspect>())
                    playerPos = car.Position();

                var generator = SystemAPI.GetSingletonRW<ObstacleGeneratorComponent>();

                obstacle.SetSpawnPos(playerPos + generator.ValueRO.spawnOffset);
            }
        }
    }
}