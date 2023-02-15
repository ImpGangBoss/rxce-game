using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

namespace RxceGame
{
    public class PlayerSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform spawnPos;
        [SerializeField] private List<GameObject> cars;

        public List<GameObject> CarPrefabs => cars;
        public Vector3 SpawnPos { get => spawnPos.position; }
    }

    public class PlayerSpawnBaker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            AddComponent(new PlayerSpawnData
            {
                playerCar = GetEntity(authoring.CarPrefabs[PlayerPrefs.GetInt("Car")]),
                spawnPos = authoring.SpawnPos
            });
        }
    }
}
