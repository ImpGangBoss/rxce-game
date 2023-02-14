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
        [SerializeField] private int prefabZeroIndex;

        public List<GameObject> GroundPrefabs { get => groundPrefabs; }
        public Vector3 PrefabZeroPos { get => prefabZeroPos; }
        public Vector3 PrefabZeroSize { get => prefabZeroSize; }
        public int PrefabZeroIndex { get => prefabZeroIndex; }
        public GameObject LocalGameObject { get => gameObject; }
    }

    public class LevelGeneratorBaker : Baker<LevelGeneratorAuthoring>
    {
        public override void Bake(LevelGeneratorAuthoring authoring)
        {
            DynamicBuffer<Prefab> groundEntities = AddBuffer<Prefab>();
            for (int i = 0; i < authoring.GroundPrefabs.Count; ++i)
                groundEntities.Add(GetEntity(authoring.GroundPrefabs[i]));

            AddComponent(new LevelGeneratorComponent
            {
                entity = GetEntity(authoring.LocalGameObject),
                prevSpawnPos = authoring.PrefabZeroPos,
                prefabSize = authoring.PrefabZeroSize,
                prefabZeroIndex = authoring.PrefabZeroIndex
            });
        }

    }
}