using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace RxceGame
{
    public class PlayerSpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject playerCarPrefab;
        [SerializeField] private Transform spawnPos;

        public GameObject PlayerCarPrefab { get => playerCarPrefab; set => playerCarPrefab = value; }
        public Vector3 SpawnPos { get => spawnPos.position; }
    }

    public class PlayerSpawnBaker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            AddComponent(new PlayerSpawnData
            {
                playerCar = GetEntity(authoring.PlayerCarPrefab),
                spawnPos = authoring.SpawnPos
            });
        }
    }
}
