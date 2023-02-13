using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

namespace RxceGame
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct PlayerSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerSpawnData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var playerSpawnerEntity = SystemAPI.GetSingletonEntity<PlayerSpawnData>();
            var playerSpawner = SystemAPI.GetAspectRW<PlayerSpawnerAspect>(playerSpawnerEntity);

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var player = ecb.Instantiate(playerSpawner.PlayerCarPrefab());
            ecb.AddComponent<PlayerTag>(player);

            ecb.Playback(state.EntityManager);
        }
    }
}
