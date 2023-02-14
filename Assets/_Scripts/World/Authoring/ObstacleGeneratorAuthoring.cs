using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

namespace RxceGame
{
    public class ObstacleGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private List<GameObject> obstaclePrefabs;
        [SerializeField] private Vector3 spawnOffset;
        [Range(0f, 1f)][SerializeField] private float spawnRate;

        public List<GameObject> ObstaclePrefabs { get => obstaclePrefabs; }
        public GameObject LocalGameObject { get => gameObject; }
        public Vector3 SpawnOffset { get => spawnOffset; }
        public float SpawnRate { get => spawnRate; }
    }

    public class ObstacleGeneratorBaker : Baker<ObstacleGeneratorAuthoring>
    {
        public override void Bake(ObstacleGeneratorAuthoring authoring)
        {
            DynamicBuffer<Prefab> obstacles = AddBuffer<Prefab>();
            for (int i = 0; i < authoring.ObstaclePrefabs.Count; ++i)
                obstacles.Add(GetEntity(authoring.ObstaclePrefabs[i]));

            AddComponent(new ObstacleGeneratorComponent
            {
                spawnOffset = authoring.SpawnOffset,
                entity = GetEntity(authoring.LocalGameObject),
                spawnChance = authoring.SpawnRate * Time.deltaTime

            });
        }
    }
}