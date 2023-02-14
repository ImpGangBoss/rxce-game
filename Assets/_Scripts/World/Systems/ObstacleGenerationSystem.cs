using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

namespace RxceGame
{
    [BurstCompile]
    public partial struct ObstacleGenerationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<ObstacleGeneratorComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var random = SystemAPI.GetSingletonRW<RandomComponent>();
            var randomNumber = random.ValueRW.random.NextFloat();
            var obstacleGenerator = SystemAPI.GetSingletonRW<ObstacleGeneratorComponent>();

            if (randomNumber > obstacleGenerator.ValueRO.spawnChance)
                return;

            var prefabsBuffer = SystemAPI.GetBuffer<Prefab>(obstacleGenerator.ValueRO.entity);
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var randomIndex = random.ValueRW.random.NextInt(0, prefabsBuffer.Length);
            ecb.Instantiate(prefabsBuffer.ElementAt(randomIndex).prefab);
            ecb.Playback(state.EntityManager);
        }
    }

}