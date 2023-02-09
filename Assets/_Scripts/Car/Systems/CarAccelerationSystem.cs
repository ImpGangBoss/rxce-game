using Unity.Entities;
using UnityEngine;
using Unity.Burst;

namespace RxceGame
{
    [BurstCompile]
    public partial struct CarAccelerationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var car in SystemAPI.Query<CarAspect>())
            {
                car.AddAcceleration(Time.deltaTime);
            }
        }
    }
}
