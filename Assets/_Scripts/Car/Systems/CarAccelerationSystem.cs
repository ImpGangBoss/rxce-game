using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;
using Unity.Physics.GraphicsIntegration;
using Unity.Transforms;

namespace RxceGame
{
    public partial struct CarAccelerationSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }

        public void OnDestroy(ref SystemState state) { }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var car in SystemAPI.Query<CarAspect>())
            {
                car.AddAcceleration(Time.deltaTime);
            }
        }
    }
}
