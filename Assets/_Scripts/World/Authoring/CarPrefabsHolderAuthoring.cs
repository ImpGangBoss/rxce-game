using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

namespace RxceGame
{
    public class CarPrefabsHolderAuthoring : MonoBehaviour
    {
        [SerializeField] private List<GameObject> carPrefabs;
        public GameObject GameObject { get; }
        public List<GameObject> CarPrefabs { get => carPrefabs; }
    }

    public class CarPrefabsHolderAuthoringBaker : Baker<CarPrefabsHolderAuthoring>
    {
        public override void Bake(CarPrefabsHolderAuthoring authoring)
        {
            DynamicBuffer<Prefab> groundEntities = AddBuffer<Prefab>();
            for (int i = 0; i < authoring.CarPrefabs.Count; ++i)
                groundEntities.Add(GetEntity(authoring.CarPrefabs[i]));

            AddComponent(new CarPrefabsHolderComponent
            {
                entity = GetEntity(authoring.GameObject)
            });
        }
    }
}
