using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

namespace RxceGame
{
    public partial struct JumpSystem : ISystem
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
                //if (carParams.ValueRO.JumpTrigger)
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
                {
                    // Jump by applying an impulse on y Axis
                    velocity.ValueRW.ApplyLinearImpulse(mass, new float3(0, carParams.ValueRO.jumpImpulse, 0));

                    // Consume trigger
                    carParams.ValueRW.JumpTrigger = false;
                }
            }
        }

    }
}