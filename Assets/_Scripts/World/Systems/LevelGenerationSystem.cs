using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Physics.Aspects;

namespace RxceGame
{
    [BurstCompile]
    public partial struct LevelGenerationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<LevelGeneratorComponent>();
            state.RequireForUpdate<RandomComponent>();
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerPos = float3.zero;

            foreach (var (playerCar, car) in
            SystemAPI.Query<PlayerTag, CarAspect>())
                playerPos = car.Position();

            var random = SystemAPI.GetSingletonRW<RandomComponent>();
            var levelGenerator = SystemAPI.GetSingletonRW<LevelGeneratorComponent>();
            var prefabsBuffer = SystemAPI.GetBuffer<GroundVariant>(levelGenerator.ValueRO.entity);
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            if (math.distance(playerPos, levelGenerator.ValueRO.prevSpawnPos) > levelGenerator.ValueRO.prefabSize.x * 2)
                return;

            var randomIndex = random.ValueRW.random.NextInt(0, prefabsBuffer.Length);
            var newGround = ecb.Instantiate(prefabsBuffer.ElementAt(randomIndex).prefab);

            float3 spawnPos = new float3(
                levelGenerator.ValueRO.prefabSize.x + levelGenerator.ValueRO.prevSpawnPos.x,
                levelGenerator.ValueRO.prevSpawnPos.y,
                0f);

            levelGenerator.ValueRW.prevSpawnPos = spawnPos;

            var newPos = new float4x4(quaternion.identity, spawnPos);
            ecb.SetComponent(newGround, new LocalToWorld { Value = newPos });

            ecb.Playback(state.EntityManager);

        }
    }
}