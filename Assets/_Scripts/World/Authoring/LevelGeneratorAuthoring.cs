using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

namespace RxceGame
{
    public class LevelGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private List<GameObject> groundPrefabs;
        [SerializeField] private Vector3 prefabZeroPos;
        [SerializeField] private Vector3 prefabZeroSize;

        public List<GameObject> GroundPrefabs { get => groundPrefabs; }
        public Vector3 PrefabZeroPos { get => prefabZeroPos; }
        public Vector3 PrefabZeroSize { get => prefabZeroSize; }
        public GameObject GameObject { get => gameObject; }
    }

    public class LevelGeneratorBaker : Baker<LevelGeneratorAuthoring>
    {
        public override void Bake(LevelGeneratorAuthoring authoring)
        {
            DynamicBuffer<GroundVariant> groundEntities = AddBuffer<GroundVariant>();
            for (int i = 0; i < authoring.GroundPrefabs.Count; ++i)
                groundEntities.Add(GetEntity(authoring.GroundPrefabs[i]));

            AddComponent(new LevelGeneratorComponent
            {
                entity = GetEntity(authoring.GameObject),
                prevSpawnPos = authoring.PrefabZeroPos,
                prefabSize = authoring.PrefabZeroSize
            });
        }

    }
}