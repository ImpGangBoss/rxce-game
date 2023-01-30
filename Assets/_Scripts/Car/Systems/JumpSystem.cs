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

            foreach (var (velocity, mass, carParams) in
            SystemAPI.Query<RefRW<PhysicsVelocity>, PhysicsMass, RefRW<CarMoveParams>>())
            {
                if (carParams.ValueRO.JumpTrigger)
                {
                    velocity.ValueRW.ApplyLinearImpulse(mass, new float3(0, carParams.ValueRO.jumpImpulse, 0));
                    carParams.ValueRW.JumpTrigger = false;
                }
            }
        }

    }
}