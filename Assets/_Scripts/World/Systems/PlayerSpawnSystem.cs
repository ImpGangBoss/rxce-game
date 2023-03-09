using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

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
            state.RequireForUpdate<CarPrefabsHolderComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        public void OnUpdate(ref SystemState state)
        {
            if (SceneLoader.Instance.EnablePlayerSpawner)
            {
                var carHolder = SystemAPI.GetSingletonEntity<CarPrefabsHolderComponent>();
                var prefabs = SystemAPI.GetBuffer<Prefab>(carHolder);
                var carIndex = PlayerPrefs.GetInt("Car", 0);

                var ecb = new EntityCommandBuffer(Allocator.Temp);

                Debug.Log("Player spawned");

                var player = ecb.Instantiate(prefabs.ElementAt(carIndex));
                ecb.AddComponent<PlayerTag>(player);

                ecb.Playback(state.EntityManager);
            }
        }
    }
}
