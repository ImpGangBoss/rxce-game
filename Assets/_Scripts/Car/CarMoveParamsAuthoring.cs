using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public class CarMoveParamsAuthoring : MonoBehaviour
    {
        public float acceleration;
        public float maxSpeed;
        public float jumpImpulse;
        public bool JumpTrigger { get; set; }
    }

    public class SpeedBaker : Baker<CarMoveParamsAuthoring>
    {
        public override void Bake(CarMoveParamsAuthoring authoring)
        {
            AddComponent(new CarMoveParams
            {
                acceleration = authoring.acceleration,
                maxSpeed = authoring.maxSpeed,
                jumpImpulse = authoring.jumpImpulse
            });
        }
    }
}
