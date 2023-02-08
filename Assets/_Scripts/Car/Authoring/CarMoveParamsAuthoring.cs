using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public class CarMoveParamsAuthoring : MonoBehaviour
    {
        public float mass;
        public float acceleration;
        public float maxSpeed;
        public float jumpImpulse;
        public float rotationSpeed;
        public float brakeSpeed;
    }

    public class SpeedBaker : Baker<CarMoveParamsAuthoring>
    {
        public override void Bake(CarMoveParamsAuthoring authoring)
        {
            AddComponent(new CarMoveParams
            {
                mass = authoring.mass,
                acceleration = authoring.acceleration,
                maxSpeed = authoring.maxSpeed,
                jumpImpulse = authoring.jumpImpulse,
                rotationSpeed = authoring.rotationSpeed,
                brakeSpeed = authoring.brakeSpeed
            });
        }
    }
}
