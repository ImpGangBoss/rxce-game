using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Physics.Aspects;

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
            //ecb.SetComponent(player, new LocalToWorld { Value = playerSpawner.GetSpawnPos() });

            ecb.Playback(state.EntityManager);
        }
    }
}
