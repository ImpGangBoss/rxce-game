using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public class GroundAuthoring : MonoBehaviour
    {
    }

    public class GroundAuthoringBaker : Baker<GroundAuthoring>
    {
        public override void Bake(GroundAuthoring authoring)
        {
            AddComponent(new GroundTag { });
        }
    }
}