using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public class GroundAuthoring : MonoBehaviour
    {
        public GameObject GameObject { get; }
    }
    public class GroundAuthoringBaker : Baker<GroundAuthoring>
    {
        public override void Bake(GroundAuthoring authoring)
        {
            AddComponent(new GroundComponent
            {
                entity = GetEntity(authoring.GameObject)
            });
        }
    }
}