using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

namespace RxceGame
{
    public class LevelGeneratorAuthoring : MonoBehaviour
    {
        [SerializeField] private List<GameObject> groundPrefabs;

        public List<GameObject> GroundPrefabs { get => groundPrefabs; }
    }

    public class LevelGeneratorBaker : Baker<LevelGeneratorAuthoring>
    {
        public override void Bake(LevelGeneratorAuthoring authoring)
        {
            DynamicBuffer<GroundVariant> groundEntities = AddBuffer<GroundVariant>();
            for (int i = 0; i < authoring.GroundPrefabs.Count; ++i)
                groundEntities.Add(GetEntity(authoring.GroundPrefabs[i]));

            Debug.Log(groundEntities.Capacity);
            Debug.Log(groundEntities.Length);

            AddComponent(new LevelGeneratorTag { });
        }

    }
}