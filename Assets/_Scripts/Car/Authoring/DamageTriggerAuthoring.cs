using UnityEngine;
using Unity.Entities;

namespace RxceGame
{
    public class DamageTriggerAuthoring : MonoBehaviour
    {

    }

    public class DamageTriggerBaker : Baker<DamageTriggerAuthoring>
    {
        public override void Bake(DamageTriggerAuthoring authoring)
        {
            AddComponent(new DamageTriggerTag { });
        }
    }

}