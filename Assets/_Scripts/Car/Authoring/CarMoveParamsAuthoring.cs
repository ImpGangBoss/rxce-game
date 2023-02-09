using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace RxceGame
{
    public class CarMoveParamsAuthoring : MonoBehaviour
    {
        [SerializeField] private MoveParamsData carConfig;

        public MoveParamsData CarConfig { get => carConfig; }
    }

    public class SpeedBaker : Baker<CarMoveParamsAuthoring>
    {
        public override void Bake(CarMoveParamsAuthoring authoring)
        {
            AddComponent(new CarMoveParams
            {
                mass = authoring.CarConfig.Mass,
                acceleration = authoring.CarConfig.Acceleration,
                maxSpeed = authoring.CarConfig.MaxSpeed,
                jumpImpulse = authoring.CarConfig.JumpImpulse,
                rotationSpeed = authoring.CarConfig.RotationSpeed,
                brakeSpeed = authoring.CarConfig.BrakeSpeed
            });
        }
    }
}
