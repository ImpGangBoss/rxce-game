using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

namespace RxceGame
{
    public partial struct CarAccelerationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        public void OnDestroy(ref SystemState state)
        {

        }

        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = Time.deltaTime;

            foreach ((RefRW<PhysicsVelocity> velocity, PhysicsMass mass, RefRW<CarMoveParams> carParams) in SystemAPI.Query<RefRW<PhysicsVelocity>, PhysicsMass, RefRW<CarMoveParams>>())
            {
                if (velocity.ValueRO.Linear.x < carParams.ValueRO.maxSpeed)
                {
                    velocity.ValueRW.ApplyLinearImpulse(mass, new float3(carParams.ValueRO.acceleration, 0f, 0f));
                }
            }

        }
    }
}
