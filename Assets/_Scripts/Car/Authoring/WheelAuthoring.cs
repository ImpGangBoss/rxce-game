using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public class WheelAuthoring : MonoBehaviour
    {
    }

    public class WheelAuthoringBaker : Baker<WheelAuthoring>
    {
        public override void Bake(WheelAuthoring authoring)
        {
        }
    }
}